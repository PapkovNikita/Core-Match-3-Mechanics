using UnityEngine;

namespace Services
{
    public struct SwipeData
    {
        public Vector3Int StartIndex;
        public Vector3Int EndIndex;

        public SwipeData(Vector3Int startIndex, Vector3Int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}