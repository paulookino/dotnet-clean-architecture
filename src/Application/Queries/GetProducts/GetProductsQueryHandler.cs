using Application.Queries.GetProduct;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductResponse>>
{
    public async Task<IReadOnlyList<ProductResponse>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await productRepository.GetActiveAsync(cancellationToken);

        return products.Select(p => new ProductResponse(
            p.Id, p.Name, p.Description, p.Price, p.Stock, p.IsActive, p.CreatedAt))
            .ToList();
    }
}
