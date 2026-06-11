using MediatR;

namespace Application.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock) : IRequest<CreateProductResponse>;

public sealed record CreateProductResponse(Guid Id, string Name, decimal Price, int Stock);
