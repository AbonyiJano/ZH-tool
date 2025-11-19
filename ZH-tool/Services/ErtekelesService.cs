using ZH_tool.Models;
using ZH_tool.Repository;

namespace ZH_tool.Services
{
    public class ErtekelesService : IErtekelesService
    {
        private readonly IRepository<Ertekeles> _ertekelesRepository;

        public ErtekelesService(
            IRepository<Ertekeles> ertekelesRepository)
        {
            _ertekelesRepository = ertekelesRepository;
        }
        public async Task<Ertekeles?> GetErtekelesByIdAsync(int id)
        {
            return await _ertekelesRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Ertekeles>> GetAllErtekelesAsync()
        {
            return await _ertekelesRepository.GetAllAsync();
        }
    }
}
