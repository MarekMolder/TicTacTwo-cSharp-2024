using System.Text.Json;
using ConsoleUI;
using DAL;
using GameBrain;

namespace ConsoleApp;

/// <summary>
/// The <see cref="GameController"/> class manages the game flow, including starting a new game, resuming a saved game,
/// displaying the game board, checking end-game conditions, and saving the game state.
/// </summary>
public class GameController
{
    private readonly IGameRepository _gameRepository;
    private readonly IConfigRepository _configRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameController"/> class.
    /// </summary>
    /// <param name="gameRepository">The repository for saving and loading game data.</param>
    /// <param name="configRepository">The repository for accessing game configurations.</param>
    public GameController(IGameRepository gameRepository, IConfigRepository configRepository)
    {
        _gameRepository = gameRepository;
        _configRepository = configRepository;
    }
    
    /// <summary>
    /// Starts a new game using the selected game configuration and player names.
    /// </summary>
    /// <param name="chosenConfig">The configuration for the new game.</param>
    /// <param name="playerX">The name of the player using 'X'.</param>
    /// <param name="playerO">The name of the player using 'O'.</param>
    public void NewGame(GameConfiguration chosenConfig, string playerX, string playerO)
    {
        var gameInstance = new TicTacTwoBrain(chosenConfig, playerX, playerO);
        var currentPlayerName = playerX;
        
        do
        {
            DisplayBoard(gameInstance, currentPlayerName, playerX, playerO);
            var moveResult = gameInstance.MakeAMove();

            if (moveResult == MoveResult.SaveGame)
            {
                _gameRepository.Savegame(gameInstance.GetGameStateJson(), chosenConfig);
                break;
            }
            
            if (moveResult == MoveResult.InvalidMove)
            {
                Console.WriteLine("Invalid move. Try again.");
                continue;
            }
            
            if (CheckEndGameConditions(gameInstance, playerX, playerO))
            {
                break;
            }
            
            currentPlayerName = currentPlayerName == playerX ? playerO : playerX;

        } while (true);
        
    }
    
    /// <summary>
    /// Resumes a previously saved game and continues from the saved state.
    /// </summary>
    /// <param name="gameBoard">The saved game board state.</param>
    /// <param name="gameConfig">The game configuration used when the game was saved.</param>
    /// <param name="currentPlayer">The player whose turn it is.</param>
    /// <param name="piecesLeftX">The number of pieces left for player 'X'.</param>
    /// <param name="piecesLeftO">The number of pieces left for player 'O'.</param>
    /// <param name="movesMadeX">The number of moves made by player 'X'.</param>
    /// <param name="movesMadeO">The number of moves made by player 'O'.</param>
    /// <param name="playerX">The name of player 'X'.</param>
    /// <param name="playerO">The name of player 'O'.</param>
    /// <param name="gridPositionX">The X position of the grid where the game was saved.</param>
    /// <param name="gridPositionY">The Y position of the grid where the game was saved.</param>
    public void OldGame(EGamePiece[][] gameBoard, GameConfiguration gameConfig, EGamePiece currentPlayer, int piecesLeftX, int piecesLeftO, int movesMadeX, int movesMadeO, string playerX, string playerO, int gridPositionX, int gridPositionY)
    {
        var gameInstance = new TicTacTwoBrain(gameConfig, playerX, playerO, gridPositionX, gridPositionY, movesMadeX, movesMadeO);
        
        gameInstance.SetGameBoard(gameBoard);
        gameInstance.CurrentPlayer = currentPlayer;
        gameInstance.PiecesLeftX = piecesLeftX;
        gameInstance.PiecesLeftO = piecesLeftO;
        gameInstance.MovesMadeX = movesMadeX;
        gameInstance.MovesMadeO = movesMadeO;
        
        var currentPlayerName = currentPlayer == EGamePiece.X ? playerX : playerO;
        
        do
        {
            DisplayBoard(gameInstance, currentPlayerName, playerX, playerO);
            var moveResult = gameInstance.MakeAMove();

            if (moveResult == MoveResult.SaveGame)
            {
                _gameRepository.Savegame(gameInstance.GetGameStateJson(), gameConfig);
                break;
            }
            
            if (moveResult == MoveResult.InvalidMove)
            {
                Console.WriteLine("Invalid move. Try again.");
                continue;
            }
            
            if (CheckEndGameConditions(gameInstance, playerX, playerO))
            {
                break;
            }
            
            currentPlayerName = currentPlayerName == playerX ? playerO : playerX;

        } while (true);
        
    }
    
    /// <summary>
    /// Displays the current game board along with player names and remaining pieces.
    /// </summary>
    /// <param name="gameInstance">The current instance of the game.</param>
    /// <param name="currentPlayerName">The name of the current player.</param>
    /// <param name="playerX">The name of player 'X'.</param>
    /// <param name="playerO">The name of player 'O'.</param>
    private void DisplayBoard(TicTacTwoBrain gameInstance, string currentPlayerName, string playerX, string playerO)
    {
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine($"Current Player: {currentPlayerName} ({gameInstance.CurrentPlayer})");
        Console.WriteLine($"{playerX} has {gameInstance.PiecesLeftX} pieces left.");
        Console.WriteLine($"{playerO} has {gameInstance.PiecesLeftO} pieces left.");
    }
    
    /// <summary>
    /// Checks whether the game has ended due to a win or a draw.
    /// </summary>
    /// <param name="gameInstance">The current instance of the game.</param>
    /// <param name="playerX">The name of player 'X'.</param>
    /// <param name="playerO">The name of player 'O'.</param>
    /// <returns>True if the game has ended, otherwise false.</returns>
    private bool CheckEndGameConditions(TicTacTwoBrain gameInstance, string playerX, string playerO)
    {
        var winner = gameInstance.CheckWin();
        if (winner != null)
        {
            DisplayWinner(winner == EGamePiece.X ? playerX : playerO, gameInstance);
            return true;
        }

        // Check for a draw
        if (!gameInstance.CheckDraw()) return false;
        
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine("It's a draw! Either no more pieces left or the board is full.");
        return true;
    }
    
    /// <summary>
    /// Displays the winner of the game.
    /// </summary>
    /// <param name="winningPlayerName">The name of the player who won the game.</param>
    /// <param name="gameInstance">The current instance of the game.</param>
    private void DisplayWinner(string winningPlayerName, TicTacTwoBrain gameInstance)
    {
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine($"Player {winningPlayerName} wins!");
    }
}
