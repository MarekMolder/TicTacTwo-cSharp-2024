using GameBrain;

namespace DAL;

/// <summary>
/// Defines the operations for interacting with game data storage.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Saves the current state of the game to persistent storage.
    /// </summary>
    /// <param name="jsonStateString">A JSON string representing the serialized state of the game.</param>
    /// <param name="gameConfig">The game configuration used for the game session.</param>
    public void Savegame(string jsonStateString, GameConfiguration gameConfig);
    
    /// <summary>
    /// Loads a game state based on a given game configuration name.
    /// </summary>
    /// <param name="gameConfigName">The name of the game configuration to load the game state for.</param>
    /// <returns>A <see cref="GameState"/> representing the loaded game state.</returns>
    GameState LoadGame(string gameConfigName);
    
    /// <summary>
    /// Retrieves a list of saved game names.
    /// </summary>
    /// <returns>A list of strings representing the names of all saved games.</returns>
    List<string> GetSavedGameNames();
    
    /// <summary>
    /// Finds a saved game by its name.
    /// </summary>
    /// <param name="gameName">The name of the saved game to search for.</param>
    /// <returns>The game state as a string if the game is found, or <c>null</c> if not found.</returns>
    string? FindSavedGame(string gameName);
}