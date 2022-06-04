using SDL2;
using CellularAutomata.Automatas;

namespace CellularAutomata;

class CaVis
{
    public bool Quit { get; private set; }
    private SDL.SDL_Event _e;
    private readonly byte[][] _colors;
    private readonly CellAutomata _automata;
    private readonly IntPtr _window;
    private readonly IntPtr _renderer;

    public CaVis(CellAutomata automata, uint scale, byte[][] colors)
    {
        this._automata = automata;
        this._colors = colors;

        if (SDL.SDL_WasInit(SDL.SDL_INIT_VIDEO) == 0)
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("Unable to initialize SDL");
                return;
            }

        _window = SDL.SDL_CreateWindow("CA Visualiser", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
            (int) (automata.Width * scale), (int) (automata.Height * scale), SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
        if (_window == IntPtr.Zero)
            Console.WriteLine("Unable to create window");
        _renderer = SDL.SDL_CreateRenderer(_window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
        SDL.SDL_RenderSetScale(_renderer, scale, scale);
    }

    public void HandleInput()
    {
        while (SDL.SDL_PollEvent(out _e) != 0)
        {
            switch (_e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    Quit = true;
                    break;
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    switch (_e.key.keysym.sym)
                    {
                        case (SDL.SDL_Keycode.SDLK_ESCAPE):
                            Quit = true;
                            break;
                    }

                    break;
            }
        }
    }

    public void Start()
    {
        _automata.Restart();
    }

    public void Step()
    {
        _automata.Step();
    }

    public void Render()
    {
        SDL.SDL_SetRenderDrawColor(_renderer, _colors[0][0], _colors[0][1], _colors[0][2], 255);
        SDL.SDL_RenderClear(_renderer);
        for (int y = 0; y < _automata.Height; y++)
        {
            for (int x = 0; x < _automata.Width; x++)
            {
                CellType cell = _automata.GetCell(x, y);
                if (cell != CellType.Dead)
                {
                    byte[] color = _colors[(int) cell];
                    SDL.SDL_SetRenderDrawColor(_renderer, color[0], color[1], color[2], 255);
                    SDL.SDL_RenderDrawPoint(_renderer, x, y);
                }
            }
        }

        SDL.SDL_RenderPresent(_renderer);
    }

    ~CaVis()
    {
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }
}