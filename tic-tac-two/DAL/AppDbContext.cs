using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<GameConfiguration> GameConfigurations { get; set; }
    public DbSet<SaveGame> SaveGames { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}