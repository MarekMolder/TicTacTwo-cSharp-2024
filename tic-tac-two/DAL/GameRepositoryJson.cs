using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    public void Savegame(string jsonStateString, string gameConfigName)
    {
        // Ensure gameConfigName is sanitized for file naming
        string sanitizedGameConfigName = string.Join("_", gameConfigName.Split(Path.GetInvalidFileNameChars()));

        // Replace colons in the timestamp for compatibility with file names
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        var fileName = Path.Combine(FileHelper._basePath, $"{sanitizedGameConfigName}_{timestamp}{FileHelper.GameExtension}");

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
    
}