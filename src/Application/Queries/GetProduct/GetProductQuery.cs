using MediatR;

namespace Application.Queries.GetProduct;

public sealed record GetProductQuery(Guid Id) : IRequest<ProductResponse>;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    bool IsActive,
    DateTime CreatedAt);
