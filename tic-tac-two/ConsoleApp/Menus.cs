using System.Text.Json;
using DAL;
using Domain;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public class Menus
{
    private readonly IGameRepository _gameRepository;
    private readonly IConfigRepository _configRepository;
    
    public Menus(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }
    
    public void RunMainMenu()
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
            new Option("Load Game", "Load a previously saved game.", LoadGame),
            new Option("Instructions", "View game instructions.", DisplayInstructions),
            new Option("About", "Learn more about the game and its creator.", DisplayAboutInfo),
            new Option("Exit", "Exit the game application.", ExitGame)
        };

        // Create and run the menu with the defined prompt and options
        new Menu(prompt, options).Run();
    }
    
    public void RunNewGameMenu()
    {
        // Prompt for selecting a game configuration
        const string prompt = "To Start a New Game, select the game configuration:";

        // Retrieve configuration names and create corresponding menu options
        var configNames = _configRepository.GetConfigurationNames();
        
        var options = configNames.Select(configName =>
        {
            // If the configuration is "Custom", trigger StartCustomGame
            if (configName == "Custom")
            {
                return new Option(configName, $"Create a custom game configuration.", StartCustomGame);
            }
            else
            {
                // Otherwise, trigger StartGame
                return new Option(configName, $"Start the {configName} game", () => StartGame(_configRepository.GetConfigurationByName(configName)));
            }
        }).ToList();
        
        options.Add(new Option("Return", "Return to the main menu.", RunMainMenu));
        options.Add(new Option("Exit", "Exit the game application.", ExitGame));

        // Create and run the new game menu
        new Menu(prompt, options).Run();
    }

    private void StartGame(GameConfiguration gameConfig)
    {
        Console.WriteLine("If you want Ai to be player then name playerX / playerO or both 'AI'");
        
        var (playerX, playerO) = CustomInput.InputPlayerNames();
        var gameController = new GameController(_gameRepository, _configRepository);
        GameState gameState = new GameState(gameConfig, playerX, playerO);

        gameController.PlayGame(gameConfig, gameState);
    }

    private void StartCustomGame()
    {
        // Küsib kasutajalt kohandatud konfiguratsiooni ja mängijate nimed
        var customConfig = CustomInput.InputCustomConfiguration();
        var (playerX, playerO) = CustomInput.InputPlayerNames();

        // Loo GameController, andes edasi _gameRepository ja _configRepository
        var gameController = new GameController(_gameRepository, _configRepository);

        GameState gameState = new GameState(customConfig, playerX, playerO);
    
        // Alusta uut mängu kohandatud konfiguratsiooniga
        gameController.PlayGame(customConfig, gameState);
    }
    
    private void LoadGame()
    {
        const string prompt = "Select a saved game to load:";
        var savedGameNames = _gameRepository.GetSavedGameNames();

        if (savedGameNames.Count == 0)
        {
            Console.WriteLine("No saved games available to load.");
            RunMainMenu();
            return;
        }

        var options = savedGameNames.Select(gameName =>
            new Option(gameName, $"Load the game '{gameName}'", () => LoadSavedGame(gameName))
        ).ToList();

        options.Add(new Option("Return", "Return to the main menu.", RunMainMenu));
        options.Add(new Option("Exit", "Exit the game application.", ExitGame));

        new Menu(prompt, options).Run();
    }
    
    private void LoadSavedGame(string gameName)
    {
        var savedGameContent = _gameRepository.FindSavedGame(gameName);

        if (string.IsNullOrEmpty(savedGameContent))
        {
            Console.WriteLine($"No saved game found with the name '{gameName}'.");
            return;
        }

        string jsonState;
        
        jsonState = savedGameContent;

        
        var gameConfig = GetConfiguration.LoadGameConfiguration(jsonState);

        var gameState = GetConfiguration.LoadGameState(jsonState, gameConfig);
        

        var gameController = new GameController(_gameRepository, _configRepository);
        gameController.PlayGame(gameConfig, gameState);
    }
    
    private void DisplayInstructions()
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
    
    private void DisplayAboutInfo()
    {
        Console.Clear(); // Clear the console for a fresh display
        Console.WriteLine("This game was created by Marek Mölder.\n");
        Console.WriteLine("Press ENTER to return to the main menu.");
        WaitForEnter(); // Wait for the user to press Enter
        RunMainMenu(); // Return to the main menu
    }
    
    private void ExitGame()
    {
        Console.WriteLine("\nPress Enter to exit..."); 
        WaitForEnter(); // Wait for the user to press Enter before exiting
        Environment.Exit(0); // Exit the application
    }
    
    private void WaitForEnter()
    {
        ConsoleKey keyPressed;
        do
        {
            keyPressed = Console.ReadKey(true).Key; // Read the key pressed without displaying it
        } while (keyPressed != ConsoleKey.Enter); // Loop until Enter is pressed
    }
}
