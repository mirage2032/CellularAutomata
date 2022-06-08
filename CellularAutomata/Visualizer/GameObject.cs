namespace CellularAutomata.Visualizer;

public abstract class GameObject
{
    
    public readonly IntPtr Texture;
    public bool Changed { get; }
    public readonly int W, H;
    
}