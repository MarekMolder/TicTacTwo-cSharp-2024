using System.Text.Json;
using Domain;
using GameBrain;

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
    public void Savegame(string jsonStateString, GameConfiguration gameConfig)
    {
        // Ensure gameConfig.Name is sanitized for file naming
        string sanitizedGameConfigName = string.Join("_", gameConfig.Name.Split(Path.GetInvalidFileNameChars()));

        // Replace colons in the timestamp for compatibility with file names
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        var fileName = Path.Combine(FileHelper.BasePath, $"{sanitizedGameConfigName}_{timestamp}{FileHelper.GameExtension}");

        try
        {
            // Write the JSON string to a file
            System.IO.File.WriteAllText(fileName, jsonStateString);
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error or inform the user)
            Console.WriteLine($"An error occurred while saving the game: {ex.Message}");
        }

        //return -1;
    }
    
    /// <summary>
    /// Finds a saved game by its name.
    /// </summary>
    /// <param name="gameName">The name of the saved game to find.</param>
    /// <returns>The file path of the saved game if found; otherwise, null.</returns>
    public string? FindSavedGame(string gameName)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        return files.FirstOrDefault(filePath => Path.GetFileNameWithoutExtension(filePath).StartsWith(gameName));
    }
    
    /// <summary>
    /// Loads the game state from a JSON file.
    /// </summary>
    /// <param name="filePath">The path to the saved game file.</param>
    /// <returns>A <see cref="GameState"/> object representing the saved game state.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file at the specified path does not exist.</exception>
    public GameState LoadGame(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Saved game not found at {filePath}.");
        }

        // Read the file content and deserialize it into a GameState object
        var jsonState = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<GameState>(jsonState);
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