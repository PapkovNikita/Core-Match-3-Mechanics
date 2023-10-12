using UnityEngine;

namespace GameStates
{
    public struct SwapStateContext
    {
        public Vector3Int FirstTile { get; }
        public Vector3Int SecondTile { get; }

        public SwapStateContext(Vector3Int firstTile, Vector3Int secondTile)
        {
            FirstTile = firstTile;
            SecondTile = secondTile;
        }
    }
}