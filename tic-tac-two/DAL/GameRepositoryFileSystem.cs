/*
using System.Runtime.Serialization;
using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL;

public class GameRepositoryFileSystem : IGameRepository2
{
    private const string SaveLocation = "/Users/marek/tic-tac-two";
    
    public void Save(Guid id, GameState state, GameConfiguration gameConfiguration)
    {
        var content = JsonSerializer.Serialize(state, JsonSerializerOptions);

        var fileName = Path.ChangeExtension(id.ToString(), ".json");

        if (!Path.Exists(SaveLocation))
        {
            Directory.CreateDirectory(SaveLocation);
        }

        File.WriteAllText(Path.Combine(SaveLocation, fileName), content);
    }

    public List<(Guid id, DateTime dt)> GetSavedGames()
    {
        var data = Directory.EnumerateFiles(SaveLocation);
        var res = data
            .Select(
                path => (
                    Guid.Parse(Path.GetFileNameWithoutExtension(path)),
                    File.GetLastWriteTime(path)
                )
            ).ToList();
        
        return res;
    }

    public GameState LoadGame(Guid id)
    {
        var fileName = Path.ChangeExtension(id.ToString(), ".json");

        var jsonStr = File.ReadAllText(Path.Combine(SaveLocation, fileName));
        var res = JsonSerializer.Deserialize<GameState>(jsonStr, JsonSerializerOptions);
        if (res == null) throw new SerializationException($"Cannot deserialize {jsonStr}");

        return res;
    }
    public void DeleteGame(Guid id)
    {
        var fileName = Path.ChangeExtension(id.ToString(), ".json");
        var filePath = Path.Combine(SaveLocation, fileName);
        
        File.Delete(filePath);
    }

    public string? FindSavedGame(string gameName)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        return files.FirstOrDefault(filePath => Path.GetFileNameWithoutExtension(filePath).StartsWith(gameName));
    }

    public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        AllowTrailingCommas = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
*/