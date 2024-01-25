using Models;

public class AppStateFactory : IAppStateFactory
{
    private AppState _state;

    public AppState Create()
    {
        return new AppState();
    }

    public void CreateNew()
    {
        _state = new AppState();
    }

    public AppState State
    {
        get
        {
            if (_state == null)
            {
                _state = Create();
            }

            return _state;
        }
    }
}