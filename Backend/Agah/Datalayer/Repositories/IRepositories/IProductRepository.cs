using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product item);
    }
}
