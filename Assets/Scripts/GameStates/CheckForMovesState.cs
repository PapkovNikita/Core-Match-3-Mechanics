using Cysharp.Threading.Tasks;
using Services;
using Services.Board;
using StateMachine;

namespace GameStates
{
    /// <summary>
    /// Check for Moves State:
    /// - After tiles settle, check if there are any valid moves left for the player.
    /// - If no moves are possible end the game.
    /// </summary>
    public class CheckForMovesState : IState
    {
        private StateMachine.StateMachine _stateMachine;
        private readonly MovesValidator _movesValidator;
        private readonly IBoardProvider _boardProvider;

        public CheckForMovesState(
            IBoardProvider boardProvider, 
            MovesValidator movesValidator)
        {
            _boardProvider = boardProvider;
            _movesValidator = movesValidator;
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
            if (_movesValidator.HasAnyPossibleMove(_boardProvider.Board))
            {
                await _stateMachine.Enter<PlayerInputState>();
            }
            else
            {
                await _stateMachine.Enter<GameOverState>();
            }
        }

    }
}