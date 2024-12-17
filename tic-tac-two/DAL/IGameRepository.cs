using Domain;

namespace DAL;

/// <summary>
/// Defines the operations for interacting with game data storage.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Saves a game state and its configuration to the database.
    /// </summary>
    public string Savegame(string jsonStateString, GameConfiguration gameConfig, string? playerX = null, string? playerO = null);
    
    /// <summary>
    /// Retrieves a list of all saved game names in the database.
    /// </summary>
    List<string> GetSavedGameNames();
    
    /// <summary>
    /// Retrieves a list of saved games associated with a specific username.
    /// </summary>
    List<string> GetUsernameSavedGameNames(string username);
    
    /// <summary>
    /// Retrieves a list of games that are available to join for a given username.
    /// A game is considered joinable if it has an empty player slot.
    /// </summary>
    List<string> GetFreeJoinGames(string username);
    
    /// <summary>
    /// Finds and retrieves the state of a saved game based on its unique name.
    /// </summary>
    string? FindSavedGame(string gameName);

    /// <summary>
    /// Updates an existing saved game state in the database with a new state.
    /// If the game already exists, it is removed and replaced with the updated state.
    /// </summary>
    public string UpdateGame(string jsonStateString, string gameName, GameConfiguration gameConfig, string? username);
}