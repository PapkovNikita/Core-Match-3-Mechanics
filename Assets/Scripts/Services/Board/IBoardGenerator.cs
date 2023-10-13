using Settings;
using UnityEngine;

namespace Services.Board
{
    public interface IBoardGenerator
    {
        Board Generate(Vector2Int size, TileType[] availableTiles);
        void FillEmptyTiles(Board board, TileType[] availableTiles);
    }
}