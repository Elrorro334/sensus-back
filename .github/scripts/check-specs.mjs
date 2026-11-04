#!/usr/bin/env node
/**
 * Verifica la estructura SDD de las especificaciones (constitution.md §3.6).
 *
 * Uso:
 *   node .github/scripts/check-specs.mjs specs
 *
 * Comprueba que cada carpeta `NNN-slug` contiene los cuatro artefactos obligatorios,
 * que el ID aparece en los cuatro y que los requisitos usan `FR-<spec>-<nn>` /
 * `NFR-<spec>-<nn>` con el prefijo correcto.
 */
import fs from 'node:fs';
import path from 'node:path';

const ARTIFACTS = ['spec.md', 'plan.md', 'tasks.md', 'checklist.md'];
const SPEC_DIR = /^(\d{3})-[a-z0-9]+(?:-[a-z0-9]+)*$/;
const REQ_ID = /\b(?:FR|NFR)-(\d{3})-(\d{2})\b/g;
const EXCLUDED = new Set(['_templates']);

const specsRoot = process.argv[2];
if (!specsRoot) {
  console.error('Uso: node .github/scripts/check-specs.mjs <directorio_specs>');
  process.exit(2);
}

if (!fs.existsSync(specsRoot) || !fs.statSync(specsRoot).isDirectory()) {
  console.error(`ERROR: no existe el directorio ${specsRoot}`);
  process.exit(1);
}

const specs = fs
  .readdirSync(specsRoot, { withFileTypes: true })
  .filter((entry) => entry.isDirectory() && !EXCLUDED.has(entry.name))
  .map((entry) => entry.name)
  .sort();

if (specs.length === 0) {
  console.error(`ERROR: no hay especificaciones en ${specsRoot}`);
  process.exit(1);
}

const errors = [];

for (const name of specs) {
  const dir = path.join(specsRoot, name);
  const match = SPEC_DIR.exec(name);
  if (!match) {
    errors.push(`${name}: la carpeta no cumple el patrón NNN-slug`);
    continue;
  }

  const specId = match[1];

  for (const artifact of ARTIFACTS) {
    const file = path.join(dir, artifact);
    if (!fs.existsSync(file)) {
      errors.push(`${name}: falta ${artifact}`);
      continue;
    }
    const text = fs.readFileSync(file, 'utf8');
    if (!text.includes(specId)) {
      errors.push(`${name}/${artifact}: no menciona el ID ${specId}`);
    }
  }

  const specFile = path.join(dir, 'spec.md');
  if (!fs.existsSync(specFile)) continue;

  const text = fs.readFileSync(specFile, 'utf8');
  for (const [, prefix, seq] of text.matchAll(REQ_ID)) {
    if (prefix !== specId) {
      errors.push(`${name}/spec.md: requisito con prefijo ajeno (${prefix}-${seq})`);
    }
  }
}

console.log(`Especificaciones revisadas: ${specs.length}`);

if (errors.length > 0) {
  console.error(`Errores: ${errors.length}`);
  for (const error of errors) console.error(`  - ${error}`);
  process.exit(1);
}

console.log('Estructura SDD correcta.');
