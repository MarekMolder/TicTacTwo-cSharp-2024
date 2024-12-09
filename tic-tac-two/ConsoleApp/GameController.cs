using ConsoleUI;
using DAL;
using Domain;
using GameBrain;

namespace ConsoleApp;

public class GameController
{
    private readonly IGameRepository _gameRepository;
    private readonly IConfigRepository _configRepository;
    TicTacTwoBrain gameInstance;
    
    public GameController(IGameRepository gameRepository, IConfigRepository configRepository)
    {
        _gameRepository = gameRepository;
        _configRepository = configRepository;
    }
    
    public void PlayGame(GameConfiguration chosenConfig, string playerX, string playerO, EGamePiece[][] gameBoard = null, EGamePiece currentPlayer = EGamePiece.X, int piecesLeftX = 0, int piecesLeftO = 0, int movesMadeX = 0, int movesMadeO = 0, int gridPositionX = 0, int gridPositionY = 0)
    {
        
        if (gameBoard == null)
        {
            // Kui mängulauad pole salvestatud, alustame uut mängu
            gameInstance = new TicTacTwoBrain(chosenConfig, playerX, playerO);
        }
        else
        {
            // Kui mängulaua olek on olemas, laadime vana mängu
            gameInstance = new TicTacTwoBrain(chosenConfig, playerX, playerO, gridPositionX, gridPositionY, movesMadeX, movesMadeO);
            gameInstance.SetGameBoard(gameBoard);
            gameInstance.CurrentPlayer = currentPlayer;
            gameInstance.PiecesLeftX = piecesLeftX;
            gameInstance.PiecesLeftO = piecesLeftO;
            gameInstance.MovesMadeX = movesMadeX;
            gameInstance.MovesMadeO = movesMadeO;
        }

        var currentPlayerName = currentPlayer == EGamePiece.X ? playerX : playerO;

    do
    {
        DisplayBoard(gameInstance, currentPlayerName, playerX, playerO);

        var canMovePiece = gameInstance.CanMovePiece();
        var canMoveGrid = gameInstance.CanMoveGrid();
        var hasPiecesLeft = gameInstance.HasPiecesLeft();

        string promptMessage;
        List<string> options = new List<string>();

        // Määrame, millised valikud on saadaval sõltuvalt tingimustest
        if (!hasPiecesLeft)
        {
            promptMessage = "You have no pieces left.";
            if (canMovePiece) options.Add("old");
            if (canMoveGrid) options.Add("grid");
            options.Add("save");
            options.Add("exit");
        }
        else
        {
            promptMessage = $"Do you want to place a new piece{(canMovePiece ? ", move an existing piece" : "")}{(canMoveGrid ? ", move the grid" : "")}, save the game or exit the game?";
            if (canMovePiece) options.Add("old");
            if (canMoveGrid) options.Add("grid");
            options.Add("new");
            options.Add("save");
            options.Add("exit");
        }

        // Kuvame kasutajale valikud
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
                _gameRepository.Savegame(gameInstance.GetGameStateJson(), chosenConfig);
                break;
            }

            if (response == "exit")
            {
                Program.Main();
            }

            Console.WriteLine("Invalid input. Please try again.");
        }

        if (CheckEndGameConditions(gameInstance, playerX, playerO))
        {
            break;
        }

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
    
    public void PlaceNewPiece()
    {
        while (true)
        {
            // Küsi mängijalt koordinaadid, kus ta tahab oma tüki paigutada
            var (x, y) = GetCoordinatesFromPlayer("Enter the coordinates where you want to place your piece <x,y>:");

            // Kontrollige, kas tükk saab õigesti paigutatud
            if (!gameInstance.PlaceNewPiece(x, y))
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue(); // Pause, et mängija saaks sõnumit lugeda
                continue; // Küsi koordinaate uuesti
            }
            
            break;
        }
    }

    public void MoveExistingPiece()
    {
        while (true)
        {
            // Get the coordinates of the piece the player wants to move
            var (oldX, oldY) = GetCoordinatesFromPlayer("Enter the coordinates of the piece you want to move <x,y>:");
        
            // Get the new coordinates where the player wants to move the piece
            var (newX, newY) =
                GetCoordinatesFromPlayer("Enter the new coordinates where you want to move the piece <x,y>:");

            if (!gameInstance.MoveExistingPiece(oldX, oldY , newX, newY))
            {
                Console.WriteLine("Invalid move. That is not your piece or the spot is already occupied.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                continue;
            }

            break;
        }
    }

    public void MoveGrid()
    {
        while (true)
        {
            // Get new coordinates for the grid from the player
            var (newGridX, newGridY) = GetCoordinatesFromPlayer("Enter new coordinates for the grid <x,y>:");

            if (!gameInstance.MoveGrid(newGridX, newGridY))
            {
                Console.WriteLine("Invalid grid position. Please try again.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                continue;
            }
            break;
        }
    }
    
    
   /// <summary>
/// Prompts the player to enter coordinates for a move and validates them.
/// Continues prompting the user until valid coordinates are entered.
/// </summary>
/// <param name="prompt">The prompt message displayed to the player.</param>
/// <returns>
/// A tuple representing the coordinates (x, y) entered by the player.
/// </returns>
public (int x, int y) GetCoordinatesFromPlayer(string prompt)
{
    while (true)
    {
        Console.WriteLine(prompt); // Display the prompt to the user
        var input = Console.ReadLine(); // Read user input

        // Attempt to parse the input into coordinates and check if they are within board limits
        if (TryParseCoordinates(input, out int x, out int y) && gameInstance.IsWithinBoard(x, y))
        {
            return (x, y); // Return the valid coordinates
        }

        Console.WriteLine(
            "Invalid input. Please enter valid coordinates within the board limits."); // Error message for invalid input
    }
}

/// <summary>
/// Attempts to parse the player's input into two integer coordinates (x, y).
/// Supports both numeric and letter input for coordinates.
/// </summary>
/// <param name="input">The raw input string from the player.</param>
/// <param name="x">The parsed x-coordinate.</param>
/// <param name="y">The parsed y-coordinate.</param>
/// <returns>
/// <c>true</c> if the input can be parsed successfully into two integers; otherwise, <c>false</c>.
/// </returns>
private bool TryParseCoordinates(string? input, out int x, out int y)
{
    x = y = -1; // Initialize output coordinates
    var parts = input?.Split(','); // Split input by comma

    if (parts != null && parts.Length == 2)
    {
        // Try parsing the first part (x-coordinate)
        if (!TryParseCoordinate(parts[0], out x)) return false;
        
        // Try parsing the second part (y-coordinate)
        if (!TryParseCoordinate(parts[1], out y)) return false;

        return true;
    }

    return false;
}

/// <summary>
/// Tries to parse a coordinate (either a number or a letter) into an integer.
/// </summary>
/// <param name="coordinate">The raw coordinate (either a number or a letter).</param>
/// <param name="parsedCoordinate">The parsed integer coordinate.</param>
/// <returns>
/// <c>true</c> if the coordinate can be parsed successfully; otherwise, <c>false</c>.
/// </returns>
private bool TryParseCoordinate(string coordinate, out int parsedCoordinate)
{
    parsedCoordinate = -1;

    // Check if the coordinate is a number
    if (int.TryParse(coordinate, out parsedCoordinate))
    {
        return true;
    }

    // Check if the coordinate is a letter (A-Z or a-z)
    if (coordinate.Length == 1 && char.IsLetter(coordinate[0]))
    {
        // Convert letter to a number (A = 10, B = 11, ..., Z = 35)
        parsedCoordinate = char.ToUpper(coordinate[0]) - 'A' + 10;
        return true;
    }

    return false;
}
    
    /// <summary>
    /// Pauses the game and waits for the player to press any key to continue.
    /// Clears the console after a key is pressed.
    /// </summary>
    private void PauseBeforeContinue()
    {
        Console.WriteLine("Press any key to continue..."); // Prompt the user
        Console.ReadKey(); // Wait for a key press
    }
}