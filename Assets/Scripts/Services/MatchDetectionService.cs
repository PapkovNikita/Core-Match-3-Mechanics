using System;
using System.Collections.Generic;
using Common;
using Common.Extensions;
using Common.Math;
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
            var tileAtPosition = board.GetTileModel(position);
            Assert.IsFalse(tileAtPosition.IsRemoved);
            var lineSize = board.GetSize()[axis];

            var matchSize = 1;

            var from = position[axis];
            var to = Math.Min(position[axis] + MIN_MATCH_SIZE, lineSize);

            for (var i = from + 1; i < to; i++)
            {
                var currentPosition = position.WithValueAtAxis(i, axis);
                var currentTile = board.GetTileModel(currentPosition);
                if (tileAtPosition.Type == currentTile.Type && !currentTile.IsRemoved)
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
                var currentPosition = position.WithValueAtAxis(i, axis);
                var currentTile = board.GetTileModel(currentPosition);
                if (tileAtPosition.Type == currentTile.Type && !currentTile.IsRemoved)
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

                var previousTile = board.GetTileModel(previousTileIndex);
                var currentTile = board.GetTileModel(currentTileIndex);

                var isMatch = currentTile.Type == previousTile.Type && !currentTile.IsRemoved && !previousTile.IsRemoved;
                if (isMatch)
                {
                    matchSize++;
                }

                var isLastTile = i == lineSize - 1;
                if (isMatch && !isLastTile)
                {
                    continue;
                }

                if (matchSize >= MIN_MATCH_SIZE)
                {
                    var from = startPosition.WithValueAtAxis(i - matchSize, axis);
                    var to = startPosition.WithValueAtAxis(i - 1, axis);

                    if (isMatch)
                    {
                        from[axis] = ++from[axis];
                        to[axis] = ++to[axis];
                    }

                    result.Add(new Match(from, to));
                }

                matchSize = 1;
            }
        }
    }
}