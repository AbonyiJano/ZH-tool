using ZH_tool.Models;

namespace ZH_tool.Repository
{
    public interface IFeladatRepository : IRepository<Feladat>
    {
        Task<IEnumerable<Feladat>> GetByGeneraltZhIdAsync(int generaltZhId);
    }
}
