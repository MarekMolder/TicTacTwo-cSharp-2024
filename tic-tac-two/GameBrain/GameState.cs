using Domain;

namespace GameBrain;

/// <summary>
/// Represents the state of the game, including the game board, players, and various game configurations.
/// </summary>
public class GameState
{
    /// <summary>
    /// The game board represented as a 2D array of game pieces.
    /// </summary>
    public EGamePiece[][] GameBoard { get; set; }
    
    /// <summary>
    /// The current player (X or O).
    /// </summary>
    public EGamePiece CurrentPlayer { get; set; }

    /// <summary>
    /// Configuration settings for the game, such as board size, win conditions, etc.
    /// </summary>
    public GameConfiguration GameConfiguration { get; set; }

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
    public string PlayerX { get; set; }
    
    /// <summary>
    /// Name of player O.
    /// </summary>
    public string PlayerO { get; set; }
    
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
    /// <param name="gameBoard">A 2D array representing the game board.</param>
    /// <param name="gameConfiguration">Configuration settings for the game.</param>
    /// <param name="currentPlayer">The player who is currently taking their turn (X or O).</param>
    /// <param name="piecesLeftX">Remaining pieces for player X.</param>
    /// <param name="piecesLeftO">Remaining pieces for player O.</param>
    /// <param name="movesMadeX">Number of moves made by player X.</param>
    /// <param name="movesMadeO">Number of moves made by player O.</param>
    /// <param name="playerX">Name of player X.</param>
    /// <param name="playerO">Name of player O.</param>
    /// <param name="gridPositionX">X-coordinate of the most recent move.</param>
    /// <param name="gridPositionY">Y-coordinate of the most recent move.</param>
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
    /// Serializes the current state of the game into a JSON string format.
    /// </summary>
    /// <returns>A JSON string representation of the game state.</returns>
    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}