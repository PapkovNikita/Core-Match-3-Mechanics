using Cysharp.Threading.Tasks;
using Events;
using Services;
using Services.Board;
using StateMachine;
using UniTaskPubSub;

namespace GameStates
{
    /// <summary>
    /// Fall State:
    /// - Tiles above the removed tiles fall to fill the empty spaces.
    /// - New tiles are generated to fill any remaining spaces.
    /// - It returns to the Match Detection State to check if the falling tiles created new matches.
    /// </summary>
    public class FallState : IState
    {
        private readonly BoardService _boardService;
        private readonly AsyncMessageBus _messageBus;
        private readonly ILevelSettingsProvider _levelSettingsProvider;
        private StateMachine.StateMachine _stateMachine;

        public FallState(
            BoardService boardService,
            ILevelSettingsProvider levelSettingsProvider,
            AsyncMessageBus messageBus)
        {
            _levelSettingsProvider = levelSettingsProvider;
            _messageBus = messageBus;
            _boardService = boardService;
        }

        public void Initialize(StateMachine.StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
        
        public async UniTask Enter()
        {
            _boardService.FallTiles();
            await _messageBus.PublishAsync(new TilesFellDownEvent());

            var levelSettings = _levelSettingsProvider.GetCurrentLevelSettings();
            _boardService.FillEmptyTiles(levelSettings.AvailableTiles);
            await _messageBus.PublishAsync(new NewTilesGeneratedEvent());
            
            await _stateMachine.Enter<MatchDetectionState>();
        }
    }
}