using SDL2;

namespace CellularAutomata.Visualizer;

public enum ButtonAction
{
    None,
    Start,
    Settings,
    MainMenu,
    StartAutomata
}

public class Button
{
    private IntPtr _surface;
    public string Fontpath;
    public string Text;
    public int X = 0, Y = 0;
    public ButtonAction Action = ButtonAction.None;
    public Type? Automata;
    public int W, H;
    public int Size;
    public SDL.SDL_Color Clactive;
    public SDL.SDL_Color Clinactive;
    public IntPtr Renderer;
    public IntPtr Texture { get; private set; }

    public Button(IntPtr renderer, string fontpath, string text, int size, SDL.SDL_Color clactive,
        SDL.SDL_Color clinactive)
    {
        Renderer = renderer;
        Fontpath = fontpath;
        Text = text;
        Size = size;
        Clactive = clactive;
        Clinactive = clinactive;
        MakeTexture(false);
    }

    private void _freemem()
    {
        SDL.SDL_FreeSurface(_surface);
        SDL.SDL_DestroyTexture(Texture);
    }

    private void _createTxt(bool active)
    {
        IntPtr font = SDL_ttf.TTF_OpenFont(Fontpath, Size);
        _surface = SDL_ttf.TTF_RenderText_Blended(font, Text, active ? Clactive : Clinactive);
        Texture = SDL.SDL_CreateTextureFromSurface(Renderer, _surface);
        SDL_ttf.TTF_SizeText(font, Text, out W, out H);
    }

    public void MakeTexture(bool active)
    {
        _freemem();
        _createTxt(active);
    }

    ~Button()
    {
        _freemem();
    }
}