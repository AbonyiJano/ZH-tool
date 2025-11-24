using ZH_tool.Models;

namespace ZH_tool.Services
{
    public interface IErtekelesService
    {
       //Task<Megoldas?> GetMegoldasByIdAsync(int id);

        // ÚJ: Értékelés lekérdezése
        Task<Ertekeles?> GetErtekelesByIdAsync(int id);
        Task<IEnumerable<Ertekeles>> GetAllErtekelesAsync();
        Task<Ertekeles?> UpdateErtekelesAsync(Ertekeles ertekeles);
    }
}
