using UnityEngine;

namespace Services
{
    public struct Match
    {
        public Vector2Int To { get; }
        public Vector2Int From { get; }

        public Match(Vector2Int from, Vector2Int to)
        {
            From = from;
            To = to;
        }
    }
}