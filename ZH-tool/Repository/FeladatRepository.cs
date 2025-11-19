using Microsoft.EntityFrameworkCore;
using ZH_tool.Data;
using ZH_tool.Models;

namespace ZH_tool.Repository
{
    public class FeladatRepository : IRepository<Feladat>, IFeladatRepository
    {
        private readonly ApplicationDbContext _context;

        public FeladatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Feladat> AddAsync(Feladat entity)
        {
            await _context.Feladatok.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entityToDelete = await _context.Feladatok.FindAsync(id);
            if (entityToDelete == null) return false;

            _context.Feladatok.Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Feladat>> GetAllAsync()
        {
            // Beemeljük a GeneraltZh-t, hogy lássuk, melyik ZH-hoz tartozik
            return await _context.Feladatok
                                 .Include(f => f.GeneraltZh)
                                 .ToListAsync();
        }

        public async Task<Feladat?> GetByIdAsync(int id)
        {
            return await _context.Feladatok
                                 .Include(f => f.GeneraltZh)
                                 .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Feladat?> UpdateAsync(Feladat entity)
        {
            var existingEntity = await _context.Feladatok.FindAsync(entity.Id);
            if (existingEntity == null) return null;

            // Frissíti a meglévő entitás értékeit
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return existingEntity;
        }
        public async Task<IEnumerable<Feladat>> GetByGeneraltZhIdAsync(int generaltZhId)
        {
            return await _context.Feladatok
                                 .Where(f => f.GeneraltZhId == generaltZhId)
                                 .ToListAsync();
        }
    }
}
