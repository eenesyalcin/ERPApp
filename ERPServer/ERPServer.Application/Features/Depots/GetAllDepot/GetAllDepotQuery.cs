using ERPServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.GetAllDepot;

public sealed class GetAllDepotQuery() : IRequest<Result<List<Depot>>>;
