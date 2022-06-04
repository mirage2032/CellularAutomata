namespace CellularAutomata.Automatas;

class LangtonAnt : CellAutomata
{
    public new static string Description => "Langton's Ant";
    private bool _stopped;
    private enum Directions
    {
        Up,
        Right,
        Down,
        Left
    }

    private Directions _direction = Directions.Up;
    private uint _antX, _antY;

    public override byte[][] GetColors()
    {
        byte[][] colors = new byte[2][];
        colors[0] = new[] {(byte) 0, (byte) 0, (byte) 0}; //Dead cell
        colors[1] = new[] {(byte) 255, (byte) 0, (byte) 0}; //Alive cell
        return colors;
    }

    public LangtonAnt(uint height, uint width)
    {
        Height = height;
        Width = width;
        Matrix = new CellType[height * width];
    }

    public override void Restart()
    {
        Array.Clear(Matrix);
        _antX = Width/2;
        _antY = Height/2;
        Matrix[_antX + Width * _antY] = CellType.A;
    }

    public override void Step()
    {
        if (_stopped)
            return;
        switch (_direction)
        {
            case Directions.Up:
                _antX--;
                break;
            case Directions.Right:
                _antY++;
                break;
            case Directions.Down :
                _antX++;
                break;
            case Directions.Left:
                _antY--;
                break;
        }
        if (_antY < 0 || _antX < 0 || _antY >= Height || _antX >= Width)
        {
            _stopped = true;
            return;
        }
        
        if (Matrix[_antX + Width * _antY] == CellType.A)
        {
            _direction = (Directions) (((int) (_direction+1)%4 + 4)%4);
            Matrix[_antX + Width * _antY] = CellType.Dead;
        }
        else
        {
            _direction = (Directions) (((int) (_direction-1)%4 + 4)%4);
            Matrix[_antX + Width * _antY] = CellType.A;
        }
    }
}