using ConsoleUI;
using DAL;
using Domain;
using GameBrain;

namespace ConsoleApp;

public class GameController(IGameRepository gameRepository)
{
    private TicTacTwoBrain _gameInstance = null!;

    /// <summary>
    /// Main game loop to play the game.
    /// </summary>
    public void PlayGame(GameConfiguration chosenConfig, GameState gameState)
    {
        InitializeOrLoadGame(chosenConfig, gameState);

        var currentPlayerName = gameState.CurrentPlayer == EGamePiece.X ? gameState.PlayerX : gameState.PlayerO;

        do
        {
            DisplayBoard(_gameInstance, currentPlayerName, gameState.PlayerX, gameState.PlayerO);
            
            if (string.Equals(currentPlayerName, "AI", StringComparison.OrdinalIgnoreCase))
            {
               MakeAiMove(gameState);
               currentPlayerName = ChangeCurrentPlayer(currentPlayerName, gameState);
            }
            else
            {
              currentPlayerName = MakePlayerMove(chosenConfig, gameState, currentPlayerName);
              CheckEndGame(gameState);
              currentPlayerName = ChangeCurrentPlayer(currentPlayerName, gameState);
            }
        } while (true);
    }
    
    /// <summary>
    /// Initializes a new game or loads an existing game state.
    /// </summary>
    private void InitializeOrLoadGame(GameConfiguration chosenConfig, GameState gameState)
    {
        if (gameState.GameBoard == null)
        {
            _gameInstance = new TicTacTwoBrain(chosenConfig, gameState.PlayerX, gameState.PlayerO);
        }
        else
        {
            _gameInstance = new TicTacTwoBrain(chosenConfig, gameState.PlayerX, gameState.PlayerO,
                gameState.GridPositionX, gameState.GridPositionY, gameState.MovesMadeX, gameState.MovesMadeO);
            _gameInstance.SetGameBoard(gameState.GameBoard);
            _gameInstance.CurrentPlayer = gameState.CurrentPlayer;
            _gameInstance.PiecesLeftX = gameState.PiecesLeftX;
            _gameInstance.PiecesLeftO = gameState.PiecesLeftO;
            _gameInstance.MovesMadeX = gameState.MovesMadeX;
            _gameInstance.MovesMadeO = gameState.MovesMadeO;
        }
    }
    
    /// <summary>
    /// Displays the game board and the current player.
    /// </summary>
    private void DisplayBoard(TicTacTwoBrain gameInstance, string currentPlayerName, string playerX, string playerO)
    {
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine($"Current Player: {currentPlayerName} ({gameInstance.CurrentPlayer})");
        Console.WriteLine($"{playerX} has {gameInstance.PiecesLeftX} pieces left.");
        Console.WriteLine($"{playerO} has {gameInstance.PiecesLeftO} pieces left.");
    }

    /// <summary>
    /// Makes a move for the AI player.
    /// </summary>
    private void MakeAiMove(GameState gameState)
    {
        Console.WriteLine("AI is thinking...");
        var hasPiecesLeft = _gameInstance.HasPiecesLeft();

        if (hasPiecesLeft)
        {
            _gameInstance.AiMove();
        }
        
        CheckEndGame(gameState);
    }

    /// <summary>
    /// Checks if the game has ended.
    /// </summary>
    private void CheckEndGame(GameState gameState)
    { 
        if (CheckEndGameConditions(_gameInstance, gameState.PlayerX, gameState.PlayerO))
        {
            Console.WriteLine("Game over! Press Enter to exit.");
            TicTacTwoBrain.WaitForEnter();
            Environment.Exit(0);
        }
    }
    
    /// <summary>
    /// Changes the current player.
    /// </summary>
    private string ChangeCurrentPlayer(string currentPlayerName, GameState gameState) 
    {
         return currentPlayerName == gameState.PlayerX ? gameState.PlayerO : gameState.PlayerX;
    }

