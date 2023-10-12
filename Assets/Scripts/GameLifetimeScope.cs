using GameStates;
using Services;
using Settings;
using UniTaskPubSub;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Views;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private Grid _grid;
    [SerializeField] private BoardView _boardView;
    
    protected override void Configure(IContainerBuilder builder)
    {
        RegisterGameStateMachine(builder);

        builder.Register<BoardService>(Lifetime.Singleton)
            .AsImplementedInterfaces()
            .AsSelf();
        
        builder.Register<MatchDetectionService>(Lifetime.Singleton);
        builder.Register<BoardPresenter>(Lifetime.Singleton);
        builder.Register<AsyncMessageBus>(Lifetime.Singleton);
        builder.Register<SwipeHandler>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<LevelSettingsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<BoardGenerator>(Lifetime.Singleton);
        builder.Register<MovesValidator>(Lifetime.Singleton);
        
        builder.RegisterInstance(_grid);    
        builder.RegisterInstance(_gameSettings);
        builder.RegisterInstance(_boardView);
        
        builder.RegisterEntryPoint<GameController>();
    }

    private static void RegisterGameStateMachine(IContainerBuilder builder)
    {
        builder.Register<GameStateMachine>(Lifetime.Singleton);
        
        builder.Register<BoardInitializationState>(Lifetime.Singleton);
        builder.Register<PlayerInputState>(Lifetime.Singleton);
        builder.Register<SwapState>(Lifetime.Singleton);
        builder.Register<MatchDetectionState>(Lifetime.Singleton);
        builder.Register<TileRemovalState>(Lifetime.Singleton);
        builder.Register<FallState>(Lifetime.Singleton);
        builder.Register<CheckForMovesState>(Lifetime.Singleton);
        builder.Register<GameOverState>(Lifetime.Singleton);
    }
}