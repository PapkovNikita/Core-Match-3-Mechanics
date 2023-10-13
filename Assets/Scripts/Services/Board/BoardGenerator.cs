using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Services.Board
{
    public class BoardGenerator : IBoardGenerator
    {
        private readonly MatchDetectionService _matchDetectionService;

        public BoardGenerator(MatchDetectionService matchDetectionService)
        {
            _matchDetectionService = matchDetectionService;
        }

        public Board Generate(Vector2Int size, TileType[] availableTiles)
        {
            var board = new Board(size);

            GenerateMatchFreeBoard(board, availableTiles);

            return board;
        }

        public void FillEmptyTiles(Board board, TileType[] availableTiles)
        {
            GenerateMatchFreeBoard(board, availableTiles, ignoreNotEmpty: true);
        }

        private void GenerateMatchFreeBoard(Board board,
            TileType[] availableTiles,
            bool ignoreNotEmpty = false,
            bool strictMode = false)
        {
            var size = board.GetSize();
            var tilesForGeneration = new List<TileType>(availableTiles.Length);
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    if (!board.GetTileModel(x, y).IsRemoved && ignoreNotEmpty)
                    {
                        continue;
                    }

                    tilesForGeneration.Clear();
                    tilesForGeneration.AddRange(availableTiles);
                    SetRandomTileWithoutMatches(board, x, y, tilesForGeneration, strictMode);
                }
            }
        }

        private void SetRandomTileWithoutMatches(Board board, int x, int y, IList<TileType> availableTiles, bool strict)
        {
            while (availableTiles.Count > 0)
            {
                var randomIndex = Random.Range(0, availableTiles.Count);
                var randomTile = availableTiles[randomIndex];
                
                board.Set(randomTile, x, y);

                if (!_matchDetectionService.HasMatchAt(board, new Vector2Int(x, y)))
                {
                    return;
                }

                availableTiles.RemoveAt(randomIndex);
            }

            if (strict)
            {
                throw new BoardGenerationException("Failed to generate a match-free board.");
            }
        }
    }
}