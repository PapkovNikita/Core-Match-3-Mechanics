using UnityEngine;

namespace Events
{
    public struct TilesSwappedEvent
    {
        public bool Successful { get; private set; }
        public Vector2Int First { get; private set; }
        public Vector2Int Second { get; private set; }


        public TilesSwappedEvent(Vector2Int first, Vector2Int second, bool successful)
        {
            Successful = successful;
            First = first;
            Second = second;
        }
    }
}