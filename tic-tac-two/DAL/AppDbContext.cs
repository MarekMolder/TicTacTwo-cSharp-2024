using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    /// <summary>
    /// Represents the 'GameConfigurations' table in the database.
    /// </summary>
    public DbSet<GameConfiguration> GameConfigurations { get; set; }
    
    /// <summary>
    /// Represents the 'SaveGames' table in the database.
    /// </summary>
    public DbSet<SaveGame> SaveGames { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class with the specified options.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}