using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApp.Pages;

public class Index : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public Index(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    [BindProperty] public Domain.GameConfiguration GameConfig { get; set; }

    [BindProperty(SupportsGet = true)] public string ConfigName { get; set; } = default!;

    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;
    
    public GameState GameState { get; set; } = default!;

    [BindProperty] public string SelectedAction { get; set; } = default!;

    [BindProperty] public int CoordinateX { get; set; }

    [BindProperty] public int CoordinateY { get; set; }

    [BindProperty] public int OldCoordinateX { get; set; }

    [BindProperty] public int OldCoordinateY { get; set; }

    public List<string> ActionSelectList { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public int GameId { get; set; }

    [BindProperty(SupportsGet = true)] public string GameName { get; set; }

    [BindProperty] public bool IsGameOver { get; set; } = false;

    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;
    

    public IActionResult OnGet()
    {
        if (!string.IsNullOrEmpty(GameName))
        {
            LoadExistingGame();
            GameState = JsonSerializer.Deserialize<GameState>(TicTacTwoBrain.GetGameStateJson());
                
            TempData["PlayerX"] = GameState.PlayerX;
            TempData["PlayerO"] = GameState.PlayerO;
        }
        else if (!string.IsNullOrEmpty(ConfigName))
        {
            StartNewGame();
            return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName });
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!string.IsNullOrEmpty(GameName))
        {
            LoadExistingGame();
            if (!IsGameOver)
            {
                GameState = JsonSerializer.Deserialize<GameState>(TicTacTwoBrain.GetGameStateJson());
                
                TempData["PlayerX"] = GameState.PlayerX;
                TempData["PlayerO"] = GameState.PlayerO;

                if (GameState.PlayerX == UserName || GameState.PlayerO == UserName)
                {
                    if ((TicTacTwoBrain.CurrentPlayer == EGamePiece.X && GameState.PlayerX == UserName) || 
                        (TicTacTwoBrain.CurrentPlayer == EGamePiece.O && GameState.PlayerO == UserName))
                    {
                        PerformAction();
                    } 
                    else
                    {
                        TempData["ErrorMessage"] = "It's not your turn!";
                    }
                }
                return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName });
            }
        }
        else if (!string.IsNullOrEmpty(ConfigName))
        {
            StartNewGame();
            return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName });
        }

        return Page();
    }


    private void LoadExistingGame()
    {
        var state = _gameRepository.FindSavedGame(GameName);

        if (string.IsNullOrEmpty(state))
        {
            // Handle the error gracefully, maybe return a message or redirect
            throw new Exception("Game state not found or is empty.");
        }
        
        GameState savedGame = JsonConvert.DeserializeObject<GameState>(state);

        if (savedGame.PlayerX != UserName && savedGame.PlayerO == "Player-0")
        {
            savedGame.PlayerO = UserName;
        }
        
        var config = GetConfiguration.LoadGameConfiguration(state);

        TicTacTwoBrain = new TicTacTwoBrain(config);
        
        TicTacTwoBrain.SetGameStateJson(JsonConvert.SerializeObject(savedGame));
        
        GameName = _gameRepository.UpdateGame(TicTacTwoBrain.GetGameStateJson(), GameName, TicTacTwoBrain.GetGameConfig(), UserName);
        
        UpdateActionSelectList();
        CheckGameOver();
    }

    private void StartNewGame()
    {
        var gameConfig = _configRepository.GetConfigurationByName(ConfigName);
        TicTacTwoBrain = new TicTacTwoBrain(gameConfig, playerX:UserName);
        GameName = _gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig(), UserName);
    }

    private void CheckGameOver()
    {
        var winner = TicTacTwoBrain.CheckWin();
        if (winner != null)
        {
            var gameState = JsonSerializer.Deserialize<GameState>(TicTacTwoBrain.GetGameStateJson());
            
            string winnerName = winner == EGamePiece.X ? gameState.PlayerX : gameState.PlayerO;
            ViewData["Message"] = $"{winnerName} wins!";
            IsGameOver = true;
        }
        else if (TicTacTwoBrain.CheckDraw())
        {
            ViewData["Message"] = "It's a draw!";
            IsGameOver = true;
        }
    }

    private void PerformAction()
    {
        bool hasPiecesLeft = TicTacTwoBrain.HasPiecesLeft();
        bool canMovePiece = TicTacTwoBrain.CanMovePiece();
        bool canMoveGrid = TicTacTwoBrain.CanMoveGrid();

        switch (SelectedAction)
        {
            case "PlaceNewButton":
                if (hasPiecesLeft)
                {
                    TicTacTwoBrain.PlaceNewPiece(CoordinateX, CoordinateY);
                }
                break;
            case "MoveOldButton":
                if (canMovePiece)
                {
                    TicTacTwoBrain.MoveExistingPiece(OldCoordinateX, OldCoordinateY, CoordinateX, CoordinateY);
                }
                break;
            case "MoveGrid":
                if (canMoveGrid)
                {
                    TicTacTwoBrain.MoveGrid(CoordinateX, CoordinateY);
                }
                break;
            default:
                return;
        }
        GameName = _gameRepository.UpdateGame(TicTacTwoBrain.GetGameStateJson(), GameName, TicTacTwoBrain.GetGameConfig(), UserName);
        CheckGameOver();
    }

    private void UpdateActionSelectList()
    {
        var actions = new List<string>();

        if (TicTacTwoBrain.HasPiecesLeft()) //TODO: SAAB IKKA NUPPE PANNA, VIGA ON MÄNGULAUAS
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