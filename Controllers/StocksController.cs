using FinDataTracker.Api.DTOs;
using FinDataTracker.Api.Repositories;
using FinDataTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialDataTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    private readonly StockService _stockService;

    public StocksController(IStockRepository stockRepository, StockService stockService)
    {
    _stockRepository = stockRepository;
    _stockService = stockService;
    }

    [HttpGet]
    public async Task<ActionResult<List<StockResponseDto>>> GetAll()
    {
        var stocks = await _stockRepository.GetAllAsync();

        var result = stocks.Select(stock => new StockResponseDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            Price = stock.Price,
            LastUpdated = stock.LastUpdated
        }).ToList();

        return Ok(result);
    }

    [HttpPost("{symbol}")]
    public async Task<IActionResult> AddStock(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            return BadRequest("Symbol is required");
        }
        symbol = symbol.ToUpper();

        var result = await _stockService.AddStockAsync(symbol);

        if (result == null)
        {
            return BadRequest("Could not fetch stock data.");
        }

        return Ok(new
        {
            result.Id,
            result.Symbol,
            result.Price,
            result.LastUpdated
        });
    }

    [HttpGet("{symbol}")]
    public async Task<ActionResult<StockResponseDto>> GetBySymbol(string symbol)
    {
        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        if (stock == null)
        {
            return NotFound("Stock not found.");
        }

        var result = new StockResponseDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            Price = stock.Price,
            LastUpdated = stock.LastUpdated
        };

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var stock = await _stockRepository.GetByIdAsync(id);

        if (stock == null)
        {
            return NotFound("Stock not found.");
        }

        await _stockRepository.DeleteAsync(stock);
        await _stockRepository.SaveChangesAsync();

        return NoContent();
    }
    [HttpGet("top")]
    public async Task<ActionResult<List<StockResponseDto>>> GetTopStocks([FromQuery] int count = 5)
    {
        if (count <= 0)
            return BadRequest("Stock data couldnot be fetched. Check the symbol.");

        var stocks = await _stockRepository.GetTopByPriceAsync(count);

        var result = stocks.Select(stock => new StockResponseDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            Price = stock.Price,
            LastUpdated = stock.LastUpdated
        }).ToList();

        return Ok(result);
    }
}