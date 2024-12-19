using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApp.Pages.PlayGame;

public class Index(IConfigRepository configRepository, IGameRepository gameRepository)
    : PageModel
{
    [BindProperty] public Domain.GameConfiguration GameConfig { get; set; } = null!;

    [BindProperty(SupportsGet = true)] public string ConfigName { get; set; } = default!;

    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;
    
    public GameState GameState { get; set; } = default!;

    [BindProperty] public string SelectedAction { get; set; } = default!;

    [BindProperty] public int CoordinateX { get; set; }

    [BindProperty] public int CoordinateY { get; set; }

    [BindProperty] public int OldCoordinateX { get; set; }

    [BindProperty] public int OldCoordinateY { get; set; }

    public List<string> ActionSelectList { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string GameName { get; set; } = null!;

    [BindProperty] public bool IsGameOver { get; set; } = false;

    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)] public string PlayerXorO{ get; set; } = default!;
    [BindProperty(SupportsGet = true)] public string NumberOfAIs{ get; set; } = default!;
    

    public IActionResult OnGet()
    {
        if (!string.IsNullOrEmpty(GameName))
        {
            LoadExistingGame();
            GameState = JsonSerializer.Deserialize<GameState>(TicTacTwoBrain.GetGameStateJson())!;
                
            TempData["PlayerX"] = GameState.PlayerX;
            TempData["PlayerO"] = GameState.PlayerO;
            
            HandleAiMove();
            
        }
        else if (!string.IsNullOrEmpty(ConfigName))
        {
            StartNewGame();
            return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName, numberOfAIs = NumberOfAIs});
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (SelectedAction == "exit")
        {
            return RedirectToPage("/NewGame/NewGame", new {userName = UserName});
        }
        
        if (!string.IsNullOrEmpty(GameName))
        {
            LoadExistingGame();
            if (!IsGameOver)
            {
                GameState = JsonSerializer.Deserialize<GameState>(TicTacTwoBrain.GetGameStateJson())!;
                
                TempData["PlayerX"] = GameState.PlayerX;
                TempData["PlayerO"] = GameState.PlayerO;
                
                HandlePlayerMove();

                return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName});
            }
        }
        else if (!string.IsNullOrEmpty(ConfigName))
        {
            StartNewGame();
            return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName});
        }

        return Page();
    }
    
    private void StartNewGame()
    {
        var gameConfig = configRepository.GetConfigurationByName(ConfigName);

        if (NumberOfAIs == "0")
        {
            if (PlayerXorO == "O")
            {
                TicTacTwoBrain = new TicTacTwoBrain(gameConfig, playerO:UserName);
                GameName = gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig(), playerO:UserName);
            }
            else
            {
                TicTacTwoBrain = new TicTacTwoBrain(gameConfig, playerX:UserName);
                GameName = gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig(), playerX:UserName);
            }
        } else if (NumberOfAIs == "1")
        {
            if (PlayerXorO == "O")
            {
                TicTacTwoBrain = new TicTacTwoBrain(gameConfig, playerO:UserName, playerX:"AI");
                GameName = gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig(), playerO:UserName, playerX:"AI");
            }
            else
            {
                TicTacTwoBrain = new TicTacTwoBrain(gameConfig, playerX:UserName, playerO:"AI");
                GameName = gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig(), playerX:UserName, playerO:"AI");
            }
        } else if (NumberOfAIs == "2")
        {
            TicTacTwoBrain = new TicTacTwoBrain(gameConfig, playerO:"AI", playerX:"AI");
            GameName = gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig(), playerX:"AI", playerO:"AI");
        }
    }
    
    private void LoadExistingGame()
    {
        var state = gameRepository.FindSavedGame(GameName) ?? throw new Exception("Game state not found or is empty.");;
        
        GameState savedGame = JsonConvert.DeserializeObject<GameState>(state)!;
        
        if (savedGame.PlayerO == "Player-0" && savedGame.PlayerX != UserName)
        { 
            savedGame.PlayerO = UserName;
        } else if (savedGame.PlayerX == "Player-X" && savedGame.PlayerO != UserName)
        { 
            savedGame.PlayerX = UserName;
        }

        
        var config = GetConfiguration.LoadGameConfiguration(state);

        TicTacTwoBrain = new TicTacTwoBrain(config);
        TicTacTwoBrain.SetGameStateJson(JsonConvert.SerializeObject(savedGame));
        
        GameName = gameRepository.UpdateGame(TicTacTwoBrain.GetGameStateJson(), GameName, TicTacTwoBrain.GetGameConfig(), UserName);
        UpdateActionSelectList();
        CheckGameOver();
    }
    
    private void CheckGameOver()
    {
        var winner = TicTacTwoBrain.CheckWin();
        if (winner != null)
        {
            var gameState = JsonSerializer.Deserialize<GameState>(TicTacTwoBrain.GetGameStateJson());
            
            string winnerName = winner == EGamePiece.X ? gameState!.PlayerX : gameState!.PlayerO;
            string message = $"{winnerName} wins!"; 
            ViewData["Message"] = message;
            IsGameOver = true;
        }
        else if (TicTacTwoBrain.CheckDraw())
        {
            string message = "It's a draw!"; 
            ViewData["Message"] = message;
            IsGameOver = true;
        }


    }
    
    private void HandleAiMove()
    {
        if (!IsGameOver && 
            ((TicTacTwoBrain.CurrentPlayer == EGamePiece.X && GameState.PlayerX == "AI") ||
             (TicTacTwoBrain.CurrentPlayer == EGamePiece.O && GameState.PlayerO == "AI")))
        {
            TicTacTwoBrain.AiMove();
            GameName = gameRepository.UpdateGame(TicTacTwoBrain.GetGameStateJson(), GameName, TicTacTwoBrain.GetGameConfig(), "AI");
            CheckGameOver();
        }
    }
    
    private void HandlePlayerMove()
    {
        if ((GameState.PlayerX == UserName || GameState.PlayerO == UserName) && 
            ((TicTacTwoBrain.CurrentPlayer == EGamePiece.X && GameState.PlayerX == UserName) ||
             (TicTacTwoBrain.CurrentPlayer == EGamePiece.O && GameState.PlayerO == UserName)))
        {
            if (!IsGameOver)
            {
                PerformAction();
            }
        }
        else
        {
            TempData["ErrorMessage"] = "It's not your turn!";
        }
    }

    private void PerformAction()
    {
        switch (SelectedAction)
        {
            case "PlaceNewButton" when TicTacTwoBrain.HasPiecesLeft():
                TicTacTwoBrain.PlaceNewPiece(CoordinateX, CoordinateY);
                break;
            case "MoveOldButton" when TicTacTwoBrain.CanMovePiece():
                TicTacTwoBrain.MoveExistingPiece(OldCoordinateX, OldCoordinateY, CoordinateX, CoordinateY);
                break;
            case "MoveGrid" when TicTacTwoBrain.CanMoveGrid():
                TicTacTwoBrain.MoveGrid(CoordinateX, CoordinateY);
                break;
            default:
                return;
        }
        GameName = gameRepository.UpdateGame(TicTacTwoBrain.GetGameStateJson(), GameName, TicTacTwoBrain.GetGameConfig(), UserName);
        CheckGameOver();
    }

    private void UpdateActionSelectList()
    {
        var actions = new List<string>();

        if (TicTacTwoBrain.HasPiecesLeft())
        {
            actions.Add("PlaceNewButton");
        }

        if (TicTacTwoBrain.CanMovePiece())
        {
            actions.Add("MoveOldButton");
        }

        if (TicTacTwoBrain.CanMoveGrid())
        {
            actions.Add("MoveGrid");
        }

        ActionSelectList = actions;
    }
}