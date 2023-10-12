using System.Collections.Generic;
using Match3;
using Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class BoardGenerator
    {
        private readonly MatchDetectionService _matchDetectionService;

        public BoardGenerator(MatchDetectionService matchDetectionService)
        {
            _matchDetectionService = matchDetectionService;
        }

        public Board Generate(Vector2Int size, TileSettings[] availableTiles)
        {
            var tiles = new TileSettings[size.x, size.y];
            var board = new Board(tiles);
            
            GenerateMatchFreeBoard(board, availableTiles);

            return board;
        }

        public void FillEmptyTiles(Board board, TileSettings[] availableTiles)
        {
            GenerateMatchFreeBoard(board, availableTiles, ignoreNotEmpty:true);
        }

        private void GenerateMatchFreeBoard(Board board, 
            TileSettings[] availableTiles, 
            bool ignoreNotEmpty = false,
            bool strictMode = false)
        {
            var tiles = board.Tiles;
            var tilesForGeneration = new List<TileSettings>(availableTiles.Length);
            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                for (var y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null && ignoreNotEmpty)
                    {
                        continue;
                    }
                    
                    tilesForGeneration.Clear();
                    tilesForGeneration.AddRange(availableTiles);
                    SetRandomTileWithoutMatches(board, x, y, tilesForGeneration, strictMode);
                }
            }
        }

        private void SetRandomTileWithoutMatches(Board board, int x, int y, List<TileSettings> availableTiles, bool strict)
        {
            var randomIndex = Random.Range(0, availableTiles.Count);
            var randomTile = availableTiles[randomIndex];
            
            while (availableTiles.Count > 0)
            {
                randomIndex = Random.Range(0, availableTiles.Count);
                board.Tiles[x, y] = availableTiles[randomIndex];

                if (!_matchDetectionService.HasMatchAt(board, new Vector3Int(x, y)))
                {
                    return;
                }
                
                availableTiles.RemoveAt(randomIndex);
            }

            if (strict)
            {
                throw new BoardGenerationException("Failed to generate a match-free board.");
            }

            board.Tiles[x, y] = randomTile;
        }
    }
}