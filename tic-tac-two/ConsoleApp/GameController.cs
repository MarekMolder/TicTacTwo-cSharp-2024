using System.Text.Json;
using ConsoleUI;
using DAL;
using GameBrain;

namespace ConsoleApp;

public class GameController
{
    private readonly IGameRepository _gameRepository;
    private readonly IConfigRepository _configRepository;

    // Konstruktor, mis võtab vastu IGameRepository eksemplari
    public GameController(IGameRepository gameRepository, IConfigRepository configRepository)
    {
        _gameRepository = gameRepository;
        _configRepository = configRepository;
    }
    
    public void NewGame(GameConfiguration chosenConfig, string playerX, string playerO)
    {
        var gameInstance = new TicTacTwoBrain(chosenConfig, playerX, playerO);
        var currentPlayerName = playerX;
        
        do
        {
            DisplayBoard(gameInstance, currentPlayerName, playerX, playerO);

            if (gameInstance.SaveTheGame())
            {
                _gameRepository.Savegame(gameInstance.GetGameStateJson(), chosenConfig);
                break;
            }
            
            if (!gameInstance.MakeAMove())
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

            if (gameInstance.SaveTheGame())
            {
                _gameRepository.Savegame(gameInstance.GetGameStateJson(), gameConfig);
                break;
            }
            
            if (!gameInstance.MakeAMove())
            {
                Console.WriteLine("Invalid move. Try again.");
                continue;
            }

            // Check if the game has ended
            if (CheckEndGameConditions(gameInstance, playerX, playerO))
            {
                break;
            }

            // Switch players
            currentPlayerName = currentPlayerName == playerX ? playerO : playerX;

        } while (true);
        
    }
    
    private void DisplayBoard(TicTacTwoBrain gameInstance, string currentPlayerName, string playerX, string playerO)
    {
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine($"Current Player: {currentPlayerName} ({gameInstance.CurrentPlayer})");
        Console.WriteLine($"{playerX} has {gameInstance.PiecesLeftX} pieces left.");
        Console.WriteLine($"{playerO} has {gameInstance.PiecesLeftO} pieces left.");
    }
    
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
    
    private void DisplayWinner(string winningPlayerName, TicTacTwoBrain gameInstance)
    {
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine($"Player {winningPlayerName} wins!");
    }
}
