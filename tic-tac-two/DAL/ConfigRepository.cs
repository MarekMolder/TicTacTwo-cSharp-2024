using GameBrain;

namespace DAL;

public class ConfigRepository
{
    private static List<GameConfiguration> _gameConfigurations = new List<GameConfiguration>()
    {
        new GameConfiguration()
        {
            Name = "Classical",
            BoardSizeWidth = 3,
            BoardSizeHeight = 3,
            PiecesNumber = 5,
            WinCondition = 3,
            GridSizeWidth = 3,
            GridSizeHeight = 3,
            MovePieceAfterNMove = 0,
            MoveGridAfterNMove = 0,
        },
        new GameConfiguration()
        {
            Name = "Regular tic-tac-two",
            BoardSizeWidth = 5,
            BoardSizeHeight = 5,
            PiecesNumber = 4,
            WinCondition = 3,
            GridSizeHeight = 3,
            GridSizeWidth = 3,
            MovePieceAfterNMove = 2,
            MoveGridAfterNMove = 2,
        }
    };

    public static List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }
    
    public static GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }
}