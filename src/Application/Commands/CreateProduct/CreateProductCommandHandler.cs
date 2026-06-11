using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var nameExists = await productRepository.ExistsByNameAsync(request.Name, cancellationToken);
        if (nameExists)
            throw new DomainException($"A product with name '{request.Name}' already exists.");

        var product = Product.Create(request.Name, request.Description, request.Price, request.Stock);

        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateProductResponse(product.Id, product.Name, product.Price, product.Stock);
    }
}
