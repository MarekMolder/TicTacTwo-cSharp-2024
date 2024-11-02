using System.Text.Json;
using ConsoleApp;
using DAL;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static readonly string ConnectionString = $"Data Source={FileHelper.BasePath}app.db";

    public static void Main(string[] args)
    {
        // Muutuja salvestusmeetodi valimiseks
        bool useDatabase = true; // Määra see `false`, kui soovid kasutada JSON-i

        // Defineeri IGameRepository ja IConfigRepository
        IGameRepository gameRepository;
        IConfigRepository configRepository;

        AppDbContext context = null;

        if (useDatabase)
        {
            // Kui kasutad andmebaasi
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
            // Kui kasutad JSON-i
            gameRepository = new GameRepositoryJson();
            configRepository = new ConfigRepositoryJson();
        }

        // Loo Menus koos õigete sõltuvustega
        var menus = new Menus(configRepository, gameRepository);

        // Käivita menüü
        menus.RunMainMenu();

        // Rakenduse lõppedes vabasta konteksti ressursid, kui kasutati andmebaasi
        context?.Dispose();
    }
}