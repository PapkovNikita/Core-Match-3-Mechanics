using UnityEngine;

namespace Common.Extensions
{
    public static class VectorIntExtensions
    {
        public static Vector3Int ToVector3Int(this Vector2Int position) => new(position.x, position.y);
        
        public static Vector2Int ToVector2Int(this Vector3Int position) => new(position.x, position.y);
        
        public static Vector2Int WithValueAtAxis(this Vector2Int position, int value, int axis)
        {
            position[axis] = value;
            return position;
        }
    }
}