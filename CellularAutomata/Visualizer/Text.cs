using SDL2;

namespace CellularAutomata.Visualizer;

public class Text : GameObject
{

    public Text(IntPtr renderer,string fontpath, string text, int size, SDL.SDL_Color color)
    {
        IntPtr font = SDL_ttf.TTF_OpenFont(fontpath, size);
        _surface = SDL_ttf.TTF_RenderText_Blended(font, text, color);
        _texture = SDL.SDL_CreateTextureFromSurface(renderer, _surface);
        SDL_ttf.TTF_SizeText(font, text, out _w, out _h);
    }

    ~Text()
    {
        SDL.SDL_FreeSurface(_surface);
        SDL.SDL_DestroyTexture(_texture);
    }
}