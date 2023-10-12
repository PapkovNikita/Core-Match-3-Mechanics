using System;
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

    private Vector3Int GenerateRandomPosition()
    {
        var boardSize = _levelSettings.Size; 
        
        var x = Random.Range(-1, boardSize.x + 1);
        var y = Random.Range(-1, boardSize.y + 1);
        
        return new Vector3Int(x, y);
    }

    public event Action<SwipeData> Swiped;
}