using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.GetProduct;

public sealed class GetProductQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductQuery, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.IsActive,
            product.CreatedAt);
    }
}
