using System.Text.Json;
using ConsoleApp;
using DAL;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// The main entry point for the application, responsible for setting up repositories and launching the main menu.
/// </summary>
public class Program
{
    /// <summary>
    /// The connection string used to connect to the SQLite database.
    /// </summary>
    private static readonly string ConnectionString = $"Data Source={FileHelper.BasePath}app.db";

    /// <summary>
    /// The entry point for the application. Sets up the repositories and launches the main menu.
    /// </summary>
    /// <param name="args">Command line arguments (not used in this application).</param>
    public static void Main(string[] args)
    {
        // Flag to select whether to use a database or JSON storage
        bool useDatabase = false; // Set to `false` to use JSON instead of the database

        // Declare the repositories for game data and configuration data
        IGameRepository gameRepository;
        IConfigRepository configRepository;

        AppDbContext context = null;
        
        // Choose whether to use the database or JSON for storage
        if (useDatabase)
        {
            // If using the database, set up the DbContext and repositories
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(ConnectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            context = new AppDbContext(contextOptions);
            
            // Apply any pending migrations to the database
            context.Database.Migrate();
            
            // Instantiate the repositories for database storage
            gameRepository = new GameRepositoryDb(context);
            configRepository = new ConfigRepositoryDb(context);
        }
        else
        {
            // If using JSON storage, instantiate the JSON-based repositories
            gameRepository = new GameRepositoryJson();
            configRepository = new ConfigRepositoryJson();
        }

        // Create the Menus instance with the appropriate repositories
        var menus = new Menus(configRepository, gameRepository);

        // Run the main menu to interact with the application
        menus.RunMainMenu();

        // Clean up the context resources when the application is done, if a database was used 
        context?.Dispose();
    }
}