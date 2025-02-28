using Auctions.Models;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Data.Services
{
    public class ListingService : IListingService
    {
        private readonly ApplicationDbContext _context;

        public ListingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Listing listing)
        {
           await _context.AddAsync(listing);
        }

        public  IQueryable<Listing> GetAll()
        {
            var applicationDbContext =  _context.Listings.Include(l => l.User);
            return applicationDbContext;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Listing> GetByIdAsync(int? id)
        {
            var listing = await _context.Listings
              .Include(l => l.User)
              .Include(l=>l.Bids)
                .ThenInclude(b => b.User)
              .Include(l=>l.Comments)
               .ThenInclude(c => c.User)
              .FirstOrDefaultAsync(m => m.Id == id);
            return listing;
            
        }
    }
}
