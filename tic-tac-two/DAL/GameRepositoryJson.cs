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
    private readonly Dictionary<int, string> _gameIdToFilePathMap = new Dictionary<int, string>();
    private static int _currentGameId = 1; // Start with an ID of 1 for the first game.

    /// <summary>
    /// Saves the game state as a JSON string to a file.
    /// </summary>
    /// <param name="jsonStateString">The JSON string representing the game state.</param>
    /// <param name="gameConfig">The game configuration associated with the saved game.</param>
    public int Savegame(string jsonStateString, GameConfiguration gameConfig)
    {
        // Ensure the game configuration name is sanitized for use in a file name
        string sanitizedGameConfigName = string.Join("_", gameConfig.Name.Split(Path.GetInvalidFileNameChars()));

        // Replace colons and other invalid characters in the timestamp for compatibility with file names
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        // Generate a unique ID (increases with each save)
        int uniqueGameId = _currentGameId++;

        // Create the file name based on the sanitized configuration name and the timestamp
        var fileName = Path.Combine(FileHelper.BasePath, $"{sanitizedGameConfigName}_{timestamp}_{uniqueGameId}{FileHelper.GameExtension}");

        try
        {
            // Save the JSON state to the file
            System.IO.File.WriteAllText(fileName, jsonStateString);

            // Map the generated game ID to the file path for future access (optional)
            _gameIdToFilePathMap[uniqueGameId] = fileName;

            // Return the unique game ID assigned to this save
            return uniqueGameId;
        }
        catch (Exception ex)
        {
            // Handle any errors during the save process
            Console.WriteLine($"An error occurred while saving the game: {ex.Message}");
            return -1;
        }
    }
    
    public string? FindSavedGame(string gameName)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        return files.FirstOrDefault(filePath => Path.GetFileNameWithoutExtension(filePath).StartsWith(gameName));
    }

    /// <summary>
    /// Loads the game state from a JSON file by the game ID.
    /// </summary>
    /// <param name="gameId">The ID of the saved game to load.</param>
    /// <returns>A <see cref="SaveGame"/> object representing the saved game.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file at the specified path does not exist.</exception>
    public SaveGame LoadGame(int gameId)
    {
        // Look up the file path for the given game ID
        var filePath = _gameIdToFilePathMap.Values.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).EndsWith(gameId.ToString()));

        if (filePath == null)
        {
            throw new FileNotFoundException($"Saved game not found with ID {gameId}.");
        }

        // Check if the file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Saved game not found at {filePath}.");
        }

        try
        {
            // Read the JSON game state from the file
            var jsonState = File.ReadAllText(filePath);

            // Deserialize the JSON string into a GameState object
            var gameState = JsonSerializer.Deserialize<GameState>(jsonState)
                            ?? throw new Exception("Failed to deserialize the game state.");

            // Return a SaveGame object that includes the game state and configuration
            return new SaveGame
            {
                Id = gameId,
                CreatedAtDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                State = jsonState,
                ConfigurationId = gameState.GameConfiguration?.Id ?? -1
            };
        }
        catch (Exception ex)
        {
            // Handle any errors during the load process
            Console.WriteLine($"An error occurred while loading the game: {ex.Message}");
            throw;
        }
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
