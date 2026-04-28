using FinDataTracker.Api.Models;

namespace FinDataTracker.Api.Repositories;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock?> GetBySymbolAsync(string symbol);
    Task AddAsync(Stock stock);
    Task DeleteAsync(Stock stock);
    Task SaveChangesAsync();
}