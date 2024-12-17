namespace DAL;

public static class FileHelper
{
    /// <summary>
    /// The path to the folder where all the game files are stored.
    /// </summary>
    public static readonly string BasePath = Environment
                                                 .GetFolderPath(Environment.SpecialFolder.UserProfile)
                                             + Path.DirectorySeparatorChar + "tic-tac-two" + Path.DirectorySeparatorChar;
    
    /// <summary>
    /// The file extension for configuration files.
    /// </summary>
    public static readonly string ConfigExtension = ".config.json";
    
    /// <summary>
    /// The file extension for game files.
    /// </summary>
    public static readonly string GameExtension = ".game.json";
    
    
}