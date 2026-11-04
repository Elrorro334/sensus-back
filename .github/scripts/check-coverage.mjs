#!/usr/bin/env node
/**
 * Verifica el umbral de cobertura por ensamblado (constitution.md P-V).
 *
 * Uso:
 *   node .github/scripts/check-coverage.mjs <directorio> <umbral> [ensamblados...]
 *
 * Lee los reportes Cobertura generados por `dotnet test --collect:"XPlat Code Coverage"`
 * y falla si alguno de los ensamblados indicados queda por debajo del umbral de líneas.
 */
import fs from 'node:fs';
import path from 'node:path';

const DEFAULT_ASSEMBLIES = ['Sensus.Domain', 'Sensus.Application'];

function walk(dir) {
  const found = [];
  if (!fs.existsSync(dir)) return found;
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) found.push(...walk(full));
    else if (entry.name.endsWith('.cobertura.xml')) found.push(full);
  }
  return found;
}

function collectStats(xml, targets) {
  const stats = new Map(targets.map((name) => [name, { covered: 0, valid: 0 }]));
  const packageRegex = /<package\b([^>]*)>([\s\S]*?)<\/package>/g;

  for (const [, attributes, body] of xml.matchAll(packageRegex)) {
    const name = /name="([^"]*)"/.exec(attributes)?.[1] ?? '';
    const owner = targets.find((target) => name.includes(target));
    if (!owner) continue;

    const entry = stats.get(owner);
    for (const [, hits] of body.matchAll(/<line\b[^>]*\bhits="(\d+)"/g)) {
      entry.valid += 1;
      if (Number(hits) > 0) entry.covered += 1;
    }
  }

  return stats;
}

const [root, rawThreshold, ...rest] = process.argv.slice(2);
if (!root || !rawThreshold) {
  console.error(
    'Uso: node .github/scripts/check-coverage.mjs <directorio> <umbral> [ensamblados...]',
  );
  process.exit(2);
}

const threshold = Number(rawThreshold);
const targets = rest.length > 0 ? rest : DEFAULT_ASSEMBLIES;
const files = walk(root);

if (files.length === 0) {
  console.error(`ERROR: no se encontraron reportes de cobertura en ${root}`);
  process.exit(1);
}

const total = new Map(targets.map((name) => [name, { covered: 0, valid: 0 }]));

for (const file of files) {
  const stats = collectStats(fs.readFileSync(file, 'utf8'), targets);
  for (const [name, partial] of stats) {
    const entry = total.get(name);
    entry.covered += partial.covered;
    entry.valid += partial.valid;
  }
}

let failed = false;
for (const name of targets) {
  const { covered, valid } = total.get(name);
  if (valid === 0) {
    console.error(`FALLO  ${name}: sin datos de cobertura`);
    failed = true;
    continue;
  }
  const pct = (covered / valid) * 100;
  const status = pct >= threshold ? 'OK    ' : 'FALLO ';
  console.log(
    `${status}${name}: ${pct.toFixed(2)} % (${Math.round(covered)}/${valid} líneas) — umbral ${threshold} %`,
  );
  failed = failed || pct < threshold;
}

process.exit(failed ? 1 : 0);
