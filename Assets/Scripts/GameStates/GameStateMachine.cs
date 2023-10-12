namespace GameStates
{
    public class GameStateMachine : StateMachine.StateMachine
    {
        public GameStateMachine(
            BoardInitializationState initializationState,
            PlayerInputState playerInputState,
            SwapState swapState,
            MatchDetectionState matchDetectionState,
            TileRemovalState tileRemovalState,
            FallState fallState,
            CheckForMovesState checkForMovesState,
            GameOverState gameOverState)
            : base(initializationState,
                playerInputState,
                swapState,
                matchDetectionState,
                tileRemovalState,
                fallState,
                checkForMovesState,
                gameOverState
            )
        {
        }
    }
}