using FinDataTracker.Api.Data;
using FinDataTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinDataTracker.Api.Repositories;
public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stocks.ToListAsync();
    }
    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.FindAsync(id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(x => x.Symbol == symbol.ToUpper());
    }

    public async Task AddAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
    }

    public Task DeleteAsync(Stock stock)
    {
        _context.Stocks.Remove(stock);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Stock>> GetTopByPriceAsync(int count)
    {
        return await _context.Stocks
            .OrderByDescending(x => x.Price)
            .Take(count)
            .ToListAsync();
    }

    public async Task<decimal> GetAveragePriceAsync()
    {
        if (!await _context.Stocks.AnyAsync())
            return 0;

        return await _context.Stocks.AverageAsync(x => x.Price);
    }

}