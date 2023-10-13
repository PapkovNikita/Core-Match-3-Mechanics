using System;
using Services.Board;
using Settings;
using UnityEngine;

public class FakeBoardGenerator : IBoardGenerator
{
    private TileType[] _availableTiles;
    private TileType[,] _tiles;
    private int _currentRowIndex;

    public FakeBoardGenerator Setup(int width, int height, TileType[] availableTilesArray)
    {
        _tiles = new TileType[width, height];
        _availableTiles = availableTilesArray;
        _currentRowIndex = 0;
        return this;
    }

    public FakeBoardGenerator SetupNextRow(params int?[] tileIndices)
    {
        if (tileIndices.Length != _tiles.GetLength(0) || _currentRowIndex >= _tiles.GetLength(1))
        {
            throw new InvalidOperationException("Invalid row setup or max rows exceeded.");
        }

        for (int colIndex = 0; colIndex < tileIndices.Length; colIndex++)
        {
            if (tileIndices[colIndex].HasValue)
            {
                var tileIndex = tileIndices[colIndex].Value;
                if (tileIndex < 0 || tileIndex >= _availableTiles.Length)
                {
                    throw new InvalidOperationException($"Invalid tile index in the row ({tileIndex}).");
                }

                _tiles[colIndex, _currentRowIndex] = _availableTiles[tileIndex];
            }
        }

        _currentRowIndex++;
        return this;
    }

    public Board Generate()
    {
        var realBoardSize = new Vector2Int(_tiles.GetLength(0), _tiles.GetLength(1));
        var board = new Board(realBoardSize);

        for (var x = 0; x < _tiles.GetLength(0); x++)
        {
            for (var y = 0; y < _tiles.GetLength(1); y++)
            {
                var type = _tiles[x, y];
                board.SetTile(type, x, y);
            }
        }

        return board;
    }

    public Board Generate(Vector2Int size, TileType[] availableTiles)
    {
        return Generate();
    }

    public void FillEmptyTiles(Board board, TileType[] availableTiles)
    {
        throw new NotImplementedException();
    }
}