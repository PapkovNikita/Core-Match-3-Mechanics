using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class LevelSettings : ILevelSettings
    {
        [field: SerializeField] public Vector2Int Size { get; private set; }
        [field: SerializeField] public TileType[] AvailableTiles { get; private set; }
    }
}