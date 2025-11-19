using ZH_tool.Models;

namespace ZH_tool.Services
{
    public interface IFeladatService
    {
        Task<Feladat> CreateFeladatAsync(Feladat feladat);
        Task<Feladat?> GetFeladatByIdAsync(int id);
        Task<IEnumerable<Feladat>> GetAllFeladatAsync();
        Task<Feladat?> UpdateFeladatAsync(Feladat feladat);
        Task<bool> DeleteFeladatAsync(int id);
        Task<IEnumerable<Feladat>> GetFeladatokByGeneraltZhIdAsync(int generaltZhId);
    }
}
