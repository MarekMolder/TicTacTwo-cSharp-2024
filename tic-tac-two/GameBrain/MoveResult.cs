namespace GameBrain;

/// <summary>
/// Represents the possible results of a move in the game.
/// </summary>
public enum MoveResult
{
    /// <summary>
    /// Indicates that the game should continue after the move.
    /// </summary>
    Continue,
    
    /// <summary>
    /// Indicates that the game should be saved after the move.
    /// </summary>
    SaveGame,
    
    /// <summary>
    /// Indicates that the move was invalid and cannot be executed.
    /// </summary>
    InvalidMove
}