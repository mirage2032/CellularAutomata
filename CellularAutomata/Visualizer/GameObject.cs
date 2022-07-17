namespace CellularAutomata.Visualizer;

public enum ActionType
{
    None,
    ChangeScene,
    Quit
}

public struct Action
{
    public String? SceneName { get; }
    public ActionType Type { get; }
    public List<string> Data { get; }

    public Action(ActionType type, String scenename, List<string> data)
    {
        SceneName = scenename;
        Type = type;
        Data = data;
    }

    public Action(ActionType type, List<string> data)
    {
        SceneName = "";
        Type = type;
        Data = data;
    }

    public Action(ActionType type)
    {
        SceneName = "";
        Type = type;
        Data = new List<string>{};
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
    public int W() => _w;

    public int H() => _h;

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
}