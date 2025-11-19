
using Microsoft.EntityFrameworkCore;
using ZH_tool.Models;
using ZH_tool.Repository;

namespace ZH_tool.Services
{
    public class FeladatService : IFeladatService
    {
        private readonly IFeladatRepository _feladatRepository;

        // Injektáljuk a FeladatRepository-t
        public FeladatService(IFeladatRepository feladatRepository)
        {
            _feladatRepository = feladatRepository;
        }

        public async Task<Feladat> CreateFeladatAsync(Feladat feladat)
        {
            // Itt helyezhetnénk el további üzleti logikát, pl. validációt
            return await _feladatRepository.AddAsync(feladat);
        }

        public async Task<Feladat?> GetFeladatByIdAsync(int id)
        {
            return await _feladatRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Feladat>> GetAllFeladatAsync()
        {
            return await _feladatRepository.GetAllAsync();
        }

        public async Task<Feladat?> UpdateFeladatAsync(Feladat feladat)
        {
            return await _feladatRepository.UpdateAsync(feladat);
        }

        public async Task<bool> DeleteFeladatAsync(int id)
        {
            return await _feladatRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Feladat>> GetFeladatokByGeneraltZhIdAsync(int generaltZhId)
        {
            return await _feladatRepository.GetByGeneraltZhIdAsync(generaltZhId);
        }
    }
}
