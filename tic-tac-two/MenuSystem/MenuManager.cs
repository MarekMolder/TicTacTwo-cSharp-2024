using System.Runtime.InteropServices;
using DAL;
using GameBrain;
using static System.Console;

namespace MenuSystem;

/// <summary>
/// Manages the menu operations, including displaying the main menu.
/// </summary>
public class MenuManager
{
    /// <summary>
    /// Displays the main menu and handles user selection.
    /// </summary>
    public void RunMainMenu()
    {
        string prompt = @"
 ______   __     ______     ______   ______     ______     ______   __     __     ______    
/\__  _\ /\ \   /\  ___\   /\__  _\ /\  __ \   /\  ___\   /\__  _\ /\ \  _ \ \   /\  __ \   
\/_/\ \/ \ \ \  \ \ \____  \/_/\ \/ \ \  __ \  \ \ \____  \/_/\ \/ \ \ \/ .\  \  \ \ \/\ \  
   \ \_\  \ \_\  \ \_____\    \ \_\  \ \_\ \_\  \ \_____\    \ \_\  \ \__/.\_ _\  \ \_____\ 
    \/_/   \/_/   \/_____/     \/_/   \/_/\/_/   \/_____/     \/_/   \/_/   \/_/   \/_____/ 

(Use the arrow keys to cycle through options and press enter to select an option.)
";

        // Define the available options for the main menu
        var options = new List<Option>
        {
            new Option("New Game", "Start a new game.", () => RunNewGameMenu()),
            new Option("Load Game", "Load a previously saved game.", () => LoadGame()),
            new Option("Instructions", "View game instructions.", DisplayInstructions),
            new Option("About", "Learn more about the game and its creator.", DisplayAboutInfo),
            new Option("Exit", "Exit the game application.", ExitGame)
        };

        Menu mainMenu = new Menu(prompt, options); // Create a new Menu instance
        mainMenu.Run(); // Run the main menu
    }
    
    public void RunNewGameMenu()
    {
        string prompt = """
                        
                        To Start a New Game, select the game configuration:
                        """;

        // Fetch configuration names dynamically
        var configNames = ConfigRepository.GetConfigurationNames();

        var options = new List<Option>();
        var gameController = new GameController(); // Create instance of GameController

        // Iterate through configurations and create menu options for each one
        foreach (var configName in configNames)
        {
            var gameConfig = ConfigRepository.GetConfigurationByName(configName);
            options.Add(new Option(configName, $"Start the {configName} game", 
                () => StartGame(gameConfig))); // Pass GameConfiguration object
        }

        // Add custom option
        options.Add(new Option("Custom", "Create a custom game configuration.", StartCustomGame)); 

        // Add options to return to the main menu or exit the game
        options.Add(new Option("Return", "Return to the main menu.", RunMainMenu)); 
        options.Add(new Option("Exit", "Exit the game application.", ExitGame));

        Menu newGameMenu = new Menu(prompt, options); // Create new menu
        newGameMenu.Run(); // Run the menu
    }
    
    private void StartGame(GameConfiguration gameConfig)
    {
        // Input player names
        var (playerX, playerO) = InputPlayerNames();

        var gameController = new GameController();
        gameController.NewGame(gameConfig, playerX, playerO);
    }
    
    private (string playerX, string playerO) InputPlayerNames()
    {
        string playerX;
        string playerO;

        // Get Player X name
        while (true)
        {
            Console.Write("Enter name for Player X: ");
            playerX = Console.ReadLine()!;
        
            if (!string.IsNullOrWhiteSpace(playerX)) // Check for non-empty input
                break;

            Console.WriteLine("Player X name must be at least 1 character long. Please try again.");
        }

        // Get Player O name
        while (true)
        {
            Console.Write("Enter name for Player O: ");
            playerO = Console.ReadLine()!;

            if (!string.IsNullOrWhiteSpace(playerO) && playerO != playerX) // Check for non-empty input and not same as Player X
                break;

            if (string.IsNullOrWhiteSpace(playerO))
            {
                Console.WriteLine("Player O name must be at least 1 character long. Please try again.");
            }
            else
            {
                Console.WriteLine("Player O name must be different from Player X. Please try again.");
            }
        }
    
        return (playerX, playerO);
    }
    
    private void StartCustomGame()
    {
        // Prompt for custom configuration parameters
        var customConfig = InputCustomConfiguration();

        // Input player names
        var (playerX, playerO) = InputPlayerNames();

        var gameController = new GameController();
        gameController.NewGame(customConfig, playerX, playerO);
    }

