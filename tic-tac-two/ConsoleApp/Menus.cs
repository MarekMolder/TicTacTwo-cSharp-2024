using DAL;
using GameBrain;
using MenuSystem;

namespace GameController;

/// <summary>
/// The Menus class handles the game's user interface, managing menu displays and user interactions.
/// </summary>
public class Menus
{
    /// <summary>
    /// Displays the main menu of the game and allows the user to select an option.
    /// </summary>
    public static void RunMainMenu()
    {
        // Main menu prompt with ASCII art
        const string prompt = """
                              
                               ______   __     ______     ______   ______     ______     ______   __     __     ______    
                              /\__  _\ /\ \   /\  ___\   /\__  _\ /\  __ \   /\  ___\   /\__  _\ /\ \  _ \ \   /\  __ \   
                              \/_/\ \/ \ \ \  \ \ \____  \/_/\ \/ \ \  __ \  \ \ \____  \/_/\ \/ \ \ \/ .\  \  \ \ \/\ \  
                                 \ \_\  \ \_\  \ \_____\    \ \_\  \ \_\ \_\  \ \_____\    \ \_\  \ \__/.\_ _\  \ \_____\ 
                                  \/_/   \/_/   \/_____/     \/_/   \/_/\/_/   \/_____/     \/_/   \/_/   \/_/   \/_____/ 

                              (Use the arrow keys to cycle through options and press enter to select an option.)

                              """;

        // List of menu options available to the user
        var options = new List<Option>
        {
            new Option("New Game", "Start a new game.", RunNewGameMenu),
            new Option("Load Game", "Load a previously saved game.", CustomInput.LoadGame),
            new Option("Instructions", "View game instructions.", DisplayInstructions),
            new Option("About", "Learn more about the game and its creator.", DisplayAboutInfo),
            new Option("Exit", "Exit the game application.", ExitGame)
        };

        // Create and run the menu with the defined prompt and options
        new Menu(prompt, options).Run();
    }

    /// <summary>
    /// Displays the new game menu where the user can select the game configuration.
    /// </summary>
    private static void RunNewGameMenu()
    {
        // Prompt for selecting a game configuration
        const string prompt = "To Start a New Game, select the game configuration:";

        // Retrieve configuration names and create corresponding menu options
        var configNames = ConfigRepository.GetConfigurationNames();
        var options = configNames.Select(configName => 
            new Option(configName, $"Start the {configName} game", () => StartGame(ConfigRepository.GetConfigurationByName(configName)))
        ).ToList();

        // Add options for custom configuration and returning to the main menu
        options.Add(new Option("Custom", "Create a custom game configuration.", StartCustomGame));
        options.Add(new Option("Return", "Return to the main menu.", RunMainMenu));
        options.Add(new Option("Exit", "Exit the game application.", ExitGame));

        // Create and run the new game menu
        new Menu(prompt, options).Run();
    }

    /// <summary>
    /// Starts the game with the selected game configuration.
    /// </summary>
    /// <param name="gameConfig">The game configuration to use for the new game.</param>
    private static void StartGame(GameConfiguration gameConfig)
    {
        // Prompt the user to input player names and start the game
        var (playerX, playerO) = CustomInput.InputPlayerNames();
        new ConsoleApp.GameController().NewGame(gameConfig, playerX, playerO);
    }

    /// <summary>
    /// Starts a custom game configuration.
    /// </summary>
    private static void StartCustomGame()
    {
        // Get the custom configuration and player names, then start the game
        var customConfig = CustomInput.InputCustomConfiguration();
        var (playerX, playerO) = CustomInput.InputPlayerNames();
        new ConsoleApp.GameController().NewGame(customConfig, playerX, playerO);
    }

    /// <summary>
    /// Displays the game instructions to the user.
    /// </summary>
    private static void DisplayInstructions()
    {
        Console.Clear(); // Clear the console for a fresh display
        Console.WriteLine("=== Tic-Tac-Two Instructions ===");
        Console.WriteLine("1. The game is played on a X*Y grid.");
        Console.WriteLine("2. Player 1 is 'X', Player 2 is 'O'.");
        Console.WriteLine("3. Players take turns selecting a number (1-9) to place their mark.");
        Console.WriteLine("4. The goal is to get two of your marks in a row, either horizontally, vertically, or diagonally.");
        Console.WriteLine("5. If all spots are filled without a winner, the game is a draw.");
        Console.WriteLine("\nPress ENTER to return to the menu...");
        WaitForEnter(); // Wait for the user to press Enter
        RunMainMenu(); // Return to the main menu
    }

    /// <summary>
    /// Displays information about the game and its creator.
    /// </summary>
    private static void DisplayAboutInfo()
    {
        Console.Clear(); // Clear the console for a fresh display
        Console.WriteLine("This game was created by Marek Mölder.\n");
        Console.WriteLine("Press ENTER to return to the main menu.");
        WaitForEnter(); // Wait for the user to press Enter
        RunMainMenu(); // Return to the main menu
    }
    
    /// <summary>
    /// Exits the game application.
    /// </summary>
    private static void ExitGame()
    {
        Console.WriteLine("\nPress Enter to exit..."); 
        WaitForEnter(); // Wait for the user to press Enter before exiting
        Environment.Exit(0); // Exit the application
    }
    
    /// <summary>
    /// Waits for the user to press the Enter key.
    /// </summary>
    private static void WaitForEnter()
    {
        ConsoleKey keyPressed;
        do
        {
            keyPressed = Console.ReadKey(true).Key; // Read the key pressed without displaying it
        } while (keyPressed != ConsoleKey.Enter); // Loop until Enter is pressed
    }
}
