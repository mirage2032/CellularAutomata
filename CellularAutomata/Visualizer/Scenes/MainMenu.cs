using SDL2;

namespace CellularAutomata.Visualizer.Scenes;

public class MainMenu : Scene
{
    public MainMenu(IntPtr window, IntPtr renderer)
    {
        _window = window;
        _renderer = renderer;
        SDL.SDL_Color clactive = new SDL.SDL_Color() {r = 111, g = 66, b = 245, a = 255};
        SDL.SDL_Color clinactive = new SDL.SDL_Color() {r = 183, g = 250, b = 111, a = 255};
        int y = 50;
        int x = 50;
        for (int i = 0; i < Constants.allautomatas.Length; i++)
        {
            string description = (string) Constants.allautomatas[i].GetProperty("Description")!.GetValue(null)!;
            Button btn = new Button(renderer,
                "sans.ttf", description,
                35, clactive, clinactive, new Action(ActionType.ChangeScene, "Automata", new List<string> {description }))
            {
                    X=x,
                    Y=y
            };
            y += btn.H();
            _gameObjects.Add(btn);
        }
    }

    public override List<Action> HandleInput()
    {
        List<Action> actions = new();
        OnHoverUpdate();
        while (SDL.SDL_PollEvent(out var e) != 0)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    actions.Add(new Action(ActionType.Quit));
                    break;
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    switch (e.key.keysym.sym)
                    {
                        case (SDL.SDL_Keycode.SDLK_ESCAPE):
                            actions.Add(new Action(ActionType.Quit));
                            break;
                    }

                    break;
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    if (e.button.button == SDL.SDL_BUTTON_LEFT && _onHoverActive != null)
                    {
                        actions.Add(_onHoverActive.OnClick() ?? new Action(ActionType.None));
                    }

                    break;
            }
        }

        return actions;
    }
 
}