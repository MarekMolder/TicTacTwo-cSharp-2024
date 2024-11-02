using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public void Savegame(string jsonStateString, GameConfiguration gameConfig);
    GameState LoadGame(string gameConfigName);
    List<string> GetSavedGameNames();
    string? FindSavedGame(string gameName);
}