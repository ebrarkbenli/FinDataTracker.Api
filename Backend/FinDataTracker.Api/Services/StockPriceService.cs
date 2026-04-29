using System.Text.Json;

namespace FinDataTracker.Api.Services;

public class StockPriceService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public StockPriceService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<decimal?> GetStockPriceAsync(string symbol)
    {
        var apiKey = _configuration["Finnhub:ApiKey"];

        var url = $"https://finnhub.io/api/v1/quote?symbol={symbol}&token={apiKey}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
           return null;
        
        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);

        var root = doc.RootElement;

        // "c" is the current price field in the Finnhub quote response
        if (root.TryGetProperty("c", out var priceElement))
        {
            var price = priceElement.GetDecimal();

            if (price <= 0)
                return null;

            return price;
        }

        return null;
    }
}