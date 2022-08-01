namespace CellularAutomata.Visualizer.Scenes.Automata;

public class SAutomata : Scene
{
    public SAutomata(IntPtr window, IntPtr renderer, Type automataType)
    {
        _window = window;
        _renderer = renderer;
        _gameObjects.Add(new GOAutomata(renderer,window,automataType));
        
    }
}