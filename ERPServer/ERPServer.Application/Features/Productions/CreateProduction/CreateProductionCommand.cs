﻿using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Productions.CreateProduction;

public sealed record CreateProductionCommand(
    Guid ProductId,
    Guid DeptId,
    decimal Quantity) : IRequest<Result<string>>;
