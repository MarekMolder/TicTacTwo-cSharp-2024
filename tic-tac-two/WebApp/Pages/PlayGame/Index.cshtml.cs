using System.Text.Json;
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

    [BindProperty] public Domain.GameConfiguration GameConfig { get; set; }

    [BindProperty(SupportsGet = true)] public string ConfigName { get; set; } = default!;

    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    [BindProperty] public string SelectedAction { get; set; } = default!;

    [BindProperty] public int CoordinateX { get; set; }

    [BindProperty] public int CoordinateY { get; set; }

    [BindProperty] public int OldCoordinateX { get; set; }

    [BindProperty] public int OldCoordinateY { get; set; }

    public SelectList ActionSelectList { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public int GameId { get; set; }

    [BindProperty(SupportsGet = true)] public string GameName { get; set; }

    [BindProperty] public bool IsGameOver { get; set; } = false;

    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;
    

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

        return Page();
    }


    private void LoadExistingGame()
    {
        var state = _gameRepository.FindSavedGame(GameName);

        var configName = GameName.Split('_')[0];

        var config = GetConfiguration.LoadGameConfiguration(state);

        TicTacTwoBrain = new TicTacTwoBrain(config);

        TicTacTwoBrain.SetGameStateJson(state);
        UpdateActionSelectList();
        CheckGameOver();
    }

    private void StartNewGame()
    {
        var gameConfig = _configRepository.GetConfigurationByName(ConfigName);
        TicTacTwoBrain = new TicTacTwoBrain(gameConfig);
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

    private IActionResult PerformAction()
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
            default:
                return Page();
        }

        // Save game state after action is performed
        GameName = _gameRepository.UpdateGame(TicTacTwoBrain.GetGameStateJson(), GameName,
            TicTacTwoBrain.GetGameConfig());
        CheckGameOver();
        return Page();
    }

    private void UpdateActionSelectList()
    {
        var actions = new List<string>();

        if (TicTacTwoBrain.HasPiecesLeft()) //TODO: SAAB IKKA NUPPE PANNA, VIGA ON MÄNGULAUAS
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

        ActionSelectList = new SelectList(actions);
    }
}