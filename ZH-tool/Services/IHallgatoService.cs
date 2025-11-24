using ZH_tool.Models;

namespace ZH_tool.Services
{
    public interface IHallgatoService
    {
        Task<Hallgato?> GetHallgatoByNeptunAsync(string neptunkod);
        Task<Hallgato> CreateHallgatoAsync(Hallgato hallgato);
        Task<IEnumerable<Hallgato>> GetAllHallgatoAsync();
        // Frissítés/Törlés opcionális, de a megoldásokat kezeljük.

    }
}
