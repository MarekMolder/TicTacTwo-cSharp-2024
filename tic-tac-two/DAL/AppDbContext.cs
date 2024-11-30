using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

/// <summary>
/// Represents the application's database context used for accessing and managing data.
/// This class includes the DbSets for <see cref="GameConfiguration"/> and <see cref="SaveGame"/> entities.
/// </summary>
/// <remarks>
/// The <see cref="AppDbContext"/> class is used by Entity Framework Core to map the database tables
/// to C# entities. It allows you to query and save instances of <see cref="GameConfiguration"/> and 
/// <see cref="SaveGame"/> entities to the underlying SQLite database.
/// </remarks>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{GameConfiguration}"/> for game configurations.
    /// Represents the 'GameConfigurations' table in the database.
    /// </summary>
    public DbSet<GameConfiguration> GameConfigurations { get; set; }
    
    /// <summary>
    /// Gets or sets the <see cref="DbSet{SaveGame}"/> for saved game instances.
    /// Represents the 'SaveGames' table in the database.
    /// </summary>
    public DbSet<SaveGame> SaveGames { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to configure the context, including connection strings and other settings.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}