using UnityEngine;

namespace GameStates
{
    public struct SwapStateContext
    {
        public Vector2Int FirstTile { get; }
        public Vector2Int SecondTile { get; }

        public SwapStateContext(Vector2Int firstTile, Vector2Int secondTile)
        {
            FirstTile = firstTile;
            SecondTile = secondTile;
        }
    }
}