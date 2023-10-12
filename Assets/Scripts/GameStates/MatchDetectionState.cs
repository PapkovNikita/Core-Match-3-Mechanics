using Cysharp.Threading.Tasks;
using Services;
using Services.Board;
using StateMachine;

namespace GameStates
{
    /// <summary>
    /// Match Detection State:
    /// - After a swap, check for matches in the grid.
    /// - Identify all groups of 3 or more matched tiles.
    /// </summary>
    public class MatchDetectionState : IState
    {
        private readonly MatchDetectionService _matchDetectionService;
        private StateMachine.StateMachine _stateMachine;
        private readonly IBoardProvider _boardProvider;

        public MatchDetectionState(
            IBoardProvider boardProvider,
            MatchDetectionService matchDetectionService)
        {
            _boardProvider = boardProvider;
            _matchDetectionService = matchDetectionService;
        }

        public void Initialize(StateMachine.StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
        
        public UniTask Enter()
        {
            var allMatches = _matchDetectionService.GetAllMatches(_boardProvider.Board);
            if (allMatches.Count > 0)
            {
                var context = new TileRemovalStateContext(allMatches);
                return _stateMachine.Enter<TileRemovalState, TileRemovalStateContext>(context);
            }

            return _stateMachine.Enter<CheckForMovesState>();
        }

    }
}