using Domain;
using GameBrain;

namespace DAL;

/// <summary>
/// Defines the operations for interacting with game data storage.
/// </summary>
public interface IGameRepository
{
    public string Savegame(string jsonStateString, GameConfiguration gameConfig);
    
    List<string> GetSavedGameNames();
    
    string? FindSavedGame(string gameName);
}