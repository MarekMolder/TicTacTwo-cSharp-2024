using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GameRepositoryDb(AppDbContext context) : IGameRepository
{
    /// <summary>
    /// Saves a game state and its configuration to the database.
    /// </summary>
    public string Savegame(string jsonStateString, GameConfiguration gameConfig, string? playerX = null, string? playerO = null)
    {
        var existingConfig = context.GameConfigurations
            .FirstOrDefault(gc => gc.Name == gameConfig.Name);
        
        if (existingConfig == null)
        {
            context.GameConfigurations.Add(gameConfig);
            context.SaveChanges();
        }
        
        var saveGame = new SaveGame
        {
            CreatedAtDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            State = jsonStateString,
            ConfigurationId =existingConfig!.Id,
            Player1 = playerX,
            Player2 = playerO
        };

        context.SaveGames.Add(saveGame);
        context.SaveChanges();
        
        return $"{existingConfig.Name.Split('_')[0]}_{saveGame.CreatedAtDateTime}";
    }

    /// <summary>
    /// Retrieves a list of all saved game names in the database.
    /// </summary>
    public List<string> GetSavedGameNames()
    {
        return context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .AsEnumerable()
            .Select(sg => $"{sg.GameConfiguration!.Name.Split("_")[0]}_{sg.CreatedAtDateTime}")
            .ToList();
    }
    
    /// <summary>
    /// Retrieves a list of saved games associated with a specific username.
    /// </summary>
    public List<string> GetUsernameSavedGameNames(string username)
    {
        return context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .Where(sg => sg.Player1 == username || sg.Player2 == username)
            .AsEnumerable()
            .Select(sg => $"{sg.GameConfiguration!.Name.Split("_")[0]}_{sg.CreatedAtDateTime}")
            .ToList();
    }
    
    /// <summary>
    /// Retrieves a list of games that are available to join for a given username.
    /// A game is considered joinable if it has an empty player slot.
    /// </summary>
    public List<string> GetFreeJoinGames(string username)
    {
        return context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .Where(sg => sg.Player1 != username && sg.Player2 == null || sg.Player2 != username && sg.Player1 == null)
            .AsEnumerable()
            .Select(sg => $"{sg.GameConfiguration!.Name.Split("_")[0]}_{sg.CreatedAtDateTime}")
            .ToList();
    }
    
    /// <summary>
    /// Finds and retrieves the state of a saved game based on its unique name.
    /// </summary>
    public string? FindSavedGame(string gameName)
    {
        string? formattedDateTime = ParseGameNameToDateTime(gameName);
        if (formattedDateTime == null) return null;

        var savedGame = context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .FirstOrDefault(sg => sg.CreatedAtDateTime == formattedDateTime);

        return savedGame?.State;
    }
    
    /// <summary>
    /// Updates an existing saved game state in the database with a new state.
    /// If the game already exists, it is removed and replaced with the updated state.
    /// </summary>
    public string UpdateGame(string jsonStateString, string gameName, GameConfiguration gameConfiguration, string? username)
    {
        var formattedDateTime = ParseGameNameToDateTime(gameName);
        if (formattedDateTime == null) return null!;
        
        var existingGame = context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .FirstOrDefault(sg => sg.CreatedAtDateTime == formattedDateTime);
        
        if (existingGame == null) return null!;
        
        existingGame.State = jsonStateString;

        if (existingGame.Player2 == null && existingGame.Player1 != username)
            existingGame.Player2 = username;
        else if (existingGame.Player1 == null && existingGame.Player2 != username)
            existingGame.Player1 = username;
        
        context.SaveChanges();
        
        return $"{gameConfiguration.Name.Split('_')[0]}_{existingGame.CreatedAtDateTime}";
    }
    
    /// <summary>
    /// Parses a saved game name into a formatted date-time string.
    /// </summary>
    private string? ParseGameNameToDateTime(string gameName)
    {
        var parts = gameName.Split('_');
        if (parts.Length < 2) return null;

        var formattedDateTime = parts[1].Replace('-', ':');

        return DateTime.TryParseExact(formattedDateTime, "yyyy:MM:dd HH:mm:ss", null, 
            System.Globalization.DateTimeStyles.None, out var dateTime)
            ? dateTime.ToString("yyyy-MM-dd HH:mm:ss")
            : null;
    }


}