    /// <summary>
    /// Handles the player's move.
    /// </summary>
    private string MakePlayerMove(GameConfiguration chosenConfig, GameState gameState, string currentPlayerName)
    {
        var canMovePiece = _gameInstance.CanMovePiece();
        var canMoveGrid = _gameInstance.CanMoveGrid();
        var hasPiecesLeft = _gameInstance.HasPiecesLeft();

        string promptMessage;
        promptMessage =
            $" {(hasPiecesLeft ? "Do you want to place a new piece" : "")}{(canMovePiece ? ", move an existing piece" : "")}{(canMoveGrid ? ", move the grid" : "")}, save the game or exit the game?";
        
        var options = GenerateMoveOptions(canMovePiece, canMoveGrid, hasPiecesLeft);
        
        while (true)
        {
            Console.WriteLine(promptMessage);
            Console.WriteLine($"Options: {string.Join("/", options)}");

            var response = Console.ReadLine()?.Trim().ToLower();

            if (response == "new")
            {
                PlaceNewPiece();
                break;
            }

            if (response == "old" && canMovePiece)
            {
                MoveExistingPiece();
                break;
            }

            if (response == "grid" && canMoveGrid)
            {
                MoveGrid();
                break;
            }

            if (response == "save")
            {
                
                if (gameState.CurrentPlayer == EGamePiece.X)
                {
                    gameState.PiecesLeftX++;
                }
                else
                {
                    gameState.PiecesLeftO++; 
                }
                gameRepository.Savegame(_gameInstance.GetGameStateJson(), chosenConfig, gameState.PlayerX, gameState.PlayerO);
                currentPlayerName = ChangeCurrentPlayer(currentPlayerName, gameState);
                break;
            }

            if (response == "exit")
            {
                Program.Main();
            }

            Console.WriteLine("Invalid input. Please try again.");
        }
        return currentPlayerName;
    }
    
    /// <summary>
    /// Generates move options based on bool values
    /// </summary>
    private List<string> GenerateMoveOptions(bool hasPiecesLeft, bool canMovePiece, bool canMoveGrid)
    {
        var options = new List<string> { "save", "exit" };
        if (canMovePiece) options.Add("old");
        if (canMoveGrid) options.Add("grid");
        if (hasPiecesLeft) options.Add("new");
        return options;
    }
    
    /// <summary>
    /// Checks the end game conditions.
    /// </summary>
    private bool CheckEndGameConditions(TicTacTwoBrain gameInstance, string playerX, string playerO)
    {
        var winner = gameInstance.CheckWin();
        if (winner != null)
        {
            DisplayWinner(winner == EGamePiece.X ? playerX : playerO, gameInstance);
            return true;
        }
        
        if (!gameInstance.CheckDraw()) return false;
        
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine("It's a draw! Either no more pieces left or the board is full.");
        return true;
    }
    
    /// <summary>
    /// Displays the winner of the game.
    /// </summary>
    private void DisplayWinner(string winningPlayerName, TicTacTwoBrain gameInstance)
    {
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine($"Player {winningPlayerName} wins!");
    }
    
    /// <summary>
    /// Prompts the player to place a new piece.
    /// </summary>
    private void PlaceNewPiece()
    {
        while (true)
        {
            var (x, y) = GetCoordinatesFromPlayer("Enter the coordinates where you want to place your piece <x,y>:");
            
            if (!_gameInstance.PlaceNewPiece(x, y))
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                Console.WriteLine("Press Enter to continue...");
                TicTacTwoBrain.WaitForEnter();
                continue; 
            }
            
            break;
        }
    }

    /// <summary>
    /// Prompts the player to move an existing piece.
    /// </summary>
    private void MoveExistingPiece()
    {
        while (true)
        {
            var (oldX, oldY) = GetCoordinatesFromPlayer("Enter the coordinates of the piece you want to move <x,y>:");
            
            var (newX, newY) =
                GetCoordinatesFromPlayer("Enter the new coordinates where you want to move the piece <x,y>:");

            if (!_gameInstance.MoveExistingPiece(oldX, oldY , newX, newY))
            {
                Console.WriteLine("Invalid move. That is not your piece or the spot is already occupied.");
                Console.WriteLine("Press Enter to continue...");
                TicTacTwoBrain.WaitForEnter();
                continue;
            }

            break;
        }
    }

    /// <summary>
    /// Prompts the player to move the grid.
    /// </summary>
    private void MoveGrid()
    {
        while (true)
        {
            var (newGridX, newGridY) = GetCoordinatesFromPlayer("Enter new coordinates for the grid <x,y>:");

            if (!_gameInstance.MoveGrid(newGridX, newGridY))
            {
                Console.WriteLine("Invalid grid position. Please try again.");
                Console.WriteLine("Press Enter to continue...");
                TicTacTwoBrain.WaitForEnter();
                continue;
            }
            break;
        }
    }
    
    /// <summary>
    /// Prompts the player to enter coordinates.
    /// </summary>
    private (int x, int y) GetCoordinatesFromPlayer(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            
            if (TicTacTwoBrain.TryParseCoordinates(input, out int x, out int y) && _gameInstance.IsWithinBoard(x, y))
            {
                return (x, y);
            }

            Console.WriteLine(
                "Invalid input. Please enter valid coordinates within the board limits.");
        }
    }
}