using GameStates;
using Services;
using Settings;
using VContainer.Unity;
using Views;

public class TestGameController 
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly LevelSettings _levelSettings;
    private readonly BoardPresenter _boardPresenter;
    private readonly ISwipeHandler _swipeHandler;

    public TestGameController(GameStateMachine gameStateMachine, 
        BoardPresenter boardPresenter,
        ISwipeHandler swipeHandler)
    {
        _swipeHandler = swipeHandler;
        _boardPresenter = boardPresenter;
        _gameStateMachine = gameStateMachine;
    }
    
    public async void Start()
    {
        _boardPresenter.Show();

        await _gameStateMachine.Enter<BoardInitializationState>();
    }

    public void DoSwipe()
    {
        _swipeHandler.Tick();
    }
}