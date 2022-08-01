namespace CellularAutomata.Visualizer;

public enum ActionType
{
    None,
    ChangeAutomata,
    ChangeScene,
    Quit
}

public struct Action
{
    public Type? AutomataType { get; }
    public ActionType Type { get; }
    
    public Scene? Scene { get; }
    public List<string> Data { get; }

    public Action(ActionType type, Type automatatype, List<string> data)
    {
        AutomataType = automatatype;
        Type = type;
        Scene = null;
        Data = data;
    }

    public Action(ActionType type, List<string> data)
    {
        AutomataType = null;
        Type = type;
        Scene = null;
        Data = data;
    }

    public Action(ActionType type)
    {
        AutomataType = null;
        Type = type;
        Scene = null;
        Data = new List<string> { };
    }
}

public abstract class GameObject
{
    protected IntPtr _surface;
    public int X;
    public int Y;
    protected IntPtr _texture;
    protected int _w;

    protected int _h;
    public int Z = 0;
    public int W() => _w;

    public int H() => _h;
    public List<GameObject> children = new List<GameObject>();

    public virtual IntPtr GenTexture()
    {
        return _texture;
    }
    
    public virtual void OnHover()
    {
    }

    public virtual void OnUnhover()
    {
    }

    public virtual Action? OnClick()
    {
        return null;
    }

    public virtual void Step()
    {
        
    }
    
}