using UnityEngine;

namespace Services
{
    public struct Match
    {
        public Vector3Int To { get; }
        public Vector3Int From { get; }

        public Match(Vector3Int from, Vector3Int to)
        {
            From = from;
            To = to;
        }
    }
}