
using Auctions.Models;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Data.Services
{
    public class RepositoryService<T> : IRepositoryService<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public RepositoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T item)
        {
            await _context.AddAsync(item);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            if (typeof(T) == typeof(Bid))
            {
                return (IQueryable<T>) _context.Bids.Include(b => b.Listing).ThenInclude(b => b.User);
            }
            return _context.Set<T>();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Bid))
            {
                return (IReadOnlyList<T>)await _context.Bids.Include(b => b.Listing).ThenInclude(b => b.User).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int? id)
        {
            if (typeof(T) == typeof(Listing))
            {
                 return  await _context.Set<Listing>().Include(l => l.User).FirstOrDefaultAsync() as T;
            }
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
