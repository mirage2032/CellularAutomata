using SDL2;

namespace CellularAutomata.Visualizer.Scenes;

public class SMainMenu : Scene
{
    public SMainMenu(IntPtr window, IntPtr renderer)
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
                35, clactive, clinactive, new Action(ActionType.ChangeScene, "Automata", Constants.allautomatas[i]))
            {
                    X=x,
                    Y=y
            };
            _gameObjects.Add(btn);
            y += btn.H();
        }
        {
            //settings button
            Button btn = new Button(renderer, "sans.ttf", "Settings", 55, clactive, clinactive,
                new Action(ActionType.ChangeScene,"Settings"))
            {
                X = x, Y = y
            };
            _gameObjects.Add(btn);
            y += btn.H();
        }
        {//exit button
            y += 50;
            SDL.SDL_Color exit_clactive = new SDL.SDL_Color() {r = 255, g = 49, b = 25, a = 255};
            SDL.SDL_GetWindowSize(window,out int w, out int h);
            Button btn = new Button(renderer, "sans.ttf", "Exit", 55, exit_clactive, clinactive,
                new Action(ActionType.Quit));
            _gameObjects.Add(btn);
            btn.X = w-btn.W()-75;
            btn.Y = h - btn.H()-75;
            y += btn.H();
        }
    }

}