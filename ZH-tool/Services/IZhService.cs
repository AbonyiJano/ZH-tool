using System.Threading.Tasks;
using ZH_tool.Models;

namespace ZH_tool.Services
{
    public interface IZhService
    {
        Task<IEnumerable<Zh>> ListAllZhkAsync();
        Task<Zh?> GetZhByIdAsync(int id);
        Task<Zh> CreateZhAsync(Zh zh);
        Task<Zh?> UpdateZhAsync(Zh zh);
        Task<bool> DeleteZhAsync(int id);

        Task<GeneraltZh?> GenerateZhContentAsync(int parentZhId);
        Task<GeneraltZh?> GetGeneratedZhByIdAsync(int id);
        Task<GeneraltZh?> UpdateGeneratedZhAsync(GeneraltZh zh);
        Task<bool> DeleteGeneratedZhAsync(int id);

    }
}
