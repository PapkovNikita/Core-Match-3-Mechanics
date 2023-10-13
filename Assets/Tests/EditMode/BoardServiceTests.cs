using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Services;
using Services.Board;
using Settings;
using UnityEngine;

namespace Tests.EditMode
{
    [TestFixture]
    public class BoardServiceTests
    {
        private readonly Mock<ILevelSettings> _levelSettingsMock = new();
        private TileType[] _availableTiles;
        private FakeBoardGenerator _boardGenerator;

        [SetUp]
        public void SetUp()
        {
            _boardGenerator = new FakeBoardGenerator();
            
            _availableTiles = new[]
            {
                new TileType(), new TileType(), new TileType()
            };

            _levelSettingsMock.Setup(x => x.AvailableTiles).Returns(_availableTiles);
            _levelSettingsMock.Setup(x => x.Size).Returns(new Vector2Int(5,5));
        }

        [Test]
        public void RemoveTiles_GivenBoardWithSeveralMatches_RemovedAllMatches()
        {
            var boardService = new BoardService(_boardGenerator);
            _boardGenerator.Setup(5, 5, _availableTiles)
                .SetupNextRow(0, 0, 2, 0, 1)
                .SetupNextRow(0, 1, 2, 0, 1)
                .SetupNextRow(0, 1, 2, 0, 1)
                .SetupNextRow(1, 2, 0, 1, 2)
                .SetupNextRow(1, 1, 1, 1, 2);

            boardService.GenerateBoard(_levelSettingsMock.Object);
            var matchesService = new MatchDetectionService();
            var matches = matchesService.GetAllMatches(boardService.Board);
            boardService.RemoveTiles(matches);

            var expectedBoard = _boardGenerator.Setup(5, 5, _availableTiles)
                .SetupNextRow(null, 0, null, null, null)
                .SetupNextRow(null, 1, null, null, null)
                .SetupNextRow(null, 1, null, null, null)
                .SetupNextRow(1, 2, 0, 1, 2)
                .SetupNextRow(null, null, null, null, 2)
                .Generate();

            Assert.AreEqual(expectedBoard, boardService.Board);
        }

        [Test]
        public void FallTiles_GivenBoardWithMissingRow_FallTilesCorrectly()
        {
            var boardService = new BoardService(_boardGenerator);
            _boardGenerator.Setup(3, 5, _availableTiles)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(null, null, null)
                .SetupNextRow(1, 2, 2)
                .SetupNextRow(1, 1, 2);
            
            boardService.GenerateBoard(_levelSettingsMock.Object);
            boardService.FallTiles();

            var newExpectedBoard = _boardGenerator.Setup(3, 5, _availableTiles)
                .SetupNextRow(null, null, null)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(1, 2, 2)
                .SetupNextRow(1, 1, 2)
                .Generate();

            Assert.AreEqual(newExpectedBoard, boardService.Board);
        }

        [Test]
        public void FallTiles_GivenBoardWithThreeMissingRows_FallTilesCorrectly()
        {
            var boardService = new BoardService(_boardGenerator);
            _boardGenerator.Setup(3, 5, _availableTiles)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null);

            boardService.GenerateBoard(_levelSettingsMock.Object);
            boardService.FallTiles();

            var newExpectedBoard = _boardGenerator.Setup(3, 5, _availableTiles)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .Generate();

            Assert.AreEqual(newExpectedBoard, boardService.Board);
        }

        [Test]
        public void FallTiles_GivenBoardWithThreeGaps_FallTilesCorrectly()
        {
            var boardService = new BoardService(_boardGenerator);
            _boardGenerator.Setup(3, 6, _availableTiles)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(null, null, null)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(null, null, null)
                .SetupNextRow(2, 2, 2)
                .SetupNextRow(null, null, null);

            boardService.GenerateBoard(_levelSettingsMock.Object);
            boardService.FallTiles();

            var newExpectedBoard = _boardGenerator.Setup(3, 6, _availableTiles)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(null, null, null)
                .SetupNextRow(0, 0, 1)
                .SetupNextRow(0, 1, 1)
                .SetupNextRow(2, 2, 2)
                .Generate();

            Assert.AreEqual(newExpectedBoard, boardService.Board);
        }

        [Test]
        public void FillEmptyTiles_GivenBoardWithMissingTiles_MissingTilesFilledProperly()
        {
            var matchDetectionService = new MatchDetectionService();
            var boardGenerator = new BoardGenerator(matchDetectionService);
            var boardService = new BoardService(boardGenerator);
            
            boardService.GenerateBoard(_levelSettingsMock.Object);
            var matches = new List<Services.Match>
            {
                new(new Vector2Int(0, 0), new Vector2Int(0, 0)),
                new(new Vector2Int(4, 4), new Vector2Int(4, 4))
            };
            boardService.RemoveTiles(matches);
            
            boardService.FillEmptyTiles(_availableTiles);

            Assert.IsFalse(boardService.Board.GetTile(0, 0).IsEmpty);
            Assert.IsFalse(boardService.Board.GetTile(4, 4).IsEmpty);
        }
    }
}