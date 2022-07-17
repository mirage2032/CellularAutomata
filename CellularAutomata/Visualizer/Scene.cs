using SDL2;

namespace CellularAutomata.Visualizer;

public abstract class Scene
{
    protected List<GameObject> _gameObjects=new();
    protected GameObject? _onHoverActive;
    protected IntPtr _window, _renderer;

    protected void RenderGameObjects(GameObject obj)
    {
        SDL.SDL_Rect rect = new SDL.SDL_Rect() {h = obj.H(), w = obj.W(), x = obj.X, y = obj.Y};
        SDL.SDL_RenderCopy(_renderer, obj.GenTexture(), IntPtr.Zero, ref rect);
    }

    public void Render()
    {
        foreach (var obj in _gameObjects)
        {
            RenderGameObjects(obj);
        }
        SDL.SDL_RenderPresent(_renderer);
    }

    private bool CheckFocusGameObject(GameObject btn, int x, int y)
    {
        if (x > btn.X && x < (btn.X + btn.W()) && y > btn.Y && y < btn.Y + btn.H())
            return true;
        return false;
    }

    protected void OnHoverUpdate()
    {
        if (SDL.SDL_GetMouseFocus() == _window)
        {
            SDL.SDL_GetMouseState(out var mouseX, out var mouseY);

            if (_onHoverActive != null && !CheckFocusGameObject(_onHoverActive, mouseX, mouseY))
            {
                _onHoverActive.OnUnhover();
                _onHoverActive = null;
            }

            if (_onHoverActive == null)
            {
                foreach (var btn in _gameObjects)
                {
                    if (CheckFocusGameObject(btn, mouseX, mouseY))
                    {
                        _onHoverActive = btn;
                        _onHoverActive.OnHover();
                        break;
                    }
                }
            }
        }
    }

    public void Tick()
    {
    }

    public virtual List<Action> HandleInput() => new();
}