using System;
using System.Collections.Generic;
using Match3;
using Settings;
using UnityEngine;

namespace Services
{
    public class BoardService : IBoardProvider
    {
        public Board Board { get; private set; } = new(null);
        
        private readonly BoardGenerator _boardGenerator;

        public BoardService(BoardGenerator boardGenerator)
        {
            _boardGenerator = boardGenerator;
        }

        public void GenerateBoard(ILevelSettings levelSettings)
        {
            Board = _boardGenerator.Generate(levelSettings.Size, levelSettings.AvailableTiles);
        }

        public void FillEmptyTiles(TileSettings[] availableTiles)
        {
            _boardGenerator.FillEmptyTiles(Board, availableTiles);
        }

        public void SwapTiles(Vector3Int first, Vector3Int second)
        {
            Board.SwapTiles(first, second);
        }

        public void RemoveTiles(List<Match> allMatches)
        {
            foreach (var match in allMatches)
            {
                RemoveTiles(match);
            }
        }

        private void RemoveTiles(Match match)
        {
            var direction = match.To - match.From;
            direction.x = Math.Sign(direction.x);
            direction.y = Math.Sign(direction.y);

            var currentTile = match.From;
            while (currentTile != match.To)
            {
                Board.RemoveTile(currentTile);
                currentTile += direction;
            }
            Board.RemoveTile(match.To);
        }

        public void FallTiles()
        {
            var width = Board.Tiles.GetLength(0);
            for (var column = 0; column < width; column++)
            {
                FallTilesInColumn(column);
            }
        }

        private void FallTilesInColumn(int column)
        {
            var tiles = Board.Tiles;
            var height = tiles.GetLength(1);

            var countEmptyTiles = 0;

            for (var i = height - 1; i >= 0; i--)
            {
                var isEmptyTile = Board.Tiles[column, i] == null;
                if (isEmptyTile)
                {
                    countEmptyTiles++;
                }
                else if (countEmptyTiles > 0)
                {
                    var currentTilePosition = new Vector3Int(column, i);
                    var emptyPlace = new Vector3Int(column, i + countEmptyTiles);
                    Board.SwapTiles(emptyPlace, currentTilePosition);
                }
            }
        }
    }
}