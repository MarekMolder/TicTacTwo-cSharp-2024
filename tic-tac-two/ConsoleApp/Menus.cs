using DAL;
using GameBrain;
using MenuSystem;

namespace GameController;

public class Menus
{
    public static void RunMainMenu()
    {
        const string prompt = """
                              
                               ______   __     ______     ______   ______     ______     ______   __     __     ______    
                              /\__  _\ /\ \   /\  ___\   /\__  _\ /\  __ \   /\  ___\   /\__  _\ /\ \  _ \ \   /\  __ \   
                              \/_/\ \/ \ \ \  \ \ \____  \/_/\ \/ \ \  __ \  \ \ \____  \/_/\ \/ \ \ \/ .\  \  \ \ \/\ \  
                                 \ \_\  \ \_\  \ \_____\    \ \_\  \ \_\ \_\  \ \_____\    \ \_\  \ \__/.\_ _\  \ \_____\ 
                                  \/_/   \/_/   \/_____/     \/_/   \/_/\/_/   \/_____/     \/_/   \/_/   \/_/   \/_____/ 

                              (Use the arrow keys to cycle through options and press enter to select an option.)

                              """;

        var options = new List<Option>
        {
            new Option("New Game", "Start a new game.", RunNewGameMenu),
            new Option("Load Game", "Load a previously saved game.", CustomInput.LoadGame),
            new Option("Instructions", "View game instructions.", DisplayInstructions),
            new Option("About", "Learn more about the game and its creator.", DisplayAboutInfo),
            new Option("Exit", "Exit the game application.", ExitGame)
        };

        new Menu(prompt, options).Run();
    }

    private static void RunNewGameMenu()
    {
        const string prompt = "To Start a New Game, select the game configuration:";

        var configNames = ConfigRepository.GetConfigurationNames();
        var options = configNames.Select(configName => 
            new Option(configName, $"Start the {configName} game", () => StartGame(ConfigRepository.GetConfigurationByName(configName)))
        ).ToList();

        options.Add(new Option("Custom", "Create a custom game configuration.", StartCustomGame));
        options.Add(new Option("Return", "Return to the main menu.", RunMainMenu));
        options.Add(new Option("Exit", "Exit the game application.", ExitGame));

        new Menu(prompt, options).Run();
    }

    private static void StartGame(GameConfiguration gameConfig)
    {
        var (playerX, playerO) = CustomInput.InputPlayerNames();
        new ConsoleApp.GameController().NewGame(gameConfig, playerX, playerO);
    }

    private static void StartCustomGame()
    {
        var customConfig = CustomInput.InputCustomConfiguration();
        var (playerX, playerO) = CustomInput.InputPlayerNames();
        new ConsoleApp.GameController().NewGame(customConfig, playerX, playerO);
    }

    private static void DisplayInstructions()
    {
        Console.Clear();
        Console.WriteLine("=== Tic-Tac-Two Instructions ===");
        Console.WriteLine("1. The game is played on a X*Y grid.");
        Console.WriteLine("2. Player 1 is 'X', Player 2 is 'O'.");
        Console.WriteLine("3. Players take turns selecting a number (1-9) to place their mark.");
        Console.WriteLine("4. The goal is to get two of your marks in a row, either horizontally, vertically, or diagonally.");
        Console.WriteLine("5. If all spots are filled without a winner, the game is a draw.");
        Console.WriteLine("\nPress ENTER to return to the menu...");
        WaitForEnter();
        RunMainMenu();
    }

    private static void DisplayAboutInfo()
    {
        Console.Clear();
        Console.WriteLine("This game was created by Marek Mölder.\n");
        Console.WriteLine("Press ENTER to return to the main menu.");
        WaitForEnter();
        RunMainMenu();
    }
    
    /// <summary>
    /// Exits the game application.
    /// </summary>
    private static void ExitGame()
    {
        Console.WriteLine("\nPress Enter to exit..."); 
        WaitForEnter(); 
        Environment.Exit(0);
    }
    
    /// <summary>
    /// Waits for the user to press the Enter key.
    /// </summary>
    private static void WaitForEnter()
    {
        ConsoleKey keyPressed;
        do
        {
            keyPressed = Console.ReadKey(true).Key;
        } while (keyPressed != ConsoleKey.Enter);
    }
}
