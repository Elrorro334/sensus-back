using MediatR;
using Sensus.Application.Comercios.DTOs;
using Sensus.Domain.Common;

namespace Sensus.Application.Comercios.Commands.UpdateMerchantProfile;

/// <summary>
/// SE-007: Actualización de datos del local comercial (dirección, código postal y horarios).
/// Recalcula las coordenadas espaciales GeoJSON únicamente si la ubicación física cambió.
/// </summary>
public sealed record UpdateMerchantProfileCommand(
    string? NombreComercial,
    string Direccion,
    string CodigoPostal,
    List<HorarioDiaDto>? Horarios) : IRequest<Result<ComercioDto>>;
