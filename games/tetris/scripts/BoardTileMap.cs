using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BoardTileMap : TileMap
{
    private readonly int _rowCount;
    private readonly int _columnCount;
    private readonly int[,] _board;
    private int _level;
    static double time = 0;
    private static Random rnd = new Random();
    private Tetromino _activeTetromino;
	private Tetromino _nextTetromino;

    private List<Tetromino> _tetrominos = new List<Tetromino>
    {
        new Tetromino(
            EnumTetrominos.I,
            new Vector2I(6, 0),
            new Array<Array<Vector2I>>
            {
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(0, 1),
                    new Vector2I(0, 2),
                    new Vector2I(0, 3)
                },
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(0, 1),
                    new Vector2I(0, 2),
                    new Vector2I(0, 3)
                }
            }
        ),

        new Tetromino(
            EnumTetrominos.O,
            new Vector2I(0, 0),
            new Array<Array<Vector2I>>
            {
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(1, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1)
                }
            }
        ),

        new Tetromino(
            EnumTetrominos.T,
            new Vector2I(1, 0),
            new Array<Array<Vector2I>>
            {
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(1, 0),
                    new Vector2I(2, 0),
                    new Vector2I(1, 1)
                },
                new Array<Vector2I>
                {
                    new Vector2I(1, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1),
                    new Vector2I(1, 2)
                },
                new Array<Vector2I>
                {
                    new Vector2I(1, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1),
                    new Vector2I(2, 1)
                },
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1),
                    new Vector2I(0, 2)
                }
            }
        ),

        new Tetromino(
            EnumTetrominos.L,
            new Vector2I(5, 0),
            new Array<Array<Vector2I>>
            {
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(0, 1),
                    new Vector2I(0, 2),
                    new Vector2I(1, 2)
                },
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(1, 0),
                    new Vector2I(2, 0),
                    new Vector2I(0, 1)
                },
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(1, 0),
                    new Vector2I(1, 1),
                    new Vector2I(1, 2)
                },
                new Array<Vector2I>
                {
                    new Vector2I(2, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1),
                    new Vector2I(2, 1)
                }
            }
        ),

        new Tetromino(
            EnumTetrominos.J,
            new Vector2I(3, 0),
            new Array<Array<Vector2I>>
            {
                new Array<Vector2I>
                {
                    new Vector2I(1, 0),
                    new Vector2I(1, 1),
                    new Vector2I(0, 2),
                    new Vector2I(1, 2)
                },
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1),
                    new Vector2I(2, 1)
                },
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(1, 0),
                    new Vector2I(0, 1),
                    new Vector2I(0, 2)
                },
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(1, 0),
                    new Vector2I(2, 0),
                    new Vector2I(2 ,1)
                }
            }
        ),

        new Tetromino(
            EnumTetrominos.S,
            new Vector2I(2, 0),
            new Array<Array<Vector2I>>
            {
                new Array<Vector2I>
                {
                    new Vector2I(1, 0),
                    new Vector2I(2, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1)
                },
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1),
                    new Vector2I(1, 2)
                }
            }
        ),

        new Tetromino(
            EnumTetrominos.Z,
            new Vector2I(4, 0),
            new Array<Array<Vector2I>>
            {
                new Array<Vector2I>
                {
                    new Vector2I(0, 0),
                    new Vector2I(1, 0),
                    new Vector2I(1, 1),
                    new Vector2I(2, 1)
                },
                new Array<Vector2I>
                {
                    new Vector2I(1, 0),
                    new Vector2I(0, 1),
                    new Vector2I(1, 1),
                    new Vector2I(0, 2)
                }
            }
        )
    };

    public BoardTileMap()
    {
        this._rowCount = 20;
        this._columnCount = 10;
        this._board = new int[_columnCount, _rowCount];
        this._level = 1;
        RollNextTetromino();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TileMapManager.SetTileMap(this, this._rowCount, this._columnCount);
        TileMapManager.DrawGameArea();
        _board[0,0] = 1;
        SpawnTetromino(_nextTetromino);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        time += delta;
        if (time > (1 / (_level)))
        {
            if (_activeTetromino == null)
            {
                SpawnTetromino(_nextTetromino);
                RollNextTetromino();
            }
            else
            {
                //MoveDown();

            }
            TileMapManager.DrawBlocks(_board);
            time = 0;
        }
    }

    private void RollNextTetromino()
    {
        _nextTetromino = _tetrominos[rnd.Next(_tetrominos.Count)];
    }

    private void SpawnTetromino(Tetromino tetromino)
    {
        _activeTetromino = _nextTetromino;

		//this.SetCell(0, ActiveTetromino.Position + coord, this.TileSet.GetSourceId(0), ActiveTetromino.TileSetPosition);
        //SetBoardCell(ActiveTetromino.GetBoardPosition() + coord, ActiveTetromino.TileSetPosition.X + 1);
	}
}
