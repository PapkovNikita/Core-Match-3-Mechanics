using System;
using Settings;
using UnityEngine;

namespace Services.Board
{
    public class TileModel
    {
        public Vector2Int Position { get; set; }

        public TileType Type { get; set; }

        public bool IsEmpty => Type == null;

        public TileModel(int x, int y, TileType type)
        {
            Position = new Vector2Int(x, y);
            Type = type;
        }

        #region Equality members

        public bool Equals(TileModel other)
        {
            return Position.Equals(other.Position) && Equals(Type, other.Type) && IsEmpty == other.IsEmpty;
        }

        public override bool Equals(object obj)
        {
            return obj is TileModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Type, IsEmpty);
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
}