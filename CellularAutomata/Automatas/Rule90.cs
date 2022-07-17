namespace CellularAutomata.Automatas;

class Rule90 : CellAutomata
{
    public new static string Description => "Rule 90";
    public override byte[][] GetColors()
    {
        byte[][] colors = new byte[2][];
        colors[0] = new [] {(byte)0, (byte)0, (byte)0};  //Dead cell
        colors[1] = new [] {(byte)255, (byte)255, (byte)255};   //Alive cell
        return colors;
    }
    
    public Rule90(int height, int width)
    {
        //Initialize Matrix
        Height = height;
        Width = width;
        Matrix = new CellType[height * width];
    }

    public override void Restart()
    {
        throw new NotImplementedException();
    }

    public override void Step()
    {
        throw new NotImplementedException();
    }
}