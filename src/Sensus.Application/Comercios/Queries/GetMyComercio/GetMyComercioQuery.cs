using MediatR;
using Sensus.Application.Comercios.DTOs;
using Sensus.Domain.Common;

namespace Sensus.Application.Comercios.Queries.GetMyComercio;

/// <summary>
/// SE-004: Consulta para obtener los datos del comercio del usuario autenticado actual.
/// </summary>
public sealed record GetMyComercioQuery() : IRequest<Result<ComercioDto>>;
