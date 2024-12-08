using Domain;
using GameBrain;

namespace DAL;

/// <summary>
/// Repository class that manages game configuration files in JSON format.
/// </summary>
public class ConfigRepositoryJson : IConfigRepository
{

    /// <summary>
    /// Retrieves a list of configuration names (without the file extension).
    /// </summary>
    /// <returns>A list of configuration names.</returns>
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();
        
        // Get all JSON configuration files in the directory without .config.json extension
        return System.IO.Directory
            .GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension)
            .Select(filePath => System.IO.Path.GetFileName(filePath)
                .Replace(FileHelper.ConfigExtension, "")) // Remove the .config.json part
            .ToList();
    }

    /// <summary>
    /// Retrieves a specific game configuration by its name.
    /// </summary>
    /// <param name="name">The name of the configuration.</param>
    /// <returns>A <see cref="GameConfiguration"/> object representing the configuration.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the configuration file is not found.</exception>
    public GameConfiguration GetConfigurationByName(string name)
    {
        // Construct the full path of the config file
        var filePath = System.IO.Path.Combine(FileHelper.BasePath, $"{name}" + FileHelper.ConfigExtension);

        if (!System.IO.File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configuration file for {name} not found.");
        }

        // Read the JSON content and deserialize it to GameConfiguration
        var jsonContent = System.IO.File.ReadAllText(filePath);
        return System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(jsonContent)!;
    }

    /// <summary>
    /// Checks if the initial configuration files exist, and if not, creates them.
    /// </summary>
    private void CheckAndCreateInitialConfig()
    {
        // Ensure the directory exists
        if (!System.IO.Directory.Exists(FileHelper.BasePath))
        {
            System.IO.Directory.CreateDirectory(FileHelper.BasePath);
        }

        // Check if there are any .config.json files
        var configFiles = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();
        
        // If no configuration files exist, create them from hardcoded defaults
        if (configFiles.Count == 0)
        {
            var hardCodedRepo = new ConcreteConfigRepositoryHardCoded();
            var optionNames = hardCodedRepo.GetConfigurationNames();
            
            foreach (var optionName in optionNames)
            {
                var gameOption = hardCodedRepo.GetConfigurationByName(optionName);
                var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(gameOption);
                
                // Correctly construct the file path and write the configuration to the file
                var filePath = System.IO.Path.Combine(FileHelper.BasePath, $"{gameOption.Name}" + FileHelper.ConfigExtension);
                System.IO.File.WriteAllText(filePath, optionJsonStr);
            }
        }
    }
}