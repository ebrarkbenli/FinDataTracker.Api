using FinDataTracker.Api.Data;
using FinDataTracker.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using FinDataTracker.Api.Repositories;
using FinDataTracker.Api.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddControllers();
builder.Services.AddHttpClient<StockPriceService>();
builder.Services.AddScoped<StockService>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

