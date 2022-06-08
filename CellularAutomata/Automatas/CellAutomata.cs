namespace CellularAutomata.Automatas;

enum CellType
{
    Dead,
    A,
    B,
    C,
    D,
    E,
    F,
    G,
}

abstract class CellAutomata
{
    public static string Description => string.Empty;
    public int Height { get; protected init; }
    public int Width { get; protected init; }
    protected CellType[] Matrix = null!;

    public abstract byte[][] GetColors();
    public CellType GetCell(int x, int y) => Matrix[x + Width * y];
    public abstract void Restart();
    public abstract void Step();
}