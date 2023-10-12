using System;
using System.Collections.Generic;
using Extensions;
using Match3;
using Misc;
using UnityEngine;

namespace Services
{
    public class MatchDetectionService
    {
        private const int MIN_MATCH_SIZE = 3;

        public bool HasMatchAt(Board board, Vector3Int position)
        {
            var horizontalMatch = GetMatchSizeByAxisAt(board, position, Axis.X);
            var verticalMatch = GetMatchSizeByAxisAt(board, position, Axis.Y);

            return horizontalMatch >= MIN_MATCH_SIZE ||
                   verticalMatch >= MIN_MATCH_SIZE;
        }

        private static int GetMatchSizeByAxisAt(Board board, Vector3Int position, int axis)
        {
            var tileAtPosition = board.Tiles[position.x, position.y];
            var lineSize = board.Tiles.GetLength(axis);

            var matchSize = 1;

            var from = position[axis];
            var to = Math.Min(position[axis] + MIN_MATCH_SIZE, lineSize);

            for (var i = from + 1; i < to; i++)
            {
                var currentPosition = position.WithValueAtAxis(i, axis);
                if (tileAtPosition == board.Tiles.At(currentPosition))
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
                if (tileAtPosition == board.Tiles.At(currentPosition))
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

        public List<Match> GetAllMatches(Board board)
        {
            var result = new List<Match>();

            var width = board.Tiles.GetLength(0);
            var height = board.Tiles.GetLength(1);
            for (var column = 0; column < width; column++)
            {
                FindMatchesOnLine(board, new Vector3Int(column, 0), Axis.Y, result);
            }

            for (var row = 0; row < height; row++)
            {
                FindMatchesOnLine(board, new Vector3Int(0, row), Axis.X, result);
            }

            return result;
        }

        private static void FindMatchesOnLine(Board board, Vector3Int startPosition,
            int axis, List<Match> result)
        {
            var lineSize = board.Tiles.GetLength(axis);
            var matchSize = 1;
            for (var i = 1; i < lineSize; i++)
            {
                var previousTileIndex = startPosition.WithValueAtAxis(i - 1, axis);
                var currentTileIndex = startPosition.WithValueAtAxis(i, axis);

                var previousTile = board.Tiles.At(previousTileIndex);
                var currentTile = board.Tiles.At(currentTileIndex);

                var isMatch = currentTile == previousTile && currentTile != null;
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
                    // 0 0 0 0 1 1 2 3 MatchSize: 4 I: 4 From: 0: To: 3
                    // 1 0 0 0 0 MatchSize: 4 I: 4 From: 1: To: 4
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