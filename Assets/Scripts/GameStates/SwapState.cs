using Cysharp.Threading.Tasks;
using Events;
using Services;
using StateMachine;
using UniTaskPubSub;

namespace GameStates
{
    /// <summary>
    /// Swap State:
    /// - Check if a valid match results from the swap.
    /// - Animate the two chosen tiles swapping places.
    /// - If no match results reverse the swap.
    /// </summary>
    public class SwapState : IPaylodedState<SwapStateContext>
    {
        private StateMachine.StateMachine _stateMachine;
        private readonly BoardService _boardService;
        private readonly AsyncMessageBus _messageBus;
        private readonly MatchDetectionService _matchDetectionService;

        public SwapState(
            BoardService boardService,
            MatchDetectionService matchDetectionService,
            AsyncMessageBus messageBus)
        {
            _matchDetectionService = matchDetectionService;
            _messageBus = messageBus;
            _boardService = boardService;
        }

        public void Initialize(StateMachine.StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public async UniTask Enter(SwapStateContext swapContext)
        {
            _boardService.SwapTiles(swapContext.FirstTile, swapContext.SecondTile);

            if (_matchDetectionService.HasMatchAt(_boardService.Board, swapContext.FirstTile) ||
                _matchDetectionService.HasMatchAt(_boardService.Board, swapContext.SecondTile))
            {
                var tileSwappedEvent = new TilesSwappedEvent(swapContext.FirstTile, swapContext.SecondTile, true);
                await _messageBus.PublishAsync(tileSwappedEvent);
                
                await _stateMachine.Enter<MatchDetectionState>();
            }
            else
            {
                var tileSwappedEvent = new TilesSwappedEvent(swapContext.FirstTile, swapContext.SecondTile, false);
                await _messageBus.PublishAsync(tileSwappedEvent);
                
                _boardService.SwapTiles(swapContext.FirstTile, swapContext.SecondTile);
                
                await _stateMachine.Enter<PlayerInputState>();
            }
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}