using DAL;
using Domain;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public class Menus(IConfigRepository configRepository, IGameRepository gameRepository)
{
    /// <summary>
    /// Runs the main menu.
    /// </summary>
    public void RunMainMenu()
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
            new Option("Load Game", "Load a previously saved game.", LoadGame),
            new Option("Instructions", "View game instructions.", DisplayInstructions),
            new Option("About", "Learn more about the game and its creator.", DisplayAboutInfo),
            new Option("Exit", "Exit the game application.", ExitGame)
        };
        
        new Menu(prompt, options).Run();
    }

    /// <summary>
    /// Runs the new game menu.
    /// </summary>
    private void RunNewGameMenu()
    {
        const string prompt = "To Start a New Game, select the game configuration:";
        
        var configNames = configRepository.GetConfigurationNames();
        
        var options = configNames.Select(configName =>
        {
            if (configName == "Custom")
            {
                return new Option(configName, $"Create a custom game configuration.",() => StartGame());
            }
            else
            {
                return new Option(configName, $"Start the {configName} game", () => StartGame(configRepository.GetConfigurationByName(configName)));
            }
        }).ToList();
        
        options.Add(new Option("Return", "Return to the main menu.", RunMainMenu));
        options.Add(new Option("Exit", "Exit the game application.", ExitGame));
        
        new Menu(prompt, options).Run();
    }

    /// <summary>
    /// Starts a new game with the specified configuration or a custom configuration.
    /// </summary>
    private void StartGame(GameConfiguration? gameConfig = null)
    {
        gameConfig ??= CustomInput.InputCustomConfiguration();
        
        Console.WriteLine("If you want Ai to be player then name playerX / playerO or both 'AI'");
        
        var (playerX, playerO) = CustomInput.InputPlayerNames();
        var gameController = new GameController(gameRepository);
        
        GameState gameState = new GameState(gameConfig, playerX, playerO);
        
        gameController.PlayGame(gameConfig, gameState);
    }
    
    /// <summary>
    /// Loads a saved game.
    /// </summary>
    private void LoadGame()
    {
        const string prompt = "Select a saved game to load:";
        var savedGameNames = gameRepository.GetSavedGameNames();

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
    
    /// <summary>
    /// Loads the specified saved game.
    /// </summary>
    private void LoadSavedGame(string gameName)
    {
        var savedGameContent = gameRepository.FindSavedGame(gameName);

        if (string.IsNullOrEmpty(savedGameContent))
        {
            Console.WriteLine($"No saved game found with the name '{gameName}'.");
            return;
        }

        var jsonState = savedGameContent;
        
        var gameConfig = GetConfiguration.LoadGameConfiguration(jsonState);

        var gameState = GetConfiguration.LoadGameState(jsonState, gameConfig);
        

        var gameController = new GameController(gameRepository);
        gameController.PlayGame(gameConfig, gameState);
    }
    
    /// <summary>
    /// Displays the game instructions.
    /// </summary>
    private void DisplayInstructions()
    {
        Console.Clear();
        Console.WriteLine("=== Tic-Tac-Two Instructions ===");
        Console.WriteLine("https://www.geekyhobbies.com/tic-tac-two-board-game-review/");
        Console.WriteLine("Press ENTER to return to the main menu.");
        TicTacTwoBrain.WaitForEnter();
        RunMainMenu();
    }
    
    /// <summary>
    /// Displays information about the game and its creator.
    /// </summary>
    private void DisplayAboutInfo()
    {
        Console.Clear();
        Console.WriteLine("This game was created by Marek Mölder.\n");
        Console.WriteLine("Press ENTER to return to the main menu.");
        TicTacTwoBrain.WaitForEnter();
        RunMainMenu();
    }
    
    /// <summary>
    /// Exits the game application.
    /// </summary>
    private void ExitGame()
    {
        Console.WriteLine("\nPress Enter to exit..."); 
        TicTacTwoBrain.WaitForEnter();
        Environment.Exit(0);
    }
    
}
