using Godot;
using Godot.Collections;

public class Tetromino
{
    public readonly EnumTetrominos Type;
    public Vector2I TileSetPosition { get; private set; }
    public Array<Array<Vector2I>> Rotations { get; private set; }
    public Vector2I Position { get; set; }
    public Array<Vector2I> Rotation { get; set; }
    public static Vector2I SpawnPosition = new Vector2I(19, -4);

    public Tetromino(EnumTetrominos type, Vector2I tileSetPosition, Array<Array<Vector2I>> rotations)
    {
        Type = type;
        TileSetPosition = tileSetPosition;
        Rotations = rotations;
        Position = SpawnPosition;
        Rotation = Rotations[0];
    }
}
