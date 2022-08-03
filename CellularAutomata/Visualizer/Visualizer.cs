using System.Drawing;
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
        _config = new Configuration("config.cfg",
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
            _config.W, _config.H, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
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

        _scene = (Scene?) Activator.CreateInstance(_allscenes["Start"], _window, _renderer);
    }

    public void Init()
    {
        while (_scene != null && !_quit)
        {
            List<Action> engineActions = _scene.HandleInput();
            foreach (var action in engineActions)
            {
                if (action.Type == ActionType.Quit)
                    _quit = true;
                if (action.Type == ActionType.ChangeScene)
                    switch (action.Scene)
                    {
                        case "Automata":
                            _scene = (Scene?) Activator.CreateInstance(_allscenes[action.Scene], _window, _renderer,
                                action.AutomataType, _config);
                            break;
                        case "Settings":
                            _scene = (Scene?) Activator.CreateInstance(_allscenes[action.Scene], _window, _renderer,
                                _config);
                            break;
                        default:
                            _scene = (Scene?) Activator.CreateInstance(_allscenes[action.Scene!], _window, _renderer);
                            break;
                    }
            }
            _scene.Tick();
            _scene.Render();
            SDL.SDL_Delay(_config.Delay);
        }
    }

    ~Visualizer()
    {
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }
}