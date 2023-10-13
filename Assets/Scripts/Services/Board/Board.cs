using Common.Extensions;
using Settings;
using UnityEngine;
using UnityEngine.Assertions;

namespace Services.Board
{
    public class Board
    {
        private readonly TileModel[,] _tiles;
        private readonly Vector2Int _boardSize;

        public Board(Vector2Int boardSize)
        {
            _boardSize = boardSize;
            _tiles = new TileModel[_boardSize.x, _boardSize.y];
            for (var x = 0; x < _tiles.GetLength(0); x++)
            for (var y = 0; y < _tiles.GetLength(1); y++)
            {
                _tiles[x, y] = new TileModel(x, y, null);
            }
        }

        public Vector2Int GetSize() => _boardSize;

        public TileModel GetTile(Vector2Int position) => GetTile(position.x, position.y);

        public TileModel GetTile(int x, int y)
        {
            // just to make sure that tiles' position is always synchronized
            Assert.AreEqual(x, _tiles[x, y].Position.x);
            Assert.AreEqual(y, _tiles[x, y].Position.y);

            return _tiles[x, y];
        }

        public void SetTile(TileType type, int x, int y)
        {
            _tiles[x, y].Position = new Vector2Int(x, y);
            _tiles[x, y].Type = type;
        }

        public void SwapTiles(Vector2Int first, Vector2Int second)
        {
            _tiles[first.x, first.y].Position = second;
            _tiles[second.x, second.y].Position = first;

            _tiles.Swap(first, second);
        }

        public void RemoveTile(Vector2Int tileIndex)
        {
            _tiles[tileIndex.x, tileIndex.y].Type = null;
        }

        #region Equality members

        public override bool Equals(object otherObject)
        {
            if (otherObject == null || GetType() != otherObject.GetType())
            {
                return false;
            }

            var otherBoard = (Board)otherObject;
            for (var x = 0; x < _tiles.GetLength(0); x++)
            {
                for (var y = 0; y < _tiles.GetLength(1); y++)
                {
                    if (otherBoard._tiles[x, y] != _tiles[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (_tiles != null ? _tiles.GetHashCode() : 0);
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