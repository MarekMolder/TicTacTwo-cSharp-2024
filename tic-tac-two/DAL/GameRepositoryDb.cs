using System.Text.Json;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

/// <summary>
/// A repository implementation for saving, loading, and managing game data using a database.
/// </summary>
public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameRepositoryDb"/> class.
    /// </summary>
    /// <param name="context">The database context used to interact with the database.</param>
    public GameRepositoryDb(AppDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Saves the game state to the database.
    /// If the configuration doesn't exist, it will be added.
    /// </summary>
    /// <param name="jsonStateString">The JSON string representing the game state.</param>
    /// <param name="gameConfig">The game configuration associated with the saved game.</param>
    public int Savegame(string jsonStateString, GameConfiguration gameConfig)
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
        
        return saveGame.Id; 
    }

    /// <summary>
    /// Loads a saved game from the database based on the game ID.
    /// </summary>
    /// <param name="gameId">The ID of the saved game to load.</param>
    /// <returns>A <see cref="SaveGame"/> object representing the saved game state.</returns>
    public SaveGame LoadGame(int gameId)
    {
        var savedGame = _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .FirstOrDefault(sg => sg.Id == gameId);

        if (savedGame == null)
        {
            throw new Exception($"No saved game found with the ID '{gameId}'.");
        }

        return savedGame;
    }

    /// <summary>
    /// Retrieves a list of all saved game names in the database.
    /// </summary>
    /// <returns>A list of saved game names in the format "ConfigurationName_Timestamp".</returns>
    public List<string> GetSavedGameNames()
    {
        return _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .Select(sg => $"{sg.GameConfiguration!.Name}_{sg.CreatedAtDateTime.Replace(":", "-")}")
            .ToList();
    }

    /// <summary>
    /// Finds a saved game by its name in the database.
    /// </summary>
    /// <param name="gameName">The name of the saved game to find (including configuration name and timestamp).</param>
    /// <returns>The JSON string representing the game state, or null if the game is not found.</returns>
    public string? FindSavedGame(string gameName)
    {
        // Remove the timestamp part to search only by configuration name
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
