using CellularAutomata.Automatas;

namespace CellularAutomata.Visualizer.Scenes.Automata;

public class GOAutomata : GameObject
{
    private CellAutomata _automata;
    public GOAutomata(IntPtr renderer, IntPtr window, Type automataType)
    {
        SDL2.SDL.SDL_GetWindowSize(window, out int w, out int h);
        _surface = SDL2.SDL.SDL_CreateRGBSurface(0, w, h, 32, 0, 255, 0, 255);
        _texture = SDL2.SDL.SDL_CreateTextureFromSurface(renderer, _surface);
    }

    public override void Step()
    {
        _automata.Step();
    }
}