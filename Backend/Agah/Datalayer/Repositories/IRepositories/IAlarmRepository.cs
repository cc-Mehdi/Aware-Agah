using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IAlarmRepository : IRepository<Alarm>
    {
        void Update(Alarm item);
    }
}
