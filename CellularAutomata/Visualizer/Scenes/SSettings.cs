using SDL2;

namespace CellularAutomata.Visualizer.Scenes;

public class SSettings : Scene
{
    private Configuration _conf;

    public void Step(ref int val, int size, bool reverse, int min, int max)
    {
        if (reverse)
            val += size;
        else
            val -= size;
        if (val <= min)
            val = 0;
        if (val >= max)
            val = max;
    }

    public SSettings(IntPtr window, IntPtr renderer, ref Configuration conf)
    {
        _conf = conf;
        _window = window;
        _renderer = renderer;
        SDL.SDL_Color color_val = new SDL.SDL_Color() {r = 111, g = 66, b = 245, a = 255};
        SDL.SDL_Color color_txt = new SDL.SDL_Color() {r = 143, g = 20, b = 111, a = 255};
        int y = 50;
        int x = 50;
        Text txt_width = new Text(renderer,
            "sans.ttf", "Width", 35, color_txt)
        {
            X = x,
            Y = y
        };
        _gameObjects.Add(txt_width);
        
        Text txt_width_value = new Text(renderer,
            "sans.ttf", conf.W.ToString(), 35, color_val)
        {
            X = x+txt_width.W()+20,
            Y = y
        };
        _gameObjects.Add(txt_width_value);
        y += txt_width.H();
        
        Text txt_height = new Text(renderer,
            "sans.ttf", "Height", 35, color_txt)
        {
            X = x,
            Y = y
        };
        
        _gameObjects.Add(txt_height);
        
        Text txt_height_value = new Text(renderer,
            "sans.ttf", conf.H.ToString(), 35, color_val)
        {
            X = x+txt_height.W()+20,
            Y = y
        };
        _gameObjects.Add(txt_height_value);
        
        y += txt_height.H();
        
        Text txt_scale = new Text(renderer,
            "sans.ttf", "Scale", 35, color_txt)
        {
            X = x,
            Y = y
        };
        
        _gameObjects.Add(txt_scale);
        
        Text txt_scale_value = new Text(renderer,
            "sans.ttf", conf.Scale.ToString(), 35, color_val)
        {
            X = x+txt_scale.W()+20,
            Y = y
        };
        _gameObjects.Add(txt_scale_value);
        
        y += txt_height.H();
        
        
        Text txt_delay = new Text(renderer,
            "sans.ttf", "Delay", 35, color_txt)
        {
            X = x,
            Y = y
        };
        
        _gameObjects.Add(txt_delay);
        
        Text txt_delay_value = new Text(renderer,
            "sans.ttf", conf.Delay.ToString(), 35, color_val)
        {
            X = x+txt_scale.W()+20,
            Y = y
        };
        _gameObjects.Add(txt_delay_value);
        
        y += txt_height.H();
    }
}