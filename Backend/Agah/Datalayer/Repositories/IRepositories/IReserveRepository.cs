using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IReserveRepository : IRepository<Reserve>
    {
        Task UpdateAsync(Reserve item);
    }
}
