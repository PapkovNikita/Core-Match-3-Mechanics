using System;
using Extensions;
using Settings;
using UnityEngine;

namespace Services.Board
{
    public class TileModel
    {
        public Vector2Int Position { get; set; }

        public TileType Type { get; set; }

        public bool IsRemoved => Type == null;

        public TileModel(int x, int y, TileType type)
        {
            Position = new Vector2Int(x, y);
            Type = type;
        }

        #region Equality members

        public bool Equals(TileModel other)
        {
            return Position.Equals(other.Position) && Equals(Type, other.Type) && IsRemoved == other.IsRemoved;
        }

        public override bool Equals(object obj)
        {
            return obj is TileModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Type, IsRemoved);
        }

        public static bool operator ==(TileModel left, TileModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TileModel left, TileModel right)
        {
            return !left.Equals(right);
        }

        #endregion
    }

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

        public TileType GetTileType(int x, int y) => _tiles[x, y].Type;

        public TileModel GetTileModel(Vector2Int position) => GetTileModel(position.x, position.y);

        public TileModel GetTileModel(int x, int y) => _tiles[x, y];

        public void Set(TileType type, int x, int y)
        {
            _tiles[x, y].Position = new Vector2Int(x, y);
            _tiles[x,y].Type = type;
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