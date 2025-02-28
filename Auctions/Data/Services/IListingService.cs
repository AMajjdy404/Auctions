using Auctions.Models;

namespace Auctions.Data.Services
{
    public interface IListingService
    {
        IQueryable<Listing> GetAll();
        Task AddAsync(Listing listing);
        Task<int> CompleteAsync();

        Task<Listing> GetByIdAsync(int? id);

    }
}
