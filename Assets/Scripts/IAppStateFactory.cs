using Models;

public interface IAppStateFactory
{
    AppState Create();
    AppState State { get; }
}