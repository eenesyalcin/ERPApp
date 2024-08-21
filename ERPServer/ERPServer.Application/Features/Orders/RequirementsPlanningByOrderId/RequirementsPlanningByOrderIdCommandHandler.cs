﻿using ERPServer.Domain.Dtos;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.RequirementsPlanningByOrderId;

internal sealed class RequirementsPlanningByOrderIdCommandHandler(
    IOrderRepository orderRepository,
    IStockMovementRepository stockMovementRepository,
    IRecipeRepository recipeRepository) : IRequestHandler<RequirementsPlanningByOrderIdCommand, Result<RequirementsPlanningByOrderIdCommandResponse>>
{
    public async Task<Result<RequirementsPlanningByOrderIdCommandResponse>> Handle(RequirementsPlanningByOrderIdCommand request, CancellationToken cancellationToken)
    {
        Order? order = 
            await orderRepository
            .Where(p => p.Id == request.OrderId)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Result<RequirementsPlanningByOrderIdCommandResponse>.Failure("Sipariş bulunamadı!");
        }

        List<ProductDto> ListOfProductsToProduced = new();
        List<ProductDto> requirementsPlanningProducts = new();

        if (order.Details is not null)
        {
            foreach (var item in order.Details)
            {
                var product = item.Product;
                List<StockMovement> movements = 
                    await stockMovementRepository
                    .Where(p => p.ProductId == product!.Id)
                    .ToListAsync(cancellationToken);

                decimal stock = movements.Sum(p => p.NumberOfEntries) - movements.Sum(p => p.NumberOfOutputs);

                if (stock < item.Quantity)
                {
                    ProductDto ProductToProduced = new()
                    {
                        Id = item.ProductId,
                        Name = product!.Name,
                        Quantity = item.Quantity - stock
                    };

                    ListOfProductsToProduced.Add(ProductToProduced);
                }
            }

            foreach (var item in ListOfProductsToProduced)
            {
                Recipe? recipe =
                    await recipeRepository
                    .Where(p => p.ProductId == item.Id)
                    .Include(p => p.Details!)
                    .ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(cancellationToken);

                if (recipe is not null && recipe.Details is not null)
                {
                    foreach (var detail in recipe.Details)
                    {
                        List<StockMovement> productMovements = 
                            await stockMovementRepository
                            .Where(p => p.ProductId == detail.ProductId)
                            .ToListAsync(cancellationToken);

                        decimal stock = productMovements.Sum(p => p.NumberOfEntries) - productMovements.Sum(p => p.NumberOfOutputs);

                        if (stock < detail.Quantity)
                        {
                            ProductDto neededProduct = new()
                            {
                                Id = detail.ProductId,
                                Name = detail.Product!.Name,
                                Quantity = detail.Quantity - stock
                            };

                            requirementsPlanningProducts.Add(neededProduct);
                        }
                    }
                }
            }
        }

        requirementsPlanningProducts = requirementsPlanningProducts
            .GroupBy(p => p.Id)
            .Select(g => new ProductDto
        {
            Id = g.Key,
            Name = g.First().Name,
            Quantity = g.Sum(item => item.Quantity)
        }).ToList();

        return new RequirementsPlanningByOrderIdCommandResponse(
            DateOnly.FromDateTime(DateTime.Now), 
            order.Number + " Numaralı Siparişin İhtiyaç Planlaması", 
            requirementsPlanningProducts);
    }
}
