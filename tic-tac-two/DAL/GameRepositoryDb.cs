using System.Text.Json;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context;
    
    public GameRepositoryDb(AppDbContext context)
    {
        _context = context;
    }
    
    public string Savegame(string jsonStateString, GameConfiguration gameConfig, string? playerX = null, string? playerO = null)
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
            ConfigurationId = gameConfig.Id,
            GameMakerUsername = playerX,
            GameJoinerUsername = playerO
        };

        _context.SaveGames.Add(saveGame);
        _context.SaveChanges();

        var configName = gameConfig.Name.Split('_')[0];
        
        return configName + "_" + saveGame.CreatedAtDateTime;
    }

    /// <summary>
    /// Retrieves a list of all saved game names in the database.
    /// </summary>
    /// <returns>A list of saved game names in the format "ConfigurationName_Timestamp".</returns>
    public List<string> GetSavedGameNames()
    {
        return _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .AsEnumerable()
            .Select(sg => $"{sg.GameConfiguration!.Name.Split("_")[0]}_{sg.CreatedAtDateTime}")
            .ToList();
    }
    
    public List<string> GetUsernameSavedGameNames(string username)
    {
        return _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .Where(sg => sg.GameMakerUsername == username || sg.GameJoinerUsername == username)
            .AsEnumerable()
            .Select(sg => $"{sg.GameConfiguration!.Name.Split("_")[0]}_{sg.CreatedAtDateTime}")
            .ToList();
    }
    
    public List<string> GetFreeJoinGames(string username)
    {
        return _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .Where(sg => sg.GameMakerUsername != username && sg.GameJoinerUsername == null || sg.GameJoinerUsername != username && sg.GameMakerUsername == null)
            .AsEnumerable()
            .Select(sg => $"{sg.GameConfiguration!.Name.Split("_")[0]}_{sg.CreatedAtDateTime}")
            .ToList();
    }
    
    public string? FindSavedGame(string gameName)
    {
        string? finalFormattedDateTime = ParseGameNameToDateTime(gameName);
        if (finalFormattedDateTime == null)
        {
            return null;
        }

        try
        {
            // Fetch the saved game from the database
            var savedGame = _context.SaveGames
                .Include(sg => sg.GameConfiguration)
                .FirstOrDefault(sg => sg.CreatedAtDateTime == finalFormattedDateTime);

            if (savedGame == null)
            {
                Console.WriteLine($"No saved game found with the created date/time '{finalFormattedDateTime}'.");
                return null;
            }

            return savedGame.State;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while searching for the saved game: {ex.Message}");
            return null;
        }
    }
    
    

    public string UpdateGame(string jsonStateString, string gameName, GameConfiguration gameConfiguration, string? username)
    {
        string? finalFormattedDateTime = ParseGameNameToDateTime(gameName);
        if (finalFormattedDateTime == null)
        {
            return null;
        }
        
        var existingGame = _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .FirstOrDefault(sg => sg.CreatedAtDateTime == finalFormattedDateTime);
        
        if (existingGame != null)
        {
            // Option 1: Delete the old game state (if you want to delete old entries)
            _context.SaveGames.Remove(existingGame);
            _context.SaveChanges(); // Ensure changes are saved

            Console.WriteLine($"Old game entry with name '{gameName}' has been deleted.");
        }

        var saveGame = new SaveGame
        {
            CreatedAtDateTime = finalFormattedDateTime,
            State = jsonStateString,
            ConfigurationId = gameConfiguration.Id,
            GameMakerUsername = existingGame.GameMakerUsername
        };

        if (existingGame.GameJoinerUsername != null)
        {
            saveGame.GameJoinerUsername = existingGame.GameJoinerUsername;
        }
        
        if (existingGame.GameMakerUsername != null)
        {
            saveGame.GameMakerUsername = existingGame.GameMakerUsername;
        }

        // If the existing game does not have a joiner and the maker is not the current user
        if (existingGame.GameJoinerUsername == null && existingGame.GameMakerUsername != username)
        {
            saveGame.GameJoinerUsername = username;
        } else if (existingGame.GameMakerUsername == null && existingGame.GameJoinerUsername != username)
        {
            saveGame.GameMakerUsername = username;
        }

        // Save the new game state as a new entry
        _context.SaveGames.Add(saveGame);
        _context.SaveChanges();
        Console.WriteLine($"New game state has been saved under the name '{gameName}'.");
        
        var configName = gameConfiguration.Name.Split('_')[0];

        return configName + "_" + saveGame.CreatedAtDateTime;
    }
    
    private string? ParseGameNameToDateTime(string gameName)
    {
        if (string.IsNullOrWhiteSpace(gameName))
        {
            Console.WriteLine("The game name cannot be null or empty.");
            return null;
        }

        Console.WriteLine($"Parsing game name: {gameName}");

        // Extract the DateTime part from the gameName
        var parts = gameName.Split('_');
        if (parts.Length < 2)
        {
            Console.WriteLine($"Invalid game name format '{gameName}'. Expected format 'name_yyyy-MM-dd HH-mm-ss'.");
            return null;
        }

        var dateTimePart = parts[1]; // This will be the "2024-12-10 11-33-12"
        Console.WriteLine($"Extracted DateTime part: {dateTimePart}");

        // Ensure we replace the hyphens and dashes with appropriate format for DateTime
        // Replacing the hyphens in the date and the dash between hour, minute, and second.
        string formattedDateTime = dateTimePart.Replace('-', ':'); // Changes "2024-12-10 11-33-12" to "2024:12:10 11:33:12"

        // Now convert the formatted string to DateTime
        if (!DateTime.TryParseExact(formattedDateTime, "yyyy:MM:dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime createdAtDateTime))
        {
            Console.WriteLine($"Invalid DateTime format in game name '{gameName}'. Expected format 'yyyy-MM-dd HH:mm:ss'.");
            return null;
        }

        // Reformat it back to "yyyy-MM-dd HH:mm:ss" to make sure it is formatted correctly
        return createdAtDateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }


}
