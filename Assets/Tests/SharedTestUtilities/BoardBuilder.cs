using System;
using Match3;
using Settings;

public class BoardBuilder
{
    private readonly TileSettings[] _availableTiles;
    private readonly TileSettings[,] _tiles;
    private int _currentRowIndex;
    private Board _board;

    public BoardBuilder(int width, int height, TileSettings[] availableTilesArray)
    {
        _tiles = new TileSettings[width, height];
        _availableTiles = availableTilesArray;
    }

    public BoardBuilder RewriteExistingBoard(Board board)
    {
        _board = board;
        return this;
    }
    
    public BoardBuilder SetupNextRow(params int?[] tileIndices)
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

    public Board Build()
    {
        if (_board == null)
        {
            return new Board(_tiles);
        }
        
        _board.Initialize(_tiles);
        return _board;
    }
}