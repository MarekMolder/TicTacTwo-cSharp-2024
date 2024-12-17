using Domain;

namespace DAL;

public class ConfigRepositoryDb(AppDbContext context) : IConfigRepository
{
    /// <summary>
    /// Retrieves all configuration names from the database, sorted by name.
    /// </summary>
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();
        
        return context.GameConfigurations
            .OrderBy(config => config.Name)
            .Select(config => config.Name)
            .ToList();
    }

    /// <summary>
    /// Retrieves a specific game configuration by its name.
    /// </summary>
    public GameConfiguration GetConfigurationByName(string name)
    {
        var config = context.GameConfigurations.FirstOrDefault(c => c.Name == name);

        if (config == null)
        {
            throw new KeyNotFoundException($"Configuration with name '{name}' not found.");
        }

        return config;
    }

    /// <summary>
    /// Ensures that initial game configurations exist in the database.
    /// If no configurations exist, creates them based on hardcoded values.
    /// </summary>
    private void CheckAndCreateInitialConfig()
    {
        if (context.GameConfigurations.Any()) return;

        var hardCodedRepo = new ConfigRepositoryHardCoded();
        var newConfigs = hardCodedRepo
            .GetConfigurationNames()
            .Select(hardCodedRepo.GetConfigurationByName)
            .ToList();
    
        context.GameConfigurations.AddRange(newConfigs);
        context.SaveChanges();
    }
    
    /// <summary>
    /// Saves a specific configuration
    /// </summary>
    public void SaveConfiguration(GameConfiguration config)
    {
        context.GameConfigurations.Add(config);
        context.SaveChanges();
    }
}