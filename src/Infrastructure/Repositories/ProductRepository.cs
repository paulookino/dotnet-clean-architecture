using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class ProductRepository(AppDbContext context)
    : Repository<Product>(context), IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetActiveAsync(CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking()
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(p => p.Name == name, cancellationToken);
}
