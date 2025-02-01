using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IReserveRepository : IRepository<Reserve>
    {
        void Update(Reserve item);
    }
}
