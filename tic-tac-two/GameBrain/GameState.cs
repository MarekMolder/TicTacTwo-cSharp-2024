namespace GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; } // The game board represented as a 2D array of game pieces.
    public EGamePiece CurrentPlayer { get; set; } // The current player (X or O).

    public GameConfiguration GameConfiguration { get; set; }  // Configuration settings for the game.

    public int PiecesLeftX { get; set; }  // Remaining pieces for player X.
    public int PiecesLeftO { get; set; }  // Remaining pieces for player O.
    public int MovesMadeX { get; set; }  // Number of moves made by player X.
    public int MovesMadeO{ get; set; }  // Number of moves made by player O.

    public GameState(EGamePiece[][] gameBoard, GameConfiguration gameConfiguration, EGamePiece currentPlayer, int piecesLeftX, int piecesLeftO, int movesMadeX, int movesMadeO)
    {
        GameBoard = gameBoard;
        GameConfiguration = gameConfiguration;
        CurrentPlayer = currentPlayer;
        PiecesLeftX = piecesLeftX;
        PiecesLeftO = piecesLeftO;
        MovesMadeX = movesMadeX;
        MovesMadeO = movesMadeO;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}