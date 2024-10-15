﻿using GameBrain;

namespace DAL;

public abstract class ConfigRepository
{
    private static readonly List<GameConfiguration> GameConfigurations =
    [
        new GameConfiguration()
        {
            Name = "Classical",
            BoardSizeWidth = 3,
            BoardSizeHeight = 3,
            PiecesNumber = 5,
            WinCondition = 3,
            UsesGrid = false,
            GridSizeWidth = 0,
            GridSizeHeight = 0,
            MovePieceAfterNMove = 10,
            MoveGridAfterNMove = 100,
            GridPositionX = 0,
            GridPositionY = 0
        },

        new GameConfiguration()
        {
            Name = "Regular tic-tac-two",
            BoardSizeWidth = 5,
            BoardSizeHeight = 5,
            PiecesNumber = 4,
            WinCondition = 3,
            UsesGrid = true,
            GridSizeHeight = 3,
            GridSizeWidth = 3,
            MovePieceAfterNMove = 2,
            MoveGridAfterNMove = 2,
            GridPositionX = 1,
            GridPositionY = 1
        }
    ];

    public static List<string> GetConfigurationNames()
    {
        return GameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }
    
    public static GameConfiguration GetConfigurationByName(string name)
    {
        return GameConfigurations.Single(c => c.Name == name);
    }
}