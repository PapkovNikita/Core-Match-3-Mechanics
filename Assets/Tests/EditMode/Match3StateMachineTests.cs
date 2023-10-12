using System.Threading.Tasks;
using GameStates;
using Moq;
using NUnit.Framework;
using Services;
using Settings;
using UniTaskPubSub;
using UnityEngine;

namespace Tests.EditMode
{
    public class Match3StateMachineTests
    {
        private readonly Mock<ILevelSettings> _levelSettingsMock = new();
        private readonly Mock<ILevelSettingsProvider> _levelSettingsProviderMock = new();
        private TileSettings[] _availableTiles;
        private ISwipeHandler _swipeHandler;
        private GameStateMachine _stateMachine;

        [SetUp]
        public void SetUp()
        {
            _availableTiles = new[]
            {
                new TileSettings(), new TileSettings(), new TileSettings()
            };

            var boardSize = new Vector2Int(10, 10);
            _levelSettingsMock.Setup(x => x.Size).Returns(boardSize);
            _levelSettingsMock.Setup(x => x.AvailableTiles).Returns(_availableTiles);
            _levelSettingsProviderMock.Setup(x => x.GetCurrentLevelSettings())
                .Returns(_levelSettingsMock.Object);
            
            _swipeHandler = new FakeSwipeHandler(_levelSettingsProviderMock.Object);
            var matchesDetectionService = new MatchDetectionService();
            var boardGenerator = new BoardGenerator(matchesDetectionService);
            var boardService = new BoardService(boardGenerator);

            var messageBus = new AsyncMessageBus();

            var boardInitializationState = new BoardInitializationState(boardService,
                _levelSettingsProviderMock.Object,
                messageBus);
            var inputState = new PlayerInputState(_swipeHandler, _levelSettingsProviderMock.Object);
            var swapState = new SwapState(boardService, matchesDetectionService, messageBus);
            var matchesDetectionState = new MatchDetectionState(boardService, matchesDetectionService);
            var tileRemovalState = new TileRemovalState(boardService, messageBus);
            var fallState = new FallState(boardService, _levelSettingsProviderMock.Object, messageBus);
            var checkForMovesState = new CheckForMovesState(boardService, new MovesValidator());
            var gameOverState = new GameOverState();

            _stateMachine = new GameStateMachine(
                boardInitializationState, inputState, swapState,
                matchesDetectionState, tileRemovalState, fallState,
                checkForMovesState, gameOverState);
        }

        [Test]
        [Category("LongRunning")]
        public async void StateMachine_With3TilesAnd100000Swipes_WorksWell()
        {
            await _stateMachine.Enter<BoardInitializationState>();

            for (int i = 0; i < 100000; i++)
            {
                _swipeHandler.Tick();
            }
        }
    }
}