using Domain;
using System.Text.Json.Nodes;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    /// <summary>
    /// Saves a game state and its configuration to the database.
    /// </summary>
    public string Savegame(string jsonStateString, GameConfiguration gameConfig, string? playerX = null, string? playerO = null)
    {
        var sanitizedName = string.Join("_", gameConfig.Name.Split(Path.GetInvalidFileNameChars()));
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var fileName = Path.Combine(FileHelper.BasePath, $"{sanitizedName}_{timestamp}{FileHelper.GameExtension}");

        try
        {
            var jsonObject = JsonNode.Parse(jsonStateString)?.AsObject() ?? throw new InvalidOperationException("Invalid JSON data.");

            if (!string.IsNullOrEmpty(playerX)) jsonObject["PlayerX"] = playerX;
            if (!string.IsNullOrEmpty(playerO)) jsonObject["PlayerO"] = playerO;

            File.WriteAllText(fileName, jsonObject.ToJsonString());
            return $"{sanitizedName}_{timestamp}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving the game: {ex.Message}");
            return $"Error: {ex.Message}";
        }
    }
    
    /// <summary>
    /// Retrieves a list of all saved game names in the database.
    /// </summary>
    public List<string> GetSavedGameNames()
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        
        return files.Select(filePath => Path.GetFileNameWithoutExtension(filePath)).ToList();
    }
    
    /// <summary>
    /// Retrieves a list of saved games associated with a specific username.
    /// </summary>
    public List<string> GetUsernameSavedGameNames(string username)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        var matchingFiles = new List<string>();

        foreach (var filePath in files)
        {
            var jsonObject = ParseJsonFile(filePath);

            if (jsonObject != null)
            {
                var playerX = jsonObject["PlayerX"]?.GetValue<string>();
                var playerO = jsonObject["PlayerO"]?.GetValue<string>();

                if (username.Equals(playerX) || username.Equals(playerO))
                {
                    matchingFiles.Add(Path.GetFileNameWithoutExtension(filePath));
                }
            }
            
        }
        return matchingFiles;
    }

    /// <summary>
    /// Retrieves a list of games that are available to join for a given username.
    /// A game is considered joinable if it has an empty player slot.
    /// </summary>
    public List<string> GetFreeJoinGames(string username)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        var matchingFiles = new List<string>();

        foreach (var filePath in files)
        {
           
            var jsonObject = ParseJsonFile(filePath);

            if (jsonObject != null)
            {
                var playerX = jsonObject["PlayerX"]?.GetValue<string>();
                var playerO = jsonObject["PlayerO"]?.GetValue<string>();

                if (!username.Equals(playerX) && playerO == "Player-0" || !username.Equals(playerO) && playerX == "Player-X")
                {
                    matchingFiles.Add(Path.GetFileNameWithoutExtension(filePath));
                }
            }
        }
        return matchingFiles;
    }

    /// <summary>
    /// Finds and retrieves the state of a saved game based on its unique name.
    /// </summary>
    public string? FindSavedGame(string gameName)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        
        var filePath = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).StartsWith(gameName));
        
        return filePath != null ? File.ReadAllText(filePath) : null;
    }
    
    /// <summary>
    /// Updates an existing saved game state in the database with a new state.
    /// If the game already exists, it is removed and replaced with the updated state.
    /// </summary>
    public string UpdateGame(string jsonStateString, string gameName, GameConfiguration gameConfiguration, string? username)
    {
        var filePath = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}")
            .FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).StartsWith(gameName));
    
        if (filePath == null) return $"Error: Game '{gameName}' not found.";

        try
        {
            var jsonObject = JsonNode.Parse(jsonStateString)?.AsObject() ?? throw new InvalidOperationException("Invalid JSON data.");
            
            if (jsonObject["PlayerO"]?.GetValue<string>() == "Player-0") jsonObject["PlayerO"] = username;
            if (jsonObject["PlayerX"]?.GetValue<string>() == "Player-X") jsonObject["PlayerX"] = username;
            
            File.WriteAllText(filePath, jsonStateString);
            
            return gameName;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating the game: {ex.Message}");
            return $"Error: {ex.Message}";
        }
    }
    
    private JsonObject? ParseJsonFile(string filePath)
    {
        try
        {
            var content = File.ReadAllText(filePath);
            return JsonNode.Parse(content)?.AsObject();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file '{filePath}': {ex.Message}");
            return null;
        }
    }
   
}

