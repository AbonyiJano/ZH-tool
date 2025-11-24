using Microsoft.EntityFrameworkCore;
using ZH_tool.Data;
using ZH_tool.Models;

namespace ZH_tool.Repository
{
    public class MegoldasRepository : IRepository<Megoldas>
    {
        private readonly ApplicationDbContext _context;
        public MegoldasRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Új megoldás mentése az adatbázisba.
        /// </summary>
        public async Task<Megoldas> AddAsync(Megoldas entity)
        {
            await _context.Megoldasok.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Megoldás törlése ID alapján.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var entityToDelete = await _context.Megoldasok.FindAsync(id);
            if (entityToDelete == null) return false;

            _context.Megoldasok.Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Összes megoldás lekérdezése a kapcsolódó adatokkal.
        /// </summary>
        public async Task<IEnumerable<Megoldas>> GetAllAsync()
        {
            return await _context.Megoldasok.ToListAsync();
        }
        /// <summary>
        /// Megoldás lekérdezése ID alapján a kapcsolódó adatokkal.
        /// </summary>
        public async Task<Megoldas?> GetByIdAsync(int id)
        {
            return await _context.Megoldasok
                .Include(m => m.Ertekeles)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        /// <summary>
        /// Megoldás adatainak frissítése.
        /// </summary>
        public async Task<Megoldas?> UpdateAsync(Megoldas entity)
        {
            var existingEntity = await _context.Megoldasok.FindAsync(entity.Id);
            if (existingEntity == null) return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return existingEntity;
        }
    }
}
