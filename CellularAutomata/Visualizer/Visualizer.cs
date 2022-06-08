using CellularAutomata.Automatas;
using SDL2;

namespace CellularAutomata.Visualizer;

public class Visualizer
{
    private Configuration _config;
    private int _delaycounter;
    private SDL.SDL_Event _e;
    private byte[][]? _colormap;
    private CellAutomata? _automata;

    private readonly IntPtr _window;
    private readonly IntPtr _renderer;

    private List<Button> _buttons = new();
    private Button? _focusedButton;
    private bool _quit;


    public string? Error { get; }

    public Visualizer()
    {
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
    }

    private void RenderText(Text txt, int x, int y)
    {
        SDL.SDL_Rect rect = new SDL.SDL_Rect() {h = txt.H, w = txt.W, x = x, y = y};
        SDL.SDL_RenderCopy(_renderer, txt.Texture, IntPtr.Zero, ref rect);
    }

    private void RenderButton(Button btn, int x, int y)
    {
        SDL.SDL_Rect rect = new SDL.SDL_Rect() {h = btn.H, w = btn.W, x = x, y = y};
        SDL.SDL_RenderCopy(_renderer, btn.Texture, IntPtr.Zero, ref rect);
    }

    private void MainMenu()
    {
        _buttons.Clear();
        SDL.SDL_Color clactive = new SDL.SDL_Color() {r = 111, g = 66, b = 245, a = 255};
        SDL.SDL_Color clinactive = new SDL.SDL_Color() {r = 183, g = 250, b = 111, a = 255};
        Button btnStart = new Button(_renderer, "sans.ttf", "Start", 65, clactive, clinactive);
        btnStart.X = _config.W / 2 - btnStart.W / 2;
        btnStart.Y = _config.H / 2 - btnStart.H / 2 - 70;
        btnStart.Action = ButtonAction.Start;
        _buttons.Add(btnStart);
        Button btnSettings = new Button(_renderer, "sans.ttf", "Settings", 65, clactive, clinactive);
        btnSettings.X = _config.W / 2 - btnSettings.W / 2;
        btnSettings.Y = _config.H / 2 - btnSettings.H / 2 + 70;
        btnSettings.Action = ButtonAction.Settings;
        _buttons.Add(btnSettings);
    }

    private void StartMenu()
    {
        _buttons.Clear();
        SDL.SDL_Color clactive = new SDL.SDL_Color() {r = 111, g = 66, b = 245, a = 255};
        SDL.SDL_Color clinactive = new SDL.SDL_Color() {r = 183, g = 250, b = 111, a = 255};
        int y = 50;
        int x = 50;
        foreach (Type automata in Constants.allautomatas)
        {
            string description = (string) automata.GetProperty("Description")!.GetValue(null)!;
            Button btn = new Button(_renderer, "sans.ttf", description, 35, clactive, clinactive)
            {
                X = x,
                Y = y,
                Action = ButtonAction.StartAutomata,
                Automata = automata
            };
            y += btn.H;
            _buttons.Add(btn);
        }
    }

    private void StartAutomata(Type automata)
    {
        _buttons.Clear();
        _automata = (CellAutomata) Activator.CreateInstance(automata, _config.H, _config.W)!;
        _automata.Restart();
        _colormap = _automata.GetColors();
    }

    private void SettingsMenu()
    {
    }

    private bool CheckFocusButton(Button btn, int x, int y)
    {
        if (x > btn.X && x < (btn.X + btn.W) && y > btn.Y && y < btn.Y + btn.H)
            return true;
        return false;
    }

    private void HandleInput()
    {
        //make focused button is active
        if (SDL.SDL_GetMouseFocus() == _window)
        {
            int mouseX, mouseY;
            SDL.SDL_GetMouseState(out mouseX, out mouseY);

            if (_focusedButton != null && !CheckFocusButton(_focusedButton, mouseX, mouseY))
            {
                _focusedButton.MakeTexture(false);
                _focusedButton = null;
            }

            if (_focusedButton == null)
            {
                foreach (var btn in _buttons)
                {
                    if (CheckFocusButton(btn, mouseX, mouseY))
                    {
                        _focusedButton = btn;
                        _focusedButton.MakeTexture(true);
                    }
                }
            }
        }

        //handle button presses
        while (SDL.SDL_PollEvent(out _e) != 0)
        {
            switch (_e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    _quit = true;
                    break;
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    switch (_e.key.keysym.sym)
                    {
                        case (SDL.SDL_Keycode.SDLK_ESCAPE):
                            _quit = true;
                            break;
                    }

                    break;
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    if (_e.button.button == SDL.SDL_BUTTON_LEFT && _focusedButton != null)
                    {
                        switch (_focusedButton.Action)
                        {
                            case ButtonAction.Start:
                                StartMenu();
                                break;
                            case ButtonAction.Settings:
                                SettingsMenu();
                                break;
                            case ButtonAction.MainMenu:
                                MainMenu();
                                break;
                            case ButtonAction.StartAutomata:
                                StartAutomata(_focusedButton.Automata!);
                                break;
                        }
                    }

                    break;
            }
        }
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

        foreach (var btn in _buttons)
        {
            RenderButton(btn, btn.X, btn.Y);
        }

        SDL.SDL_RenderPresent(_renderer);
    }

    public void Init()
    {
        MainMenu();

        while (!_quit)
        {
            Render();
            SDL.SDL_Delay(1);
            HandleInput();
            _automata?.Step();
        }
    }

    ~Visualizer()
    {
        _buttons.Clear();
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }
}