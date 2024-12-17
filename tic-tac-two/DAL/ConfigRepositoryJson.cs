using Domain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{

    /// <summary>
    /// Retrieves a list of configuration names (without the file extension).
    /// </summary>
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();
        
        return Directory
            .GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension)
            .Select(filePath => Path.GetFileName(filePath)
                .Replace(FileHelper.ConfigExtension, ""))
            .ToList();
    }

    /// <summary>
    /// Retrieves a specific game configuration by its name.
    /// </summary>
    public GameConfiguration GetConfigurationByName(string name)
    {
        Console.WriteLine($"real: {name}");
       
        var filePath = Path.Combine(FileHelper.BasePath, $"{name}" + FileHelper.ConfigExtension);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configuration file for {name} not found.");
        }
        
        var jsonContent = File.ReadAllText(filePath);
        return System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(jsonContent)!;
    }

    /// <summary>
    /// Checks if the initial configuration files exist, and if not, creates them.
    /// </summary>
    private static void CheckAndCreateInitialConfig()
    {
        if (!Directory.Exists(FileHelper.BasePath))
        {
            Directory.CreateDirectory(FileHelper.BasePath);
        }
        
        var configFiles = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();
        
        if (configFiles.Count == 0)
        {
            var hardCodedRepo = new ConfigRepositoryHardCoded();
            var optionNames = hardCodedRepo.GetConfigurationNames();
            
            foreach (var optionName in optionNames)
            {
                var gameOption = hardCodedRepo.GetConfigurationByName(optionName);
                var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(gameOption);
                
                var filePath = Path.Combine(FileHelper.BasePath, $"{gameOption.Name}" + FileHelper.ConfigExtension);
                File.WriteAllText(filePath, optionJsonStr);
            }
        }
    }
    
    /// <summary>
    /// Saves a specific configuration
    /// </summary>
    public void SaveConfiguration(GameConfiguration config)
    {
        if (!Directory.Exists(FileHelper.BasePath))
        {
            Directory.CreateDirectory(FileHelper.BasePath);
        }
        
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(config);
        
        var filePath = Path.Combine(FileHelper.BasePath, $"{config.Name}" + FileHelper.ConfigExtension);

        File.WriteAllText(filePath, jsonContent);
    }
}