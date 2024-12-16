using System.Text.Json;
using Domain;
using GameBrain;
using System.IO;
using System.Text.Json.Nodes;

namespace DAL;

/// <summary>
/// A repository implementation for saving, loading, and managing game data stored in JSON files.
/// </summary>
public class GameRepositoryJson : IGameRepository
{
    private IGameRepository _gameRepositoryImplementation;
    
    public string Savegame(string jsonStateString, GameConfiguration gameConfig, string? playerX = null, string? playerO = null)
    {
        // Ensure gameConfig.Name is sanitized for file naming
        string sanitizedGameConfigName = string.Join("_", gameConfig.Name.Split(Path.GetInvalidFileNameChars()));

        // Replace colons in the timestamp for compatibility with file names
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        var fileName = Path.Combine(FileHelper.BasePath, $"{sanitizedGameConfigName}_{timestamp}{FileHelper.GameExtension}");
        

        try
        {
            var jsonObject = JsonNode.Parse(jsonStateString)?.AsObject();

            if (jsonObject == null)
            {
                throw new InvalidOperationException("Invalid JSON data.");
            }

            if (!string.IsNullOrEmpty(playerX))
            {
                jsonObject["playerX"] = playerX;
            }
            
            if (!string.IsNullOrEmpty(playerO))
            {
                jsonObject["PlayerO"] = playerO;
            }
            

            System.IO.File.WriteAllText(fileName, jsonStateString);
            // Return the sanitized game name, or you can return the full file name if needed.
            return sanitizedGameConfigName + "_" + timestamp;
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error or inform the user)
            Console.WriteLine($"An error occurred while saving the game: {ex.Message}");
            return $"Error: {ex.Message}";
        }
    }
    
    public string UpdateGame(string jsonStateString, string gameName, GameConfiguration gameConfiguration, string username)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
    
        // Find the first matching file path
        var filePath = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).StartsWith(gameName));
    
        if (filePath == null)
        {
            return $"Error: Game with the name '{gameName}' not found.";
        }

        try
        {
            var jsonObject = JsonNode.Parse(jsonStateString)?.AsObject();

            if (jsonObject == null)
            {
                throw new InvalidOperationException("Invalid JSON data.");
            }
            
            if (jsonObject["PlayerO"]?.GetValue<string>() == "Player-0")
            {
                jsonObject["PlayerO"] = username;
            }

            // Update PlayerX if the current value is "Player-X" and playerX is not empty
            if (jsonObject["PlayerX"]?.GetValue<string>() == "Player-X")
            {
                jsonObject["PlayerX"] = username;
            }
            
            // Create a new file with the new name and updated game state
            System.IO.File.WriteAllText(filePath, jsonStateString);

            // Return the new name (sanitized game name with the new timestamp)
            return gameName;
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error or inform the user)
            Console.WriteLine($"An error occurred while updating the game: {ex.Message}");
            return $"Error: {ex.Message}";
        }
    }
    
    public List<string> GetUsernameSavedGameNames(string username)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        var matchingFiles = new List<string>();

        foreach (var filePath in files)
        {
            try
            {
                // Read the file content
                var fileContent = System.IO.File.ReadAllText(filePath);
            
                // Parse the JSON content
                var jsonObject = JsonNode.Parse(fileContent)?.AsObject();

                if (jsonObject != null)
                {
                    // Check if PlayerX or PlayerO equals the given username
                    var playerX = jsonObject["PlayerX"]?.GetValue<string>();
                    var playerO = jsonObject["PlayerO"]?.GetValue<string>();

                    if (username.Equals(playerX) || username.Equals(playerO))
                    {
                        // Add the file name without extension to the list if username matches
                        matchingFiles.Add(Path.GetFileNameWithoutExtension(filePath));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error or inform the user)
                Console.WriteLine($"An error occurred while processing file {filePath}: {ex.Message}");
            }
        }

        return matchingFiles;
    }


    public List<string> GetFreeJoinGames(string username)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        var matchingFiles = new List<string>();

        foreach (var filePath in files)
        {
            try
            {
                // Read the file content
                var fileContent = System.IO.File.ReadAllText(filePath);
            
                // Parse the JSON content
                var jsonObject = JsonNode.Parse(fileContent)?.AsObject();

                if (jsonObject != null)
                {
                    // Check if PlayerX or PlayerO equals the given username
                    var playerX = jsonObject["PlayerX"]?.GetValue<string>();
                    var playerO = jsonObject["PlayerO"]?.GetValue<string>();

                    if (!username.Equals(playerX) && playerO == "Player-0" || !username.Equals(playerO) && playerX == "Player-X")
                    {
                        // Add the file name without extension to the list if username matches
                        matchingFiles.Add(Path.GetFileNameWithoutExtension(filePath));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error or inform the user)
                Console.WriteLine($"An error occurred while processing file {filePath}: {ex.Message}");
            }
        }

        return matchingFiles;
    }

    public string? FindSavedGame(string gameName)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
    
        // Find the first matching file path
        var filePath = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).StartsWith(gameName));
    
        // If filePath is null, return null; otherwise, read the file
        return filePath != null ? File.ReadAllText(filePath) : null;
    }
    
    public List<string> GetSavedGameNames()
    {
        // Search for all game files in the specified directory
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");

        // Return the file names without extensions
        return files.Select(filePath => Path.GetFileNameWithoutExtension(filePath)).ToList();
    }
   
}

