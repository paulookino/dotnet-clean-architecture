using Domain.Events;
using Domain.Exceptions;

namespace Domain.Entities;

public sealed class Product : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }

    private Product() { }

    public static Product Create(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");
        if (price < 0)
            throw new DomainException("Product price cannot be negative.");
        if (stock < 0)
            throw new DomainException("Stock cannot be negative.");

        var product = new Product
        {
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            Price = price,
            Stock = stock,
            IsActive = true
        };

        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.Name));
        return product;
    }

    public void UpdateDetails(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");
        if (price < 0)
            throw new DomainException("Product price cannot be negative.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AdjustStock(int quantity)
    {
        if (Stock + quantity < 0)
            throw new DomainException($"Insufficient stock. Current: {Stock}, requested: {-quantity}.");

        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}

public sealed record ProductCreatedEvent(Guid ProductId, string ProductName) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
