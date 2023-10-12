using UnityEngine;

namespace Events
{
    public struct TilesSwappedEvent
    {
        public bool Successful { get; private set; }
        public Vector3Int First { get; private set; }
        public Vector3Int Second { get; private set; }


        public TilesSwappedEvent(Vector3Int first, Vector3Int second, bool successful)
        {
            Successful = successful;
            First = first;
            Second = second;
        }
    }
}