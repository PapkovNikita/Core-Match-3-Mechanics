using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Services.Board
{
    public class BoardService : IBoardProvider
    {
        public Board Board { get; private set; }
        
        private readonly IBoardGenerator _boardGenerator;

        public BoardService(IBoardGenerator boardGenerator)
        {
            _boardGenerator = boardGenerator;
        }

        public void GenerateBoard(ILevelSettings levelSettings)
        {
            Board = _boardGenerator.Generate(levelSettings.Size, levelSettings.AvailableTiles);
        }

        public void FillEmptyTiles(TileType[] availableTiles)
        {
            _boardGenerator.FillEmptyTiles(Board, availableTiles);
        }

        public void SwapTiles(Vector2Int first, Vector2Int second)
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
            var width = Board.GetSize().x;
            for (var column = 0; column < width; column++)
            {
                FallTilesInColumn(column);
            }
        }

        private void FallTilesInColumn(int column)
        {
            var height = Board.GetSize().y;
            var countEmptyTiles = 0;

            for (var i = height - 1; i >= 0; i--)
            {
                var isEmptyTile = Board.GetTileType(column, i) == null;
                if (isEmptyTile)
                {
                    countEmptyTiles++;
                }
                else if (countEmptyTiles > 0)
                {
                    var currentTilePosition = new Vector2Int(column, i);
                    var emptyPlace = new Vector2Int(column, i + countEmptyTiles);
                    Board.SwapTiles(emptyPlace, currentTilePosition);
                }
            }
        }
    }
}