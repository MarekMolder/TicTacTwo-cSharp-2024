using System.Text.Json;
using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    public void Savegame(string jsonStateString, GameConfiguration gameConfig)
    {
        // Ensure gameConfig.Name is sanitized for file naming
        string sanitizedGameConfigName = string.Join("_", gameConfig.Name.Split(Path.GetInvalidFileNameChars()));

        // Replace colons in the timestamp for compatibility with file names
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        var fileName = Path.Combine(FileHelper.BasePath, $"{sanitizedGameConfigName}_{timestamp}{FileHelper.GameExtension}");

        try
        {
            System.IO.File.WriteAllText(fileName, jsonStateString);
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error or inform the user)
            Console.WriteLine($"An error occurred while saving the game: {ex.Message}");
        }
    }
    
    public string? FindSavedGame(string gameName)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        return files.FirstOrDefault(filePath => Path.GetFileNameWithoutExtension(filePath).StartsWith(gameName));
    }
    
    public GameState LoadGame(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Saved game not found at {filePath}.");
        }

        // Loeme faili sisu ja deserialiseerime GameState objektiks
        var jsonState = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<GameState>(jsonState);
    }
    
    public List<string> GetSavedGameNames()
    {
        // Otsi kõiki mängufaile kindlast kataloogist
        var files = Directory.GetFiles(FileHelper.BasePath, $"*{FileHelper.GameExtension}");
        
        // Tagasta failinimed ilma laiendita
        return files.Select(filePath => Path.GetFileNameWithoutExtension(filePath)).ToList();
    }
    
}