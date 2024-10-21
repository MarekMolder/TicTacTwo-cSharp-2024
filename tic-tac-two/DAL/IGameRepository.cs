using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public void Savegame(string jsonStateString, string gameConfigName);
}