using Microsoft.EntityFrameworkCore;
using Products.Repository.Interfaces;
using Products.Repository.Data;
using Products.Common.Entities;
using Products.Repository.Exceptions;
using Products.Utility.Exceptions;

namespace Products.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await ExceptionExecutor.ExecuteWithExceptionHandling(
                async () => await _context.Products.ToListAsync(),
                RepositoryErrorMessages.GetAllFailed
            );

        public async Task<Product> GetByIdAsync(int id) =>
            await ExceptionExecutor.ExecuteWithExceptionHandling(
                async () =>
                {
                    var product = await _context.Products.FindAsync(id);
                    if (product == null)
                        throw new RepositoryException(RepositoryErrorMessages.ProductNotFound, 404);
                    return product;
                },
                RepositoryErrorMessages.GetByIdFailed
            );

        public async Task<int> AddAsync(Product product) =>
            await ExceptionExecutor.ExecuteWithExceptionHandling(
                async () =>
                {
                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                    return product.Id;
                },
                RepositoryErrorMessages.AddFailed
            );

        public async Task<int> UpdateAsync(Product product) =>
            await ExceptionExecutor.ExecuteWithExceptionHandling(
                async () =>
                {
                    // Fetch the latest entity from the database
                    var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id) ?? throw new RepositoryException(RepositoryErrorMessages.UpdateNotFound, 404);

                    // Update properties (except RowVersion)
                    existing.Name = product.Name;
                    existing.Description = product.Description;
                    existing.Stock = product.Stock;

                    // Save changes (EF will handle RowVersion automatically)
                    await _context.SaveChangesAsync();
                    return existing.Id;
                },
                RepositoryErrorMessages.UpdateFailed
            );

        public async Task<int> DeleteAsync(int id) =>
            await ExceptionExecutor.ExecuteWithExceptionHandling(
                async () =>
                {
                    var product = await _context.Products.FindAsync(id);
                    if (product != null)
                    {
                        _context.Products.Remove(product);
                        await _context.SaveChangesAsync();
                        return product.Id;
                    }
                    throw new RepositoryException(RepositoryErrorMessages.DeleteNotFound, 404);
                },
                RepositoryErrorMessages.DeleteFailed
            );

        public async Task<bool> ExistsAsync(int id) =>
            await ExceptionExecutor.ExecuteWithExceptionHandling(
                async () => await _context.Products.AnyAsync(p => p.Id == id),
                RepositoryErrorMessages.ExistsFailed
            );
    }
}