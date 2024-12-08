using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;

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
    
    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)]
    public string GameName { get; set; } = default!;
    
    
    public void OnGet()
    {
        var gameState = _gameRepository.LoadGame(GameName); 
        TicTacTwoBrain = new TicTacTwoBrain(new Domain.GameConfiguration()
        {
            Name = "Classical"
        });

        TicTacTwoBrain.SetGameStateJson(gameState.ToString());
    }

    public void OnPost()
    {
        var gameState = _gameRepository.LoadGame(GameName);
        TicTacTwoBrain = new TicTacTwoBrain(new Domain.GameConfiguration()
        {
            Name = "Classical"
        });

        TicTacTwoBrain.SetGameStateJson(gameState.ToString());
        
        
        
    }
}