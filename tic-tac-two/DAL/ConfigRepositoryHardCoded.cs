using Domain;

namespace DAL;

public class ConfigRepositoryHardCoded
{
    /// <summary>
    /// A static readonly list of predefined game configurations.
    /// </summary>
    private readonly List<GameConfiguration> _gameConfigurations =
    [
        new GameConfiguration()
        {
            Name = "Classical", // Name of the configuration
            BoardSizeWidth = 3, // Width of the game board
            BoardSizeHeight = 3, // Height of the game board
            PiecesNumber = 5, // Number of pieces available for each player
            WinCondition = 3, // Number of pieces in a row needed to win
            UsesGrid = false, // Indicates whether a grid is used
            GridSizeWidth = 0, // Width of the grid (0 if not used)
            GridSizeHeight = 0, // Height of the grid (0 if not used)
            MovePieceAfterNMove = 10, // Number of moves before a piece can be moved
            MoveGridAfterNMove = 100, // Number of moves before the grid can be moved
            GridPositionX = 0, // X position of the grid
            GridPositionY = 0 // Y position of the grid
        },

        new GameConfiguration()
        {
            Name = "Regular",
            BoardSizeWidth = 5,
            BoardSizeHeight = 5,
            PiecesNumber = 25,
            WinCondition = 3,
            UsesGrid = true,
            GridSizeHeight = 3,
            GridSizeWidth = 3,
            MovePieceAfterNMove = 2,
            MoveGridAfterNMove = 2,
            GridPositionX = 1,
            GridPositionY = 1
        },
        
        new GameConfiguration()
        {
        Name = "Custom"
        }
    ];

    /// <summary>
    /// Retrieves the names of all available game configurations, sorted alphabetically.
    /// </summary>
    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }
    
    /// <summary>
    /// Gets a specific game configuration by its name.
    /// </summary>
    public GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }
}
