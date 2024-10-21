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
    public string PlayerX { get; set; }
    public string PlayerO { get; set; }
    public int GridPositionX { get; set; }
    public int GridPositionY { get; set; }

    public GameState(EGamePiece[][] gameBoard, GameConfiguration gameConfiguration, EGamePiece currentPlayer, int piecesLeftX, int piecesLeftO, int movesMadeX, int movesMadeO, string playerX, string playerO, int gridPositionX, int gridPositionY)
    {
        GameBoard = gameBoard;
        GameConfiguration = gameConfiguration;
        CurrentPlayer = currentPlayer;
        PiecesLeftX = piecesLeftX;
        PiecesLeftO = piecesLeftO;
        MovesMadeX = movesMadeX;
        MovesMadeO = movesMadeO;
        PlayerO = playerO;
        PlayerX = playerX;
        GridPositionX = gridPositionX;
        GridPositionY = gridPositionY;
    }

    

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}