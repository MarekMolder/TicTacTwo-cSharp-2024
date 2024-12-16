using Domain;
using GameBrain;

namespace DAL;

/// <summary>
/// Defines the operations for interacting with game data storage.
/// </summary>
public interface IGameRepository
{
    public string Savegame(string jsonStateString, GameConfiguration gameConfig, string? playerX = null, string? playerO = null);
    
    List<string> GetSavedGameNames();
    
    List<string> GetUsernameSavedGameNames(string username);
    
    List<string> GetFreeJoinGames(string username);
    
    string? FindSavedGame(string gameName);

    public string UpdateGame(string jsonStateString, string gameName, GameConfiguration gameConfig, string? username);
}