using NUnit.Framework;
using Services;
using Services.Board;
using Settings;

namespace Tests.EditMode
{
    [TestFixture]
    public class BoardServiceTests
    {
        private TileType[] _availableTiles;
        private BoardGenerator _boardGenerator;

        [SetUp]
        public void SetUp()
        {
            _boardGenerator = new BoardGenerator(new MatchDetectionService());
            _availableTiles = new[]
            {
                new TileType(), new TileType(), new TileType()
            };
        }

        [Test]
        public void RemoveTiles_GivenBoardWithSeveralMatches_RemovedAllMatches()
        {
            var boardService = new BoardService(_boardGenerator);
            var board = new BoardBuilder(5, 5, _availableTiles)
                .SetupNextRow(0, 0, 2, 0, 1)
                .SetupNextRow(0, 1, 2, 0, 1)
                .SetupNextRow(0, 1, 2, 0, 1)
                .SetupNextRow(1, 2, 0, 1, 2)
                .SetupNextRow(1, 1, 1, 1, 2)
                .RewriteExistingBoard(boardService.Board)
                .Build();

            var matchesService = new MatchDetectionService();
            var matches = matchesService.GetAllMatches(boardService.Board);
            boardService.RemoveTiles(matches);

            var expectedBoard = new BoardBuilder(5, 5, _availableTiles)
                .SetupNextRow(null, 0, null, null, null)
                .SetupNextRow(null, 1, null, null, null)
                .SetupNextRow(null, 1, null, null, null)
                .SetupNextRow(1, 2, 0, 1, 2)
                .SetupNextRow(null, null, null, null, 2)
                .RewriteExistingBoard(boardService.Board)
                .Build();

            Assert.AreEqual(expectedBoard, board);
        }

        [Test]
        public void FallTiles_GivenBoardWithMissingRow_FallTilesCorrectly()
        {
            var boardService = new BoardService(_boardGenerator);
            var board = new BoardBuilder(3, 5, _availableTiles)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(null, null, null)
                .SetupNextRow(1, 2, 2)
                .SetupNextRow(1, 1, 2)
                .RewriteExistingBoard(boardService.Board)
                .Build();

            boardService.FallTiles();

            var newExpectedBoard = new BoardBuilder(3, 5, _availableTiles)
                .SetupNextRow(null, null, null)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(1, 2, 2)
                .SetupNextRow(1, 1, 2)
                .Build();

            Assert.AreEqual(newExpectedBoard, board);
        }

        [Test]
        public void FallTiles_GivenBoardWithThreeMissingRows_FallTilesCorrectly()
        {
            var boardService = new BoardService(_boardGenerator);
            var board = new BoardBuilder(3, 5, _availableTiles)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .RewriteExistingBoard(boardService.Board)
                .Build();

            boardService.FallTiles();

            var newExpectedBoard = new BoardBuilder(3, 5, _availableTiles)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .Build();

            Assert.AreEqual(newExpectedBoard, board);
        }

        [Test]
        public void FallTiles_GivenBoardWithThreeGaps_FallTilesCorrectly()
        {
            var boardService = new BoardService(_boardGenerator);
            var board = new BoardBuilder(3, 6, _availableTiles)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(null, null, null)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(null, null, null)
                .SetupNextRow(2, 2, 2)
                .SetupNextRow(null, null, null)
                .RewriteExistingBoard(boardService.Board)
                .Build();

            boardService.FallTiles();

            var newExpectedBoard = new BoardBuilder(3, 6, _availableTiles)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(2, 2, 2)
                .Build();

            Assert.AreEqual(newExpectedBoard, board);
        }

        [Test]
        public void FillEmptyTiles_GivenBoardWithMissingTiles_MissingTilesFilledProperly()
        {
            var boardService = new BoardService(_boardGenerator);
            var board = new BoardBuilder(3, 5, _availableTiles)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(null, 1, 2)
                .SetupNextRow(1, 1, null)
                .SetupNextRow(null, 2, null)
                .RewriteExistingBoard(boardService.Board)
                .Build();

            boardService.FillEmptyTiles(_availableTiles);

            Assert.IsNotNull(board.Get(2, 0));
            Assert.IsNotNull(board.Get(2, 2));
            Assert.IsNotNull(board.Get(0, 4));
            Assert.IsNotNull(board.Get(2, 4));
        }
    }
}