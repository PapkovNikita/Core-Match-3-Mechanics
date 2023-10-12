using Extensions;
using Settings;
using UnityEngine;

namespace Match3
{
    public class Board
    {
        public TileSettings[,] Tiles { get; private set; }

        public Board(TileSettings[,] tiles)
        {
            Tiles = tiles;
        }

        public void Initialize(TileSettings[,] tiles)
        {
            Tiles = tiles;
        }

        public void SwapTiles(Vector3Int first, Vector3Int second)
        {
            Tiles.Swap(first, second);
        }

        public void RemoveTile(Vector3Int tileIndex)
        {
            Tiles[tileIndex.x, tileIndex.y] = null;
        }

        #region Equality members

        public override bool Equals(object otherObject)
        {
            if (otherObject == null || GetType() != otherObject.GetType())
            {
                return false;
            }

            var otherBoard = (Board)otherObject;
            for (var x = 0; x < Tiles.GetLength(0); x++)
            {
                for (var y = 0; y < Tiles.GetLength(1); y++)
                {
                    if (otherBoard.Tiles[x, y] != Tiles[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (Tiles != null ? Tiles.GetHashCode() : 0);
        }

        public static bool operator ==(Board left, Board right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Board left, Board right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}