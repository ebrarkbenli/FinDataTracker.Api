using FinDataTracker.Api.Models;
using FinDataTracker.Api.Repositories;

namespace FinDataTracker.Api.Services;

public class StockService
{
    private readonly IStockRepository _repository;
    private readonly StockPriceService _priceService;

    public StockService(IStockRepository repository, StockPriceService priceService)
    {
        _repository = repository;
        _priceService = priceService;
    }
     public async Task<Stock?> AddStockAsync(string symbol)
    {
        var existing = await _repository.GetBySymbolAsync(symbol);

        if (existing != null)
            return existing;

        var price = await _priceService.GetStockPriceAsync(symbol);

        if (price == null)
            return null;

        var stock = new Stock
        {
            Symbol = symbol.ToUpper(),
            Price = price.Value,
            LastUpdated = DateTime.UtcNow
        };
        await _repository.AddAsync(stock);
        await _repository.SaveChangesAsync();

        return stock;
    }
}