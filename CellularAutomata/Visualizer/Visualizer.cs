using CellularAutomata.Automatas;
using SDL2;

namespace CellularAutomata.Visualizer;

public class Visualizer
{
    private Configuration _config;

    private readonly IntPtr _window;
    private readonly IntPtr _renderer;

    private Dictionary<String, Type> _allscenes;
    private Scene? _scene;
    private bool _quit;
    public string? Error { get; }

    public Visualizer(Dictionary<String, Type> allscenes)
    {
        _allscenes = allscenes;
        _config = ConfigParser.GetConfig("config.cfg",
            new Configuration()
                {Delay = Constants.Delay, H = Constants.Height, W = Constants.Width, Scale = Constants.Scale});
        if (SDL.SDL_WasInit(SDL.SDL_INIT_VIDEO) == 0)
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("Unable to initialize SDL");
                Error = "SDL INIT";
            }

        if (SDL_ttf.TTF_WasInit() == 0)
            if (SDL_ttf.TTF_Init() < 0)
            {
                Console.WriteLine("Unable to initialize SDL_TTF");
                Error = "SDL_TTF INIT";
            }

        _window = SDL.SDL_CreateWindow("Visualiser", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
            _config.W * _config.Scale, _config.H * _config.Scale, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
        if (_window == IntPtr.Zero)
        {
            Console.WriteLine("Unable to create window");
            Error = "SDL CreateWindow";
        }

        _renderer = SDL.SDL_CreateRenderer(_window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
        if (_window == IntPtr.Zero)
        {
            Console.WriteLine("Unable to create renderer");
            Error = "SDL CreateRenderer";
        }

        SDL.SDL_RenderSetScale(_renderer, _config.Scale, _config.Scale);
        _scene = (Scene?) Activator.CreateInstance(_allscenes["Start"], _window, _renderer);
    }

    /*private void StartMenu()
    {
        _gameObjects.Clear();
        SDL.SDL_Color clactive = new SDL.SDL_Color() {r = 111, g = 66, b = 245, a = 255};
        SDL.SDL_Color clinactive = new SDL.SDL_Color() {r = 183, g = 250, b = 111, a = 255};
        int y = 50;
        int x = 50;
        for (int i = 0; i < Constants.allautomatas.Length; i++)
        {
            string description = (string) Constants.allautomatas[i].GetProperty("Description")!.GetValue(null)!;
            Button btn = new Button(_renderer, "/home/alx/RiderProjects/CellularAutomata/CellularAutomata/bin/Release/net6.0/sans.ttf", description, 35, clactive, clinactive)
            {
                X = x,
                Y = y,
                Action = new Action(),
                index = i
            };
            y += btn.H();
            _gameObjects.Add(btn);
        }
    }

    private void StartAutomata(Type automata)
    {
        _gameObjects.Clear();
        _automata = (CellAutomata) Activator.CreateInstance(automata, _config.H, _config.W)!;
        _automata.Restart();
        _colormap = _automata.GetColors();
    }


    private void Render()
    {
        if (_automata != null)
        {
            SDL.SDL_SetRenderDrawColor(_renderer, _colormap![0][0], _colormap[0][1], _colormap[0][2], 255);
            SDL.SDL_RenderClear(_renderer);
            for (int y = 0; y < _automata.Height; y++)
            {
                for (int x = 0; x < _automata.Width; x++)
                {
                    CellType cell = _automata.GetCell(x, y);
                    if (cell != CellType.Dead)
                    {
                        byte[] color = _colormap[(int) cell];
                        SDL.SDL_SetRenderDrawColor(_renderer, color[0], color[1], color[2], 255);
                        SDL.SDL_RenderDrawPoint(_renderer, x, y);
                    }
                }
            }
        }
        else
        {
            SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(_renderer);
        }

        foreach (var btn in _gameObjects)
        {
            RenderGameObject(btn, btn.X, btn.Y);
            Console.WriteLine(SDL.SDL_GetError());
        }

        SDL.SDL_RenderPresent(_renderer);
    }
    */

    public void Init()
    {
        while (_scene!=null && !_quit)
        {
            List<Action> engineAction = _scene.HandleInput();
            _scene.Tick();
            _scene.Render();
            SDL.SDL_Delay(1);
        }
    }

    ~Visualizer()
    {
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }
}