using Domain;

namespace GameBrain;

/// <summary>
/// Represents the configuration settings for a game.
/// </summary>
public record GameConfiguration()
{
    // Primary key
    public int Id {get ; set; }
    
    /// <summary>
    /// Gets or sets the name of the game configuration.
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the width of the game board.
    /// </summary>
    public int BoardSizeWidth { get; set; }
    
    /// <summary>
    /// Gets or sets the height of the game board.
    /// </summary>
    public int BoardSizeHeight { get; set; }
    
    /// <summary>
    /// Gets or sets the number of pieces available for each player.
    /// </summary>
    public int PiecesNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the number of pieces in a row needed to win.
    /// </summary>
    public int WinCondition { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether a grid is used in the game.
    /// </summary>
    public bool UsesGrid { get; set; }
    
    /// <summary>
    /// Gets or sets the width of the grid.
    /// </summary>
    public int GridSizeWidth { get; set; }
    
    /// <summary>
    /// Gets or sets the height of the grid.
    /// </summary>
    public int GridSizeHeight { get; set; }
    
    /// <summary>
    /// Gets or sets the number of moves before a piece can be moved.
    /// </summary>
    public int MovePieceAfterNMove { get; set; }
    
    /// <summary>
    /// Gets or sets the number of moves before the grid can be moved.
    /// </summary>
    public int MoveGridAfterNMove { get; set; }

    /// <summary>
    /// Gets or sets the X position of the grid.
    /// </summary>
    public int GridPositionX { get; set; }
    
    /// <summary>
    /// Gets or sets the Y position of the grid.
    /// </summary>
    public int GridPositionY { get; set; }

    /// <summary>
    /// Returns a string representation of the game configuration.
    /// </summary>
    /// <returns>A formatted string displaying the configuration settings.</returns>
    
    public ICollection<SaveGame>? SaveGames { get; set; }


    public override string ToString() =>
        $"Id - {Id}" +
        $"| Name - {Name}" +
        $"| Board {BoardSizeWidth}x{BoardSizeHeight} " +
        $"| Uses grid {UsesGrid} " +
        $"| Grid {GridSizeWidth}x{GridSizeHeight} " +
        $"| Grid position: {GridPositionX},{GridPositionY} " +
        $"| To win: {WinCondition} " +
        $"| Can move pieces after {MovePieceAfterNMove} moves " +
        $"| Can move grid after {MoveGridAfterNMove} moves " +
        $"| Games: {SaveGames?.Count} ";

}
