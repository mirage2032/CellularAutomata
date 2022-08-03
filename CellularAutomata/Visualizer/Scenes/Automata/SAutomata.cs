using CellularAutomata.Automatas;
using SDL2;

namespace CellularAutomata.Visualizer.Scenes.Automata;

public class SAutomata : Scene
{
    private int _w, _h;
    private CellAutomata _automata;
    private byte[][] _colors;
    private Configuration _conf;
    public SAutomata(IntPtr window, IntPtr renderer, Type automataType, ref Configuration conf)
    {
        SDL.SDL_GetWindowSize(window, out _w, out _h);
        _conf = conf;
        _window = window;
        _renderer = renderer;
        _automata = (CellAutomata)Activator.CreateInstance(automataType, _h/conf.Scale, _w/conf.Scale)!;
        _automata.Restart();
        _colors = _automata.GetColors();
    }

    public override void Tick()
    {
        _automata.Step();
    }

    public override void RenderDraw()
    {

        for (int y = 0; y < _automata.Height; y++)
        {
            for (int x = 0; x < _automata.Width; x++)
            {
                CellType cell = _automata.GetCell(x, y);
                for (int i = 0; i < _conf.Scale; i++)
                {
                    for (int j = 0; j < _conf.Scale; j++)
                    {
                        if (cell != CellType.Dead)
                        {
                            byte[] color = _colors[(int) cell];
                            SDL.SDL_SetRenderDrawColor(_renderer, color[0], color[1], color[2], 255);
                            SDL.SDL_RenderDrawPoint(_renderer, x*_conf.Scale+i, y*_conf.Scale+j);
                        }
                    }
                }
            }
        }

        base.RenderDraw();
    }

    ~SAutomata()
    {
        SDL.SDL_RenderSetScale(_renderer, 1, 1);
    }
}