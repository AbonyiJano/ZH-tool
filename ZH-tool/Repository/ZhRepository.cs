using Microsoft.EntityFrameworkCore;
using ZH_tool.Data;
using ZH_tool.Models;
namespace ZH_tool.Repository
{
    public class ZhRepository : IRepository<Zh>
    {
        private readonly ApplicationDbContext _context;

        public ZhRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Zh>> GetAllAsync()
        {
            return await _context.Zhk.ToListAsync();
        }
        public async Task<Zh?> GetByIdAsync(int id)
        {
            return await _context.Zhk.FindAsync(id);
        }
        public async Task<Zh> AddAsync(Zh zh)
        {
            _context.Zhk.Add(zh);
            await _context.SaveChangesAsync();
            return zh;
        }
        public async Task<Zh?> UpdateAsync(Zh zh)
        {
            var existingZh = await _context.Zhk.FindAsync(zh.Id);
            if (existingZh == null)
            {
                return null;
            }

            // Frissítés (itt érdemes a mezőket egyesével másolni, ha nem akarjuk, hogy minden mező frissüljön)
            _context.Entry(existingZh).CurrentValues.SetValues(zh);

            await _context.SaveChangesAsync();
            return existingZh;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var zhToDelete = await _context.Zhk.FindAsync(id);
            if (zhToDelete == null)
            {
                return false;
            }

            _context.Zhk.Remove(zhToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
