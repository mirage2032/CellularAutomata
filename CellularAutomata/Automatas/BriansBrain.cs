namespace CellularAutomata.Automatas;

class BriansBrain : CellAutomata
{
    public new static string Description => "Brian's Brain";
    private CellType[] _tmpMatrix;
    private int[] _neighbourMatrix;
    public override byte[][] GetColors()
    {
        byte[][] colors = new byte[3][];
        colors[0] = new [] {(byte)0, (byte)0, (byte)0};      //Dead cell
        colors[1] = new [] {(byte)0, (byte)0, (byte)255};       //Dying cell
        colors[2] = new [] {(byte)255, (byte)255, (byte)255};   //Alive cell
        return colors;
    }
    
    public BriansBrain(uint height, uint width)
    {
        //Initialize Matrix
        Height = height;
        Width = width;
        Matrix = new CellType[height * width];
        _tmpMatrix = new CellType[height * width];
        _neighbourMatrix = new int[height * width];
    }
    private void CountNeighbours()
    {
        Array.Clear(_neighbourMatrix);
        //Iterate through all cells
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                //Don't increase neighbour's neighbor count if cell is dead
                if (Matrix[j + Width * i]==CellType.Dead || Matrix[j + Width * i]==CellType.A)
                    continue;
                //Iterate through each cell's neighbours
                for (int y = i - 1; y < i + 2; y++)
                {
                    for (int x = j - 1; x < j + 2; x++)
                    {
                        //Don't increase on neighbour count
                        if (y == i && x == j)
                            continue;
                        //Count cells outside the range as dead
                        if (y < 0 || x < 0 || y >= Height || x >= Width)
                        {
                            continue;
                        }

                        //Increase neighbor count
                        _neighbourMatrix[x + Width * y]++;
                    }
                }
            }
        }
    }
    public override void Restart()
    {
        Random rnd = new Random();
        for (int i = 0; i < Matrix.Length; i++)
        {
            Matrix[i] = (CellType) rnd.Next(3);
        }
    }

    public override void Step()
    {
        CountNeighbours();
        for (uint i = 0; i < Height; i++)
        {
            for (uint j = 0; j < Width; j++)
            {
                uint index = j + Width * i;
                switch (Matrix[index])
                {
                    case CellType.B:
                        _tmpMatrix[index] = CellType.A;
                        break;
                    case CellType.A:
                        _tmpMatrix[index] = CellType.Dead;
                        break;
                    case CellType.Dead:
                        if (_neighbourMatrix[index] == 2)
                            _tmpMatrix[index] = CellType.B;
                        break;
                }
            }
        }

        Matrix = _tmpMatrix;
    }
}
