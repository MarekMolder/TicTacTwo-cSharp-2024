﻿namespace DAL;

public static class FileHelper
{
    public static string _basePath = Environment
                                            .GetFolderPath(Environment.SpecialFolder.UserProfile)
                                        + System.IO.Path.DirectorySeparatorChar + "tic-tac-two";
    
    public static string ConfigExtension = ".config.json";
    
    public static string GameExtension = ".game.json";
}