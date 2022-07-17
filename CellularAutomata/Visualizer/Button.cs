using SDL2;

namespace CellularAutomata.Visualizer;

public class Button :GameObject
{
    private string Text;
    private Action? _action;
    private SDL.SDL_Color clactive;
    private SDL.SDL_Color _clinactive;
    private IntPtr _renderer;
    private IntPtr _font;
    private bool _active;
    private bool _changed = false;

    public Button(IntPtr renderer, string fontpath, string text, int fontsize, SDL.SDL_Color clactive,
        SDL.SDL_Color clinactive,Action? action=null)
    {
        _action = action;
        _renderer = renderer;
        Text = text;
        this.clactive = clactive;
        _clinactive = clinactive;
        _font = SDL_ttf.TTF_OpenFont(fontpath, fontsize);
        _createTxt(false);
    }

    private void _freemem()
    {
        SDL.SDL_FreeSurface(_surface);
        SDL.SDL_DestroyTexture(_texture);
    }

    private void _createTxt(bool active)
    {
        _surface = SDL_ttf.TTF_RenderText_Blended(_font, Text, active ? clactive : _clinactive);
        _texture = SDL.SDL_CreateTextureFromSurface(_renderer, _surface);
        SDL_ttf.TTF_SizeText(_font, Text, out _w, out _h);
    }

    public sealed override IntPtr GenTexture()
    {
        if(_changed)
        {
            _changed = false;
            _freemem();
            _createTxt(_active);
        }

        return _texture;
    }

    public override void OnHover()
    {
        _changed = true;
        _active = true;
    }

    public override void OnUnhover()
    {
        _changed = true;
        _active = false;
    }

    public override Action? OnClick()
    {
        return _action;
    }

    ~Button()
    {
        _freemem();
    }
}