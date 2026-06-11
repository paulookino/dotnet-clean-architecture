using Application.Commands.CreateProduct;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Unit.Application;

public sealed class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _handler = new CreateProductCommandHandler(_productRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateAndReturnProduct()
    {
        _productRepository
            .Setup(r => r.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreateProductCommand("Widget", "A widget", 9.99m, 100);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Widget");
        result.Price.Should().Be(9.99m);
        result.Stock.Should().Be(100);
        _productRepository.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateName_ShouldThrowDomainException()
    {
        _productRepository
            .Setup(r => r.ExistsByNameAsync("Duplicate", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateProductCommand("Duplicate", "desc", 1m, 0);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Domain.Exceptions.DomainException>()
            .WithMessage("*already exists*");
    }
}
