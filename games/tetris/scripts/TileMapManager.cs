using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public static class TileMapManager
{
    private static TileMap _tileMap;
    private static Vector2I _gameAreaCornerTopLeft;
    private static Vector2I _gameAreaCornerBottomRight;
    private static Vector2I _wallsCornerTopLeft;
    private static Vector2I _wallsCornerBottomRight;
    private static int _rowCount;
    private static int _columnCount;

    public static void SetTileMap(TileMap tileMap, int rows, int columns)
    {
        _tileMap = tileMap;
        _rowCount = rows;
        _columnCount = columns;

        // Get center tile
        Vector2 viewportSize = _tileMap.GetViewportRect().Size;
        Vector2I viewportCenter = new Vector2I((int)viewportSize.X/2, (int)viewportSize.Y/2);
        Vector2I tileSize = _tileMap.TileSet.TileSize;
        Vector2I centerTile = new Vector2I((viewportCenter.X - (viewportCenter.X % tileSize.X)) / tileSize.X, (viewportCenter.Y - (viewportCenter.Y % tileSize.Y)) / tileSize.Y);

        // Get game area coordinates
        int centerRow = rows/2;
        int centerColumn = columns/2;
        _gameAreaCornerTopLeft = new Vector2I(centerTile.X - centerColumn, centerTile.Y - centerRow);
        _gameAreaCornerBottomRight = new Vector2I(centerTile.X + centerColumn, centerTile.Y + centerRow);
        _wallsCornerTopLeft = new Vector2I(_gameAreaCornerTopLeft.X - 1, _gameAreaCornerTopLeft.Y - 1);
        _wallsCornerBottomRight = new Vector2I(_gameAreaCornerBottomRight.X + 1, _gameAreaCornerBottomRight.Y + 1);
    }

    public static void DrawGameArea()
    {
        if(_tileMap != null)
        {
            // Draw game area
            for(int x = _wallsCornerTopLeft.X; x < _wallsCornerBottomRight.X; x++)
            {
                for(int y = _wallsCornerTopLeft.Y; y < _wallsCornerBottomRight.Y; y++)
                {
                    if(
                        y == _wallsCornerTopLeft.Y || 
                        y == _wallsCornerBottomRight.Y-1 ||
                        x == _wallsCornerTopLeft.X ||
                        x == _wallsCornerBottomRight.X-1
                    ){
                        _tileMap.SetCell(0, new Vector2I(x, y), _tileMap.TileSet.GetSourceId(0), new Vector2I(7,0));
                    }
                }
            }
        }
    }

    private static void ClearBlocks()
    {
        for(int x = _gameAreaCornerTopLeft.X; x < _gameAreaCornerBottomRight.X; x++)
        {
            for(int y = _gameAreaCornerTopLeft.Y; y < _gameAreaCornerBottomRight.Y; y++)
            {
                _tileMap.EraseCell(0, new Vector2I(x,y));
            }
        }
    }

    public static void DrawBlocks(int[,] board)
    {
        if(_tileMap != null)
        {
            ClearBlocks();
            for(int x = 0; x < _columnCount; x++)
            {
                for(int y = 0; y < _rowCount; y++)
                {
                    if(board[x, y] != 0)
                    {
                        _tileMap.SetCell(0, new Vector2I(_gameAreaCornerTopLeft.X + x, _gameAreaCornerTopLeft.Y + y), _tileMap.TileSet.GetSourceId(0), new Vector2I(board[x, y]-1, 0));
                    }
                }
            }
        }
    }
}