    private GameConfiguration InputCustomConfiguration()
    {
        int boardWidth = GetValidatedInput("Enter board width (min 2, max 20): ", 2, 20);
        int boardHeight = GetValidatedInput("Enter board height (min 2, max 20): ", 2, 20);
        int piecesNumber = GetValidatedInput("Enter pieces number (min 2, max 100): ", 2, 100);
        int winCondition = GetValidatedInput("Enter win condition (min 2, max 10): ", 2, 10);
    
        // Get grid dimensions with additional validation
        int gridWidth;
        int gridHeight;
    
        do
        {
            gridWidth = GetValidatedInput($"Enter grid width (min 2, max {boardWidth}): ", 2, boardWidth);
            gridHeight = GetValidatedInput($"Enter grid height (min 2, max {boardHeight}): ", 2, boardHeight);
        
            if (gridWidth > boardWidth || gridHeight > boardHeight)
            {
                Console.WriteLine($"Grid dimensions must not exceed board dimensions. " +
                                  $"Please ensure grid width ≤ {boardWidth} and grid height ≤ {boardHeight}.");
            }
        }
        while (gridWidth > boardWidth || gridHeight > boardHeight);

        int movePieceAfterNMove = GetValidatedInput("After number of ... steps, you can move pieces (if 0 then cant move pieces): ", 1, null); // No upper limit
        int moveGridAfterNMove = GetValidatedInput("After number of ... steps, you can move grid (if 0 then cant move grid): ", 0, null); // No upper limit
        
        


        return new GameConfiguration
        {
            Name = "Custom",
            BoardSizeWidth = boardWidth,
            BoardSizeHeight = boardHeight,
            PiecesNumber = piecesNumber,
            GridSizeWidth = gridWidth,
            GridSizeHeight = gridHeight,
            WinCondition = winCondition,
            MovePieceAfterNMove = movePieceAfterNMove,
            MoveGridAfterNMove = moveGridAfterNMove,
            
        };
    }
    
    private int GetValidatedInput(string prompt, int minValue, int? maxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= minValue && (maxValue == null || value <= maxValue))
            {
                return value;
            }
            else
            {
                if (maxValue == null)
                {
                    Console.WriteLine($"Invalid input. Please enter a number greater than or equal to {minValue}.");
                }
                else
                {
                    Console.WriteLine($"Invalid input. Please enter a number between {minValue} and {maxValue}.");
                }
            }
        }
    }

    /// <summary>
    /// Displays the game instructions to the user.
    /// </summary>
    private void DisplayInstructions()
    {
        Clear(); // Clear the console
        WriteLine("=== Tic-Tac-Two Instructions ===");
        WriteLine("1. The game is played on a X*Y grid.");
        WriteLine("2. Player 1 is 'X', Player 2 is 'O'.");
        WriteLine("3. Players take turns selecting a number (1-9) to place their mark.");
        WriteLine("4. The goal is to get **two** of your marks in a row, either horizontally, vertically, or diagonally.");
        WriteLine("5. If all spots are filled without a winner, the game is a draw.");
        WriteLine("");
        WriteLine("Press ENTER to return to the menu...");
        WaitForEnter(); // Wait for the user to press Enter
        RunMainMenu(); // Return to the main menu
    }

    /// <summary>
    /// Displays information about the game creator.
    /// </summary>
    private void DisplayAboutInfo()
    {
        Clear(); // Clear the console
        WriteLine("This game was created by Marek Mölder.");
        WriteLine("");
        WriteLine("Press ENTER to return to the main menu.");
        WaitForEnter(); // Wait for the user to press Enter
        RunMainMenu(); // Return to the main menu
    }

    /// <summary>
    /// Exits the game application.
    /// </summary>
    private void ExitGame()
    {
        WriteLine("\nPress Enter to exit..."); // Prompt user to exit
        WaitForEnter(); // Wait for the user to press Enter
        Environment.Exit(0); // Terminate the application
    }

    /// <summary>
    /// Waits for the user to press the Enter key.
    /// </summary>
    private void WaitForEnter()
    {
        ConsoleKey keyPressed;
        // Keep waiting until Enter is pressed
        do
        {
            keyPressed = Console.ReadKey(true).Key;
        } while (keyPressed != ConsoleKey.Enter);
    }

    /// <summary>
    /// Loads a previously saved game. (Implement this method as needed.)
    /// </summary>
    private void LoadGame()
    {
        // Implement load game logic here
    }
}