using UnityEngine;

namespace Services
{
    public struct SwipeData
    {
        public Vector2Int StartIndex;
        public Vector2Int EndIndex;

        public SwipeData(Vector2Int startIndex, Vector2Int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}