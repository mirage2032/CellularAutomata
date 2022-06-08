using SDL2;

namespace CellularAutomata.Visualizer;

public class Text : GameObject
{
    private IntPtr _surface;
    public readonly IntPtr Texture;
    public readonly int W, H;

    public Text(IntPtr renderer,string fontpath, string text, int size, SDL.SDL_Color color)
    {
        IntPtr font = SDL_ttf.TTF_OpenFont(fontpath, size);
        _surface = SDL_ttf.TTF_RenderText_Blended(font, text, color);
        Texture = SDL.SDL_CreateTextureFromSurface(renderer, _surface);
        SDL_ttf.TTF_SizeText(font, text, out W, out H);
    }

    ~Text()
    {
        SDL.SDL_FreeSurface(_surface);
        SDL.SDL_DestroyTexture(Texture);
    }
}