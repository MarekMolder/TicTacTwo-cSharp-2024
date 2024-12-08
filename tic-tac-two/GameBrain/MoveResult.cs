namespace GameBrain;

/// <summary>
/// Represents the possible results of a move in the game.
/// </summary>
public enum MoveResult
{
    HandleNoPiecesLeft,
    HandlePlayerChoice,
    SaveOrContinue,
    PromptMovePieceOrGrid,
    MoveExistingPiece,
    MoveGrid,
    PlaceNewPiece,
    SaveGame,
    InvalidMove,
    Continue
}