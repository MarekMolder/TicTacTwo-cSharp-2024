/*
using Domain;
using GameBrain;

namespace DAL;

public interface IGameRepository2
{
    void Save(Guid id, GameState state, GameConfiguration gameConfiguration);
    List<(Guid id, DateTime dt)> GetSavedGames();

    GameState LoadGame(Guid id);
    void DeleteGame(Guid gameId);
    string? FindSavedGame(string gameName);
}
*/