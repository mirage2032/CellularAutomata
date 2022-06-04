namespace CellularAutomata.Automatas;

class Cgol : CellAutomata
{
    public new static string Description => "Conway's Game of Life";
    private CellType[] _tmpMatrix;
    private int[] _neighbourMatrix;
    
    public override byte[][] GetColors()
    {
        byte[][] colors = new byte[2][];
        colors[0] = new [] {(byte)24, (byte)24, (byte)24};  //Dead cell
        colors[1] = new [] {(byte)255, (byte)0, (byte)0};   //Alive cell
        return colors;
    }
    public Cgol(uint height, uint width)
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
                if (Matrix[j + Width * i]==CellType.Dead)
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
        //Iterate through all cells and give them a random value of true or false
        Random rnd = new Random();
        for (int i = 0; i < Matrix.Length; i++)
        {
            Matrix[i] = (CellType) rnd.Next(2);
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
                switch (_neighbourMatrix[index])
                {
                    case 2:
                        _tmpMatrix[index] = Matrix[index];
                        break;
                    case 3:
                        _tmpMatrix[index] = CellType.A;
                        break;
                    default:
                        _tmpMatrix[index] = CellType.Dead;
                        break;
                }
            }
        }

        Matrix = _tmpMatrix;
    }
}