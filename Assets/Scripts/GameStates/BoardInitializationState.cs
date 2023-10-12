using Cysharp.Threading.Tasks;
using Events;
using Services;
using Services.Board;
using StateMachine;
using UniTaskPubSub;

namespace GameStates
{
    /// <summary>
    /// Initialization State:
    /// - Set up the game board.
    /// - Load assets, generate initial tiles, etc.
    /// </summary>
    public class BoardInitializationState : IState
    {
        private readonly BoardService _boardService;
        private readonly AsyncMessageBus _messageBus;
        private readonly ILevelSettingsProvider _levelSettingsProvider;
        private StateMachine.StateMachine _stateMachine;

        public BoardInitializationState(
            BoardService boardService,
            ILevelSettingsProvider levelSettingsProvider,
            AsyncMessageBus messageBus)
        {
            _levelSettingsProvider = levelSettingsProvider;
            _messageBus = messageBus;
            _boardService = boardService;
        }

        public async UniTask Enter()
        {
            var levelSettings = _levelSettingsProvider.GetCurrentLevelSettings();
            _boardService.GenerateBoard(levelSettings);

            await _messageBus.PublishAsync(new BoardInitializedEvent(_boardService.Board));
            await _stateMachine.Enter<PlayerInputState>();
        }

        public void Initialize(StateMachine.StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}