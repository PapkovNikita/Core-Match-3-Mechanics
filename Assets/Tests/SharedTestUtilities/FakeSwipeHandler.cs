﻿using System;
using Services;
using Settings;
using UnityEngine;
using Random = UnityEngine.Random;

public class FakeSwipeHandler : ISwipeHandler
{
    private readonly ILevelSettings _levelSettings;

    public FakeSwipeHandler(ILevelSettingsProvider levelSettingsProvider)
    {
        _levelSettings = levelSettingsProvider.GetCurrentLevelSettings();
    }

    public void Tick()
    {
        var swipe = new SwipeData(GenerateRandomPosition(), GenerateRandomPosition());
        Swiped?.Invoke(swipe);
    }

    private Vector2Int GenerateRandomPosition()
    {
        var boardSize = _levelSettings.Size; 
        
        // For testing purposes, we sometimes swipe outside the board.
        // Hence, the range is slightly larger than the board's borders.
        var x = Random.Range(-1, boardSize.x + 1);
        var y = Random.Range(-1, boardSize.y + 1);
        
        return new Vector2Int(x, y);
    }

    public event Action<SwipeData> Swiped;
}