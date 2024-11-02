using System.Text.Json;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context;
    private IGameRepository _gameRepositoryImplementation;

    public GameRepositoryDb(AppDbContext context)
    {
        _context = context;
    }
    
    public void Savegame(string jsonStateString, GameConfiguration gameConfig)
    {
        // Check if the configuration exists in the database by name
        var existingConfig = _context.GameConfigurations
            .FirstOrDefault(gc => gc.Name == gameConfig.Name);

        // If the configuration doesn't exist, create and add it
        if (existingConfig == null)
        {
            _context.GameConfigurations.Add(gameConfig);
            _context.SaveChanges(); // Save the new configuration to generate its ID
        }
        else
        {
            // Use the existing configuration's ID if it already exists
            gameConfig = existingConfig;
        }

        // Create and save the SaveGame entry
        var saveGame = new SaveGame
        {
            CreatedAtDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            State = jsonStateString,
            ConfigurationId = gameConfig.Id
        };

        _context.SaveGames.Add(saveGame);
        _context.SaveChanges();
    }

    public GameState LoadGame(string gameConfigName)
    {
        var savedGame = _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .OrderByDescending(sg => sg.CreatedAtDateTime)
            .FirstOrDefault(sg => sg.GameConfiguration!.Name == gameConfigName);

        if (savedGame == null)
        {
            throw new Exception($"No saved game found with the name '{gameConfigName}'.");
        }

        return JsonSerializer.Deserialize<GameState>(savedGame.State) 
               ?? throw new Exception("Failed to deserialize game state.");
    }

    public List<string> GetSavedGameNames()
    {
        return _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .Select(sg => $"{sg.GameConfiguration!.Name}_{sg.CreatedAtDateTime.Replace(":", "-")}")
            .ToList();
    }

    public string? FindSavedGame(string gameName)
    {
        // Eemaldame ajatempliosa, et otsida ainult konfiguratsiooninime alusel
        var configName = gameName.Split('_')[0];

        var savedGame = _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .OrderByDescending(sg => sg.CreatedAtDateTime)
            .FirstOrDefault(sg => sg.GameConfiguration != null && sg.GameConfiguration.Name == configName);

        if (savedGame == null)
        {
            Console.WriteLine($"No saved game found with the name '{gameName}'.");
            return null;
        }
        return savedGame.State;
    }
}