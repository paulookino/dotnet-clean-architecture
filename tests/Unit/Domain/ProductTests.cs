using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Unit.Domain;

public sealed class ProductTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateProduct()
    {
        var product = Product.Create("Test Product", "Description", 9.99m, 100);

        product.Should().NotBeNull();
        product.Name.Should().Be("Test Product");
        product.Price.Should().Be(9.99m);
        product.Stock.Should().Be(100);
        product.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyName_ShouldThrowDomainException(string? name)
    {
        var act = () => Product.Create(name!, "desc", 10m, 0);

        act.Should().Throw<DomainException>().WithMessage("*name*");
    }

    [Fact]
    public void Create_WithNegativePrice_ShouldThrowDomainException()
    {
        var act = () => Product.Create("Product", "desc", -1m, 0);

        act.Should().Throw<DomainException>().WithMessage("*price*");
    }

    [Fact]
    public void Create_ShouldRaiseProductCreatedEvent()
    {
        var product = Product.Create("Event Product", "desc", 1m, 0);

        product.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ProductCreatedEvent>();
    }

    [Fact]
    public void AdjustStock_WithSufficientStock_ShouldIncreaseStock()
    {
        var product = Product.Create("Product", "desc", 1m, 10);

        product.AdjustStock(5);

        product.Stock.Should().Be(15);
    }

    [Fact]
    public void AdjustStock_BelowZero_ShouldThrowDomainException()
    {
        var product = Product.Create("Product", "desc", 1m, 5);

        var act = () => product.AdjustStock(-10);

        act.Should().Throw<DomainException>().WithMessage("*Insufficient stock*");
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        var product = Product.Create("Product", "desc", 1m, 0);

        product.Deactivate();

        product.IsActive.Should().BeFalse();
        product.UpdatedAt.Should().NotBeNull();
    }
}
