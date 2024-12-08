using Domain;
using GameBrain;

namespace DAL;

/// <summary>
/// Defines operations for interacting with game configuration data storage.
/// </summary>
public interface IConfigRepository
{
    /// <summary>
    /// Retrieves a list of all available game configuration names.
    /// </summary>
    /// <returns>A list of strings representing the names of all game configurations.</returns>
    List<string> GetConfigurationNames();
    
    /// <summary>
    /// Retrieves a game configuration by its name.
    /// </summary>
    /// <param name="name">The name of the game configuration to retrieve.</param>
    /// <returns>A <see cref="GameConfiguration"/> object corresponding to the given name.</returns>
    GameConfiguration GetConfigurationByName(string name);
}