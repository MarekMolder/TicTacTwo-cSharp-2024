using Domain;

namespace GameBrain;

public class GameState
{
    /// <summary>
    /// The game board represented as a 2D array of game pieces.
    /// </summary>
    public EGamePiece[][] GameBoard { get; set; } = null!;

    /// <summary>
    /// The current player (X or O).
    /// </summary>
    public EGamePiece CurrentPlayer { get; set; } = EGamePiece.X;

    /// <summary>
    /// Configuration settings for the game, such as board size, win conditions, etc.
    /// </summary>
    public GameConfiguration GameConfiguration { get; set; } = null!;

    /// <summary>
    /// Remaining pieces for player X.
    /// </summary>
    public int PiecesLeftX { get; set; }
    
    /// <summary>
    /// Remaining pieces for player O.
    /// </summary>
    public int PiecesLeftO { get; set; } 
    
    /// <summary>
    /// Number of moves made by player X.
    /// </summary>
    public int MovesMadeX { get; set; }
    
    /// <summary>
    /// Number of moves made by player O.
    /// </summary>
    public int MovesMadeO{ get; set; } 
    
    /// <summary>
    /// Name of player X.
    /// </summary>
    public string PlayerX { get; set; } = null!;

    /// <summary>
    /// Name of player O.
    /// </summary>
    public string PlayerO { get; set; } = null!;

    /// <summary>
    /// X-coordinate of the most recent move (used for tracking where a piece was placed).
    /// </summary>
    public int GridPositionX { get; set; }
    
    /// <summary>
    /// Y-coordinate of the most recent move (used for tracking where a piece was placed).
    /// </summary>
    public int GridPositionY { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GameState"/> class with specified game parameters.
    /// </summary>
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
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GameState"/> class with specified game parameters.
    /// </summary>
    public GameState(GameConfiguration gameConfiguration, string playerX, string playerO)
    {
        GameConfiguration = gameConfiguration;
        PlayerX = playerX;
        PlayerO = playerO;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GameState"/> class with specified game parameters.
    /// </summary>
    public GameState(EGamePiece[][] gameBoard, GameConfiguration gameConfiguration)
    {
        GameBoard = gameBoard;
        GameConfiguration = gameConfiguration;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameState"/> class.
    /// </summary>
    public GameState()
    {
        
    }
    
    /// <summary>
    /// Serializes the current state of the game into a JSON string format.
    /// </summary>
    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
