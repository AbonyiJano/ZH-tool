using Microsoft.EntityFrameworkCore;
using ZH_tool.Data;
using ZH_tool.Models;

namespace ZH_tool.Repository
{
    public class ErtekelesRepository : IRepository<Ertekeles>
    {
        private readonly ApplicationDbContext _context;

        public ErtekelesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Ertekeles> AddAsync(Ertekeles entity)
        {
            await _context.Ertekelesek.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entityToDelete = await _context.Ertekelesek.FindAsync(id);
            if (entityToDelete == null) return false;

            _context.Ertekelesek.Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Ertekeles>> GetAllAsync()
        {
            return await _context.Ertekelesek
                                 .Include(e => e.Megoldas) 
                                 .ToListAsync();
        }

        public async Task<Ertekeles?> GetByIdAsync(int id)
        {
            return await _context.Ertekelesek
                                 .Include(e => e.Megoldas)
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Ertekeles?> UpdateAsync(Ertekeles entity)
        {
            var existingEntity = await _context.Ertekelesek.FindAsync(entity.Id);
            if (existingEntity == null) return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return existingEntity;
        }

    }
}
