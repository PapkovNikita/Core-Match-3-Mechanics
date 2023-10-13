using System;
using System.Collections.Generic;
using Common.Extensions;
using Common.Math;
using Services.Board;
using UnityEngine;
using UnityEngine.Assertions;

namespace Services
{
    public class MatchDetectionService
    {
        private const int MIN_MATCH_SIZE = 3;

        public bool HasMatchAt(Board.Board board, Vector2Int position)
        {
            var horizontalMatch = GetMatchSizeByAxisAt(board, position, Axis.X);
            var verticalMatch = GetMatchSizeByAxisAt(board, position, Axis.Y);

            return horizontalMatch >= MIN_MATCH_SIZE ||
                   verticalMatch >= MIN_MATCH_SIZE;
        }

        private static int GetMatchSizeByAxisAt(Board.Board board, Vector2Int position, int axis)
        {
            var tileAtPosition = board.GetTile(position);
            Assert.IsFalse(tileAtPosition.IsEmpty);
            var lineSize = board.GetSize()[axis];

            var matchSize = 1;

            var from = position[axis];
            var to = Math.Min(position[axis] + MIN_MATCH_SIZE, lineSize);

            for (var i = from + 1; i < to; i++)
            {
                if (IsMatch(board, position, axis, i, tileAtPosition))
                {
                    matchSize++;
                }
                else
                {
                    break;
                }
            }

            to = Math.Max(position[axis] - MIN_MATCH_SIZE, 0);
            for (var i = from - 1; i >= to; i--)
            {
                if (IsMatch(board, position, axis, i, tileAtPosition))
                {
                    matchSize++;
                }
                else
                {
                    break;
                }
            }

            return matchSize;
        }

        private static bool IsMatch(Board.Board board, Vector2Int position, int axis, int i, TileModel tileAtPosition)
        {
            var currentPosition = position.WithValueAtAxis(i, axis);
            var currentTile = board.GetTile(currentPosition);
            return tileAtPosition.Type == currentTile.Type && !currentTile.IsEmpty;
        }

        public List<Match> GetAllMatches(Board.Board board)
        {
            var result = new List<Match>();

            var width = board.GetSize().x;
            var height = board.GetSize().y;
            for (var column = 0; column < width; column++)
            {
                FindMatchesOnLine(board, new Vector2Int(column, 0), Axis.Y, result);
            }

            for (var row = 0; row < height; row++)
            {
                FindMatchesOnLine(board, new Vector2Int(0, row), Axis.X, result);
            }

            return result;
        }

        private static void FindMatchesOnLine(Board.Board board, Vector2Int startPosition,
            int axis, List<Match> result)
        {
            var lineSize = board.GetSize()[axis];
            var matchSize = 1;
            for (var i = 1; i < lineSize; i++)
            {
                var previousTileIndex = startPosition.WithValueAtAxis(i - 1, axis);
                var currentTileIndex = startPosition.WithValueAtAxis(i, axis);

                var previousTile = board.GetTile(previousTileIndex);
                var currentTile = board.GetTile(currentTileIndex);

                var isMatch = currentTile.Type == previousTile.Type && !currentTile.IsEmpty && !previousTile.IsEmpty;
                if (isMatch)
                {
                    matchSize++;
                }

                var isLastTile = i == lineSize - 1;
                if (isLastTile || !isMatch)
                {
                    TrySaveNewMatch(currentTileIndex, matchSize, axis, isMatch, result);
                    matchSize = 1;
                }
            }
        }

        private static void TrySaveNewMatch(Vector2Int currentPosition, int matchSize, int axis, 
            bool isCurrentTileMatched, List<Match> result)
        {
            if (matchSize >= MIN_MATCH_SIZE)
            {
                var from = currentPosition.WithValueAtAxis(currentPosition[axis] - matchSize + 1, axis);
                var to = currentPosition;

                if (!isCurrentTileMatched)
                {
                    to[axis] = --to[axis];
                    from[axis] = --from[axis];
                }

                result.Add(new Match(from, to));
            }
        }
    }
}