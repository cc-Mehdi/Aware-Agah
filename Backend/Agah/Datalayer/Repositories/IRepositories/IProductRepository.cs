using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateAsync(Product item);
    }
}
