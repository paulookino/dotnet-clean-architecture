using Application.Queries.GetProduct;
using MediatR;

namespace Application.Queries.GetProducts;

public sealed record GetProductsQuery : IRequest<IReadOnlyList<ProductResponse>>;
