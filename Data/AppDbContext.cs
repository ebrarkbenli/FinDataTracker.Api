using FinDataTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinDataTracker.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Stock> Stocks => Set<Stock>();
}