using System;
using Match3;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class LevelSettings : ILevelSettings
    {
        [field: SerializeField] public Vector2Int Size { get; private set; }
        [field: SerializeField] public TileSettings[] AvailableTiles { get; private set; }
    }
}