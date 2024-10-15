using GameBrain;

namespace DAL;

/// <summary>
/// Abstract base class for managing game configurations.
/// </summary>
public abstract class ConfigRepository
{
    /// <summary>
    /// A static readonly list of predefined game configurations.
    /// </summary>
    private static readonly List<GameConfiguration> GameConfigurations =
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
            Name = "Regular tic-tac-two", // Name of the configuration
            BoardSizeWidth = 5, // Width of the game board
            BoardSizeHeight = 5, // Height of the game board
            PiecesNumber = 4, // Number of pieces available for each player
            WinCondition = 3, // Number of pieces in a row needed to win
            UsesGrid = true, // Indicates that a grid is used
            GridSizeHeight = 3, // Height of the grid
            GridSizeWidth = 3, // Width of the grid
            MovePieceAfterNMove = 2, // Number of moves before a piece can be moved
            MoveGridAfterNMove = 2, // Number of moves before the grid can be moved
            GridPositionX = 1, // X position of the grid
            GridPositionY = 1 // Y position of the grid
        }
    ];

    /// <summary>
    /// Retrieves the names of all available game configurations, sorted alphabetically.
    /// </summary>
    /// <returns>A list of configuration names.</returns>
    public static List<string> GetConfigurationNames()
    {
        return GameConfigurations
            .OrderBy(x => x.Name) // Order configurations by name
            .Select(config => config.Name) // Select the name of each configuration
            .ToList(); // Convert to a list
    }
    
    /// <summary>
    /// Gets a specific game configuration by its name.
    /// </summary>
    /// <param name="name">The name of the game configuration to retrieve.</param>
    /// <returns>The game configuration matching the provided name.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no configuration matches the name.</exception>
    public static GameConfiguration GetConfigurationByName(string name)
    {
        return GameConfigurations.Single(c => c.Name == name); // Retrieve the configuration with the matching name
    }
}
