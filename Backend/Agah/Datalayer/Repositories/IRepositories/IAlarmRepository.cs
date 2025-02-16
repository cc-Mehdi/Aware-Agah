using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IAlarmRepository : IRepository<Alarm>
    {
        Task UpdateAsync(Alarm item);
    }
}
