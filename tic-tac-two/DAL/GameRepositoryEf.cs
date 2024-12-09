/*
using System.Text.Json;
using Domain;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;


namespace DAL;

public class GameRepositoryEf : IGameRepository2
{
    private readonly AppDbContext _context;

    public GameRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    public void Save(Guid id, GameState state, GameConfiguration gameConfig)
    {
        var game = _context.SaveGames.FirstOrDefault(g => g.Id == state.Id);
        if (game == null)
        {
            game = new SaveGame()
            {
                Id = state.Id,
                State = JsonSerializer.Serialize(state, JsonSerializerOptions),
                GameConfigurationId = gameConfig.Id
            };
            _context.SaveGames.Add(game);
        }
        else
        {
            game.UpdatedAtDt = DateTime.Now;
            game.State = JsonSerializer.Serialize(state, JsonSerializerOptions);
        }
    }

    public List<(Guid id, DateTime dt)> GetSavedGames()
    {
        return _context.SaveGames
            .OrderByDescending(g => g.UpdatedAtDt)
            .ToList()
            .Select(g => (g.Id, g.UpdatedAtDt))
            .ToList();
    }

    public GameState LoadGame(Guid id)
    {
        var game = _context.SaveGames.First(g => g.Id == id);
        return JsonSerializer.Deserialize<GameState>(game.State, JsonSerializerOptions)!;
    }
    
    public void DeleteGame(Guid id)
    {
        var game = _context.SaveGames.First(g => g.Id == id);

        if (game != null)
        {
            _context.SaveGames.Remove(game);
            _context.SaveChanges();    
        }
    }
    
    public string? FindSavedGame(string gameName)
    {
        // Remove the timestamp part to search only by configuration name
        var configName = gameName.Split('_')[0];

        var savedGame = _context.SaveGames
            .Include(sg => sg.GameConfiguration)
            .OrderByDescending(sg => sg.CreatedAtDt)
            .FirstOrDefault(sg => sg.GameConfiguration != null && sg.GameConfiguration.Name == configName);

        if (savedGame == null)
        {
            Console.WriteLine($"No saved game found with the name '{gameName}'.");
            return null;
        }
        return savedGame.State;
    }
    
    public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        AllowTrailingCommas = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
*/
