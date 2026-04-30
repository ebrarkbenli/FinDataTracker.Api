using FinDataTracker.Api.Data;
using FinDataTracker.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using FinDataTracker.Api.Repositories;
using FinDataTracker.Api.Services;
var builder = WebApplication.CreateBuilder(args);
var frontendCorsPolicy = "FrontendCors";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddControllers();
builder.Services.AddHttpClient<StockPriceService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontendCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(frontendCorsPolicy);

app.MapControllers();

app.Run();

