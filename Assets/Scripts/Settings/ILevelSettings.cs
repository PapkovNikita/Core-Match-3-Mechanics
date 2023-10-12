using UnityEngine;

namespace Settings
{
    public interface ILevelSettings
    {
        public Vector2Int Size { get; }
        public TileType[] AvailableTiles { get; }
    }
}