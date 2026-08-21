using Microsoft.EntityFrameworkCore;
using MonoSlice.Modules.Catalog.Persistence;
using MonoSlice.Shared.Abstractions.Contracts;

namespace MonoSlice.Modules.Catalog.Contracts;

public sealed class CatalogModuleApi : ICatalogModuleApi
{
    private readonly CatalogDbContext _dbContext;

    public CatalogModuleApi(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductContractDto?> GetProductByIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

        if (product is null)
        {
            return null;
        }

        return new ProductContractDto(
            product.Id,
            product.Name,
            product.Sku,
            product.Price,
            product.StockQuantity,
            product.IsActive);
    }

    public async Task<bool> HasSufficientStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

        return product is not null && product.IsActive && product.StockQuantity >= quantity;
    }
}
