using Microsoft.EntityFrameworkCore;
using ZH_tool.Data;
using ZH_tool.Models;

namespace ZH_tool.Repository
{
    public class HallgatoRepository : IRepository<Hallgato>
    {
        private readonly ApplicationDbContext _context;
        public HallgatoRepository(ApplicationDbContext context)
        {

            _context = context;
        }
        /// <summary>
        /// Új hallgató mentése az adatbázisba.
        /// </summary>
        public async Task<Hallgato> AddAsync(Hallgato entity)
        {
            await _context.Hallgatok.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Hallgató törlése Neptunkód alapján (itt a 'Neptunkod' az ID).
        /// </summary>
        async Task<bool> IRepository<Hallgato>.DeleteAsync(int id) 
        => throw new NotSupportedException("A Hallgató entitás törléséhez a string Neptunkód ID-t kell használni.");
        public async Task<bool> DeleteAsync(string neptunkod)
        {
            var hallgatoToDelete = await _context.Hallgatok.FindAsync(neptunkod);
            if (hallgatoToDelete == null) return false;

            _context.Hallgatok.Remove(hallgatoToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Összes hallgató lekérdezése.
        /// </summary>
        public async Task<IEnumerable<Hallgato>> GetAllAsync()
        {
            return await _context.Hallgatok.Include(h => h.Megoldasok).ToListAsync();
        }
        /// <summary>
        /// Hallgató lekérdezése Neptunkód alapján.
        /// </summary>
        /// 
        async Task<Hallgato?> IRepository<Hallgato>.GetByIdAsync(int id)
            => throw new NotSupportedException("A Hallgató entitás lekérdezéséhez a string Neptunkód ID-t kell használni.");
        public async Task<Hallgato?> GetByIdAsync(string neptunkod)
        {
            return await _context.Hallgatok
                                 .Include(h => h.Megoldasok)
                                 .FirstOrDefaultAsync(h => h.Neptunkod == neptunkod);
        }
        /// <summary>
        /// Hallgató adatainak frissítése.
        /// </summary>
        public async Task<Hallgato?> UpdateAsync(Hallgato entity)
        {
            // FindAsync a Neptunkod alapján
            var existingEntity = await _context.Hallgatok.FindAsync(entity.Neptunkod);
            if (existingEntity == null) return null;

            // Frissítés: csak azokat a mezőket állítja be, amelyek megváltoztak
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            
            await _context.SaveChangesAsync();
            return existingEntity;
        }
}

}
