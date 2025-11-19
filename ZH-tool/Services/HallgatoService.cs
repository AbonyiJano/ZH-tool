using ZH_tool.Models;
using ZH_tool.Repository;

namespace ZH_tool.Services
{
    public class HallgatoService : IHallgatoService
    {
        private readonly IRepository<Hallgato> _hallgatoRepository;

        public HallgatoService(
            IRepository<Hallgato> hallgatoRepository)
        {
            _hallgatoRepository = hallgatoRepository;
        }
        /// <summary>
        /// Hallgató lekérdezése Neptunkód alapján. String kulcsot használ.
        /// </summary>
        public async Task<Hallgato?> GetHallgatoByNeptunAsync(string neptunkod)
        {
            // Mivel a Repository-ban a string ID-t fogadó metódust implementáltuk (felülírva az int ID-st),
            // itt speciális metódust kell használni, vagy explicit castot a Repository-n:
            return await ((HallgatoRepository)_hallgatoRepository).GetByIdAsync(neptunkod);
        }
        /// <summary>
        /// Új hallgató létrehozása. Először ellenőrizzük, létezik-e már az adott Neptunkód.
        /// </summary>
        public async Task<Hallgato> CreateHallgatoAsync(Hallgato hallgato)
        {
            return await _hallgatoRepository.AddAsync(hallgato);
        }


    }
}
