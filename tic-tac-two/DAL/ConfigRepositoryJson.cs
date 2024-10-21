using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    
public List<string> GetConfigurationNames()
{
    CheckAndCreateInitialConfig();
    
    // Get all JSON configuration files in the directory without .config.json extension
    return System.IO.Directory
        .GetFiles(FileHelper._basePath, "*" + FileHelper.ConfigExtension)
        .Select(filePath => System.IO.Path.GetFileName(filePath)
            .Replace(FileHelper.ConfigExtension, "")) // Remove the .config.json part
        .ToList();
}

public GameConfiguration GetConfigurationByName(string name)
{
    // Construct the full path of the config file
    var filePath = System.IO.Path.Combine(FileHelper._basePath, $"{name}" + FileHelper.ConfigExtension);

    if (!System.IO.File.Exists(filePath))
    {
        throw new FileNotFoundException($"Configuration file for {name} not found.");
    }

    // Read the JSON content and deserialize it to GameConfiguration
    var jsonContent = System.IO.File.ReadAllText(filePath);
    return System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(jsonContent);
}

private void CheckAndCreateInitialConfig()
{
    // Ensure the directory exists
    if (!System.IO.Directory.Exists(FileHelper._basePath))
    {
        System.IO.Directory.CreateDirectory(FileHelper._basePath);
    }

    // Check if there are any .config.json files
    var configFiles = System.IO.Directory.GetFiles(FileHelper._basePath, "*" + FileHelper.ConfigExtension).ToList();
    
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
            var filePath = System.IO.Path.Combine(FileHelper._basePath, $"{gameOption.Name}" + FileHelper.ConfigExtension);
            System.IO.File.WriteAllText(filePath, optionJsonStr);
        }
    }
}

}