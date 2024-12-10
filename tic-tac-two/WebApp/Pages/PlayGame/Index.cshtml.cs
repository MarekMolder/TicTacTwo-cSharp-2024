using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    
    [BindProperty]
    public Domain.GameConfiguration GameConfig { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string ConfigName { get; set; } = default!;
    
    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;
    
    [BindProperty]
    public string SelectedAction { get; set; } = default!;
    
    [BindProperty]
    public int CoordinateX { get; set; }
    
    [BindProperty]
    public int CoordinateY { get; set; }
    
    [BindProperty]
    public int OldCoordinateX { get; set; }
    
    [BindProperty]
    public int OldCoordinateY { get; set; }

    public SelectList ActionSelectList { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)] 
    public int GameId { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public string GameName { get; set; }
    
    [BindProperty]
    public bool IsGameOver { get; set; } = false;
    
    [BindProperty(SupportsGet = true)]
    public string UserName { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)]
    public int BoardWidth { get; set; }

    [BindProperty(SupportsGet = true)]
    public int BoardHeight { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PieceNumber { get; set; }

    [BindProperty(SupportsGet = true)]
    public int WinCondition { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PieceMove { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool Grid { get; set; }

    [BindProperty(SupportsGet = true)]
    public int GridWidth { get; set; }

    [BindProperty(SupportsGet = true)]
    public int GridHeight { get; set; }

    [BindProperty(SupportsGet = true)]
    public int GridMove { get; set; }

    [BindProperty(SupportsGet = true)]
    public int GridX { get; set; }

    [BindProperty(SupportsGet = true)]
    public int GridY { get; set; }

    public IActionResult OnGet()
    {
        if (!string.IsNullOrEmpty(GameName))
        {
            LoadExistingGame();
        }
        else if (!string.IsNullOrEmpty(ConfigName))
        {
            StartNewGame();
            return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName });
        }
        else
        {
            StartGameWithCustomConfig();
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
                PerformAction();
                return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName });
            }
        } 
        else if (!string.IsNullOrEmpty(ConfigName))
        {
            StartNewGame();
            return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName });
        }
        else
        {
            StartGameWithCustomConfig();
            return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName });
        }
        return Page();
    }

   
        private void LoadExistingGame()
        {
            var dbGame = _gameRepository.FindSavedGame(GameName);
            
            var configName = GameName.Split('_')[0];
           
            
            
            TicTacTwoBrain = new TicTacTwoBrain(new Domain.GameConfiguration()
            {
                Name = configName
            });

            TicTacTwoBrain.SetGameStateJson(dbGame);
            UpdateActionSelectList();
            CheckGameOver();
        }

        private void StartNewGame()
        {
            var gameSettings = _configRepository.GetConfigurationByName(ConfigName);
            TicTacTwoBrain = new TicTacTwoBrain(gameSettings);
            GameName = _gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig());
        }
        
        private void StartGameWithCustomConfig()
        {
            
            
            var gameConfig = new Domain.GameConfiguration
            {
                Name = "Custom",
                BoardSizeWidth = BoardWidth,
                BoardSizeHeight = BoardHeight,
                PiecesNumber = PieceNumber,
                WinCondition = WinCondition,
                MovePieceAfterNMove = PieceMove,
                UsesGrid = Grid,
                GridSizeWidth = GridWidth,
                GridSizeHeight = GridHeight,
                MoveGridAfterNMove = GridMove,
                GridPositionX = GridX,
                GridPositionY = GridY
            };

            // Loo TicTacTwoBrain objekt vastavalt kohandatud konfiguratsioonile
            TicTacTwoBrain = new TicTacTwoBrain(gameConfig);

            // Salvesta mängu ja saad mängu ID
            GameName = _gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig());
        }

        private void CheckGameOver()
        {
            var winner = TicTacTwoBrain.CheckWin();
            if (winner != null)
            {
                string winnerName = winner == EGamePiece.X ? "Player X" : "Player O";
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
            switch (SelectedAction)
            {
                case "new":
                    TicTacTwoBrain.PlaceNewPiece(CoordinateX, CoordinateY);
                    break;
                case "old":
                    TicTacTwoBrain.MoveExistingPiece(OldCoordinateX, OldCoordinateY, CoordinateX, CoordinateY);
                    break;
                case "grid":
                    TicTacTwoBrain.MoveGrid(CoordinateX, CoordinateY);
                    break;
                case "save":
                    // No game state change, just saving the current state
                    break;
                case "exit":
                    // Handle exit logic, if any
                    return;
                default:
                    return;
            }
            // Save game state after action is performed
            GameName = _gameRepository.Savegame(TicTacTwoBrain.GetGameStateJson(), TicTacTwoBrain.GetGameConfig());
            CheckGameOver();
        }

        private void UpdateActionSelectList()
        {
            var actions = new List<string>();

            if (TicTacTwoBrain.HasPiecesLeft())
            {
                actions.Add("new");
            }

            if (TicTacTwoBrain.CanMovePiece())
            {
                actions.Add("old");
            }

            if (TicTacTwoBrain.CanMoveGrid())
            {
                actions.Add("grid");
            }

            actions.Add("save");
            actions.Add("exit");

            ActionSelectList = new SelectList(actions);
        }
}
