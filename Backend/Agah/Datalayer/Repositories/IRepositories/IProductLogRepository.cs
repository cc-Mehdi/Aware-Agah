using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IProductLogRepository : IRepository<ProductLog>
    {
        Task UpdateAsync(ProductLog item);
    }
}
