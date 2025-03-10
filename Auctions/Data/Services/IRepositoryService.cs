namespace Auctions.Data.Services
{
    public interface IRepositoryService<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        IQueryable<T> GetAll();
        Task AddAsync(T item);
        Task<T> GetByIdAsync(int? id);
        Task<int> CompleteAsync();

    }
}
