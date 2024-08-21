using ERPServer.Domain.Dtos;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.RequirementsPlanningByOrderId;

public sealed record RequirementsPlanningByOrderIdCommand(
    Guid Id) : IRequest<Result<RequirementsPlanningByOrderIdCommandResponse>>;

public sealed record RequirementsPlanningByOrderIdCommandResponse(
    DateOnly Date,
    string Title,
    List<ProductDto> Products);

internal sealed class RequirementsPlanningByOrderIdCommandHandler(
    IOrderRepository orderRepository) : IRequestHandler<RequirementsPlanningByOrderIdCommand, Result<RequirementsPlanningByOrderIdCommandResponse>>
{
    public async Task<Result<RequirementsPlanningByOrderIdCommandResponse>> Handle(RequirementsPlanningByOrderIdCommand request, CancellationToken cancellationToken)
    {
        Order? order = 
            await orderRepository
            .Where(p => p.Id == request.Id)
            .Include(p => p.Details)
            .FirstOrDefaultAsync(cancellationToken);

        /*
            1  --->  Siparişteki ürünlerin tüm depolardaki adetlerine bakılacak.
            2  --->  Eğer yetersiz ise kaç tane üretilmesi gerektiği tespit edilecek.
            3  --->  Her bir ürün için gereken ürün reçetesi alınacak ve o ürünlerin tek tek stokların bakılack.
            4  --->  Üretilmesi için gereken ürünlerden kaç tanesine ihtiyaç olduğu tespit edilip liste olarak geri denderilecek.
        */

        return new RequirementsPlanningByOrderIdCommandResponse(DateOnly.FromDateTime(DateTime.Now), order?.Number + " Numaralı Siparişin İhtiyaç Planlaması", new());
    }
}
