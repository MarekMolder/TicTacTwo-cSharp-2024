using System.Text.Json;
using Domain;
using GameBrain;
using System.IO;

namespace DAL;

/// <summary>
/// A repository implementation for saving, loading, and managing game data stored in JSON files.
/// </summary>
public class GameRepositoryJson : IGameRepository
{

    /// <summary>
    /// Saves the game state as a JSON string to a file.
    /// </summary>
    /// <param name="jsonStateString">The JSON string representing the game state.</param>
    /// <param name="gameConfig">The game configuration associated with the saved game.</param>
    public string Savegame(string jsonStateString, GameConfiguration gameConfig)
    {
        // Ensure gameConfig.Name is sanitized for file naming
        string sanitizedGameConfigName = string.Join("_", gameConfig.Name.Split(Path.GetInvalidFileNameChars()));

        // Replace colons in the timestamp for compatibility with file names
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        var fileName = Path.Combine(FileHelper.BasePath, $"{sanitizedGameConfigName}_{timestamp}{FileHelper.GameExtension}");

        try
        {
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

    
    public string? FindSavedGame(string gameName)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
    
        // Find the first matching file path
        var filePath = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).StartsWith(gameName));
    
        // If filePath is null, return null; otherwise, read the file
        return filePath != null ? File.ReadAllText(filePath) : null;
    }
    

    /// <summary>
    /// Retrieves a list of all saved game names (without extensions).
    /// </summary>
    /// <returns>A list of saved game names.</returns>
    public List<string> GetSavedGameNames()
    {
        // Search for all game files in the specified directory
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");

        // Return the file names without extensions
        return files.Select(filePath => Path.GetFileNameWithoutExtension(filePath)).ToList();
    }
}
