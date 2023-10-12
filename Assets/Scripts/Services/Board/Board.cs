using Extensions;
using Settings;
using UnityEngine;

namespace Services.Board
{
    public class TileModel
    {
        public TileType Type { get; }
        public Vector3Int Position { get; }

        public TileModel(TileType tileType)
        {
            Type = tileType;
        }
    }
    
    public class Board
    {
        private TileType[,] _tiles;
        private Vector2Int _boardSize;

        public Board(TileType[,] tiles)
        {
            Initialize(tiles);
        }
        
        public void Initialize(TileType[,] tiles)
        {
            _tiles = tiles;
            _boardSize = tiles == null ?
                Vector2Int.one : 
                new Vector2Int(_tiles.GetLength(0), _tiles.GetLength(1));
        }

        public Vector2Int GetSize() => _boardSize;

        public TileType Get(Vector3Int position) => Get(position.x, position.y);
        
        public TileType Get(int x, int y) => _tiles[x, y];

        public void Set(TileType type, int x, int y) => _tiles[x, y] = type;

        public void SwapTiles(Vector3Int first, Vector3Int second)
        {
            _tiles.Swap(first, second);
        }

        public void RemoveTile(Vector3Int tileIndex)
        {
            _tiles[tileIndex.x, tileIndex.y] = null;
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