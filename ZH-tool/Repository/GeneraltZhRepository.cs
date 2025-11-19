using Microsoft.EntityFrameworkCore;
using ZH_tool.Data;
using ZH_tool.Models;
namespace ZH_tool.Repository
{
    public class GeneraltZhRepository : IRepository<GeneraltZh>
    {
        private readonly ApplicationDbContext _context;

        public GeneraltZhRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Lekérdezi az összes generált ZH-t az adatbázisból.
        /// </summary>
        public async Task<IEnumerable<GeneraltZh>> GetAllAsync()
        {
            return await _context.GeneraltZhk.ToListAsync();
        }
        /// <summary>
        /// Lekérdez egy generált ZH-t azonosító alapján.
        /// </summary>
        public async Task<GeneraltZh?> GetByIdAsync(int id)
        {
            return await _context.GeneraltZhk.FindAsync(id);
        }
        /// <summary>
        /// Elment egy új generált ZH entitást az adatbázisba.
        /// </summary>
        /// <param name="entity">A mentendő GeneraltZh entitás.</param>
        public async Task<GeneraltZh> AddAsync(GeneraltZh entity)
        {
            await _context.GeneraltZhk.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Frissít egy létező generált ZH entitást.
        /// </summary>
        /// <param name="entity">A frissített GeneraltZh entitás.</param>
        public async Task<GeneraltZh?> UpdateAsync(GeneraltZh entity)
        {
            var existingEntity = await _context.GeneraltZhk.FindAsync(entity.Id);
            if (existingEntity == null)
            {
                return null;
            }
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return existingEntity;
        }
        /// <summary>
        /// Töröl egy generált ZH-t azonosító alapján.
        /// </summary>
        /// <param name="id">A törlendő entitás azonosítója.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            var entityToDelete = await _context.GeneraltZhk.FindAsync(id);
            if (entityToDelete == null) return false;

            _context.GeneraltZhk.Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
