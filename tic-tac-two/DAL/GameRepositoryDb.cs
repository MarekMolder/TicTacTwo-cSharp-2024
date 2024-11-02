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
    
    public void Savegame(string jsonStateString, string gameConfigName)
    {
        var gameConfig = _context.GameConfigurations
            .FirstOrDefault(gc => gc.Name == gameConfigName);

        if (gameConfig == null)
        {
            throw new Exception("Game configuration not found");
        }

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