using GameBrain;

namespace DAL;

/// <summary>
/// Repository class for managing game configurations stored in the database.
/// </summary>
public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigRepositoryDb"/> class.
    /// </summary>
    /// <param name="context">The database context used for retrieving and saving game configurations.</param>
    public ConfigRepositoryDb(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all configuration names from the database, sorted by name.
    /// </summary>
    /// <returns>A list of configuration names.</returns>
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();
        
        // Retrieve configuration names from the database, ordered by name
        return _context.GameConfigurations
            .OrderBy(config => config.Name)
            .Select(config => config.Name)
            .ToList();
    }

    /// <summary>
    /// Retrieves a specific game configuration by its name.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve.</param>
    /// <returns>A <see cref="GameConfiguration"/> object representing the configuration.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the configuration with the specified name is not found.</exception>
    public GameConfiguration GetConfigurationByName(string name)
    {
        var config = _context.GameConfigurations.FirstOrDefault(c => c.Name == name);

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
        // Check if there are any configurations in the database
        if (!_context.GameConfigurations.Any())
        {
            // If no configurations exist, create them from hardcoded values
            var hardCodedRepo = new ConcreteConfigRepositoryHardCoded();
            var optionNames = hardCodedRepo.GetConfigurationNames();
            
            foreach (var optionName in optionNames)
            {
                var gameOption = hardCodedRepo.GetConfigurationByName(optionName);
                
                // Add the hardcoded configuration to the database
                _context.GameConfigurations.Add(gameOption);
            }
            
            _context.SaveChanges(); // Save changes to the database
        }
    }
}