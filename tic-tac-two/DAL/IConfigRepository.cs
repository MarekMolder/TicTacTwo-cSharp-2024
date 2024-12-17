using GameConfiguration = Domain.GameConfiguration;

namespace DAL;

public interface IConfigRepository
{
    /// <summary>
    /// Retrieves a list of all available game configuration names.
    /// </summary>
    List<string> GetConfigurationNames();
    
    /// <summary>
    /// Retrieves a game configuration by its name.
    /// </summary>
    GameConfiguration GetConfigurationByName(string name);

    /// <summary>
    /// Saves a specific configuration.
    /// </summary>
    void SaveConfiguration(GameConfiguration config);
}