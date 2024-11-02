using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public void Savegame(string jsonStateString, string gameConfigName);
    GameState LoadGame(string gameConfigName);
    List<string> GetSavedGameNames();
    string? FindSavedGame(string gameName);
}