using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Events;
using Services;
using Services.Board;
using StateMachine;
using UniTaskPubSub;

namespace GameStates
{
    /// <summary>
    /// Tile Removal State:
    /// - Remove matched tiles from the board.
    /// </summary>
    public class TileRemovalState : IPaylodedState<TileRemovalStateContext>
    {
        private readonly BoardService _boardService;
        private readonly AsyncMessageBus _messageBus;
        private StateMachine.StateMachine _stateMachine;

        public TileRemovalState(BoardService boardService, AsyncMessageBus messageBus)
        {
            _messageBus = messageBus;
            _boardService = boardService;
        }

        public void Initialize(StateMachine.StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async UniTask Enter(TileRemovalStateContext context)
        {
            await RemoveTiles(context.Matches);
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        private async UniTask RemoveTiles(List<Match> allMatches)
        {
            _boardService.RemoveTiles(allMatches);

            await _messageBus.PublishAsync(new TilesRemovedEvent());
            await _stateMachine.Enter<FallState>();
        }
    }
}