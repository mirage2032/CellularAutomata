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

    public virtual void Render()
    {
        RenderClear();
        RenderDraw();
    }
    public virtual void RenderClear()
    {
        SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255);
        SDL.SDL_RenderClear(_renderer);
    }
    public virtual void RenderDraw()
    {
        List<GameObject> allObjects=new List<GameObject>();
        allObjects.AddRange(_gameObjects);
        List<GameObject> searchCurrent = _gameObjects;
        List<GameObject> searchNext = new List<GameObject>();
        while (true)
        {
            if (searchCurrent.Count == 0) break;
            foreach (var obj in searchCurrent)
            {
                allObjects.Add(obj);
                searchNext.AddRange(obj.children);
            }

            searchCurrent = searchNext;
            searchNext = new List<GameObject>();
        }
        allObjects = allObjects.OrderBy(o => o.Z).ToList();
        foreach (var obj in allObjects)
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

    public virtual void Tick()
    {
    }

    public List<Action> HandleInput()
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
                        Action? action = _onHoverActive.OnClick() ?? null;
                        if (action == null)
                            break;
                        if (action.Value.Type==ActionType.Callback)
                            action.Value.Callback.Invoke();
                        else
                            actions.Add(_onHoverActive.OnClick() ?? new Action(ActionType.None));
                    }

                    break;
            }
        }

        return actions;
    }
}