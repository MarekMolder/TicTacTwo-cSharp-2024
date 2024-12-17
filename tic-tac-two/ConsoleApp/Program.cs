using DAL;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp;

public static class Program
{
    /// <summary>
    /// The connection string used to connect to the SQLite database.
    /// </summary>
    private static readonly string ConnectionString = $"Data Source={FileHelper.BasePath}app.db";

    /// <summary>
    /// The entry point for the application. Sets up the repositories and launches the main menu.
    /// </summary>
    public static void Main()
    {
        // Flag to select whether to use a database or JSON storage
        bool useDatabase = false; // Set to `false` to use JSON instead of the database

     
        IGameRepository gameRepository;
        IConfigRepository configRepository;

        AppDbContext context = null!;
        
        if (useDatabase)
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(ConnectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            context = new AppDbContext(contextOptions);
            
            context.Database.Migrate();

            gameRepository = new GameRepositoryDb(context);
            configRepository = new ConfigRepositoryDb(context);
        }
        else
        {
            gameRepository = new GameRepositoryJson();
            configRepository = new ConfigRepositoryJson();
        }
        
        var menus = new Menus(configRepository, gameRepository);
        
        menus.RunMainMenu();
        
        context.Dispose();
    }
}