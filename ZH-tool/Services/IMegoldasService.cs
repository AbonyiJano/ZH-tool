using ZH_tool.Models;

namespace ZH_tool.Services
{
    public interface IMegoldasService
    {
        Task<Megoldas?> SubmitMegoldasAsync(Megoldas megoldas);
        Task<Megoldas?> GetMegoldasByIdAsync(int id);
        Task<IEnumerable<Megoldas>> GetAllMegoldasAsync();

        Task<Ertekeles?> GradeMegoldasAsync(Megoldas megoldas);
    }
}
