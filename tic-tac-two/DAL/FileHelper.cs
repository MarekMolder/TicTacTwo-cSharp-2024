namespace DAL;

/// <summary>
/// Helper class that provides methods and properties for file management.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// The path to the folder where all the game files are stored.
    /// </summary>
    public static string BasePath = Environment
                                            .GetFolderPath(Environment.SpecialFolder.UserProfile)
                                        + System.IO.Path.DirectorySeparatorChar + "tic-tac-two" + Path.DirectorySeparatorChar;
    
    /// <summary>
    /// The file extension for configuration files.
    /// </summary>
    public static string ConfigExtension = ".config.json";
    
    /// <summary>
    /// The file extension for game files.
    /// </summary>
    public static string GameExtension = ".game.json";
    
    
}