using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IProductLogRepository : IRepository<ProductLog>
    {
        void Update(ProductLog item);
    }
}
