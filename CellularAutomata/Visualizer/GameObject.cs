using System.Reflection;

namespace CellularAutomata.Visualizer;

public enum ActionType
{
    None,
    ChangeAutomata,
    ChangeScene,
    Callback,
    Quit
}
public delegate void CallbackDelegate();

public struct Action
{
    public Type? AutomataType { get; }
    public ActionType Type { get; }
    
    public string? Scene { get; }
    public List<string> Data { get; }
    
    public CallbackDelegate Callback { get; }
    

    public Action(ActionType type, Type automatatype, List<string> data)
    {
        AutomataType = automatatype;
        Type = type;
        Scene = null;
        Data = data;
        Callback = null;
    }

    public Action(ActionType type,string scene, Type automataType)
    {
        AutomataType = automataType;
        Type = type;
        Scene = scene;
        Data = null;
        Callback = null;
    }
    public Action(ActionType type,string scene)
    {
        AutomataType = null;
        Type = type;
        Scene = scene;
        Data = null;
        Callback = null;
    }


    public Action(ActionType type, CallbackDelegate callback)
    {
        AutomataType = null;
        Type = type;
        Callback = callback;
        Data = null;
        Scene = null;
    }

    public Action(ActionType type, List<string> data)
    {
        AutomataType = null;
        Type = type;
        Scene = null;
        Data = data;
        Callback = null;
    }

    public Action(ActionType type)
    {
        AutomataType = null;
        Type = type;
        Scene = null;
        Data = new List<string> { };
        Callback = null;
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