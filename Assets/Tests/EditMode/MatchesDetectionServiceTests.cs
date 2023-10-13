using System.Linq;
using NUnit.Framework;
using Services;
using Settings;
using UnityEngine;

namespace Tests.EditMode
{
    [TestFixture]
    public class MatchesDetectionServiceTests
    {
        private MatchDetectionService _service;
        private TileType[] _availableTiles;
        private FakeBoardGenerator _boardGenerator;

        [SetUp]
        public void SetUp()
        {
            _boardGenerator = new FakeBoardGenerator();
            _service = new MatchDetectionService();

            _availableTiles = new[]
            {
                new TileType(), new TileType(), new TileType()
            };
        }

        [TestFixture]
        public class HasMatchAtTests : MatchesDetectionServiceTests
        {
            [Test]
            public void GivenTileSurroundedByDifferentTiles_ReturnsFalse()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(1, 0, 2)
                    .SetupNextRow(2, 1, 0)
                    .SetupNextRow(0, 2, 1)
                    .Generate();

                var result = _service.HasMatchAt(board, new Vector2Int(1, 1));

                Assert.IsFalse(result);
            }

            [Test]
            public void GivenBoundaryPositionWithMatch_ReturnsTrue()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 1, 0)
                    .SetupNextRow(2, 2, 0)
                    .SetupNextRow(1, 0, 0)
                    .Generate();

                var result = _service.HasMatchAt(board, new Vector2Int(2, 2));

                Assert.IsTrue(result);
            }

            [Test]
            public void GivenSingleHorizontalMatch_ReturnsTrue()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 0, 0)
                    .SetupNextRow(1, 2, 1)
                    .SetupNextRow(2, 1, 2)
                    .Generate();

                // Act
                bool result = _service.HasMatchAt(board, new Vector2Int(0, 0)); 

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void GivenSingleVerticalMatch_ReturnsTrue()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 1, 2)
                    .SetupNextRow(0, 1, 2)
                    .SetupNextRow(0, 1, 2)
                    .Generate();

                // Act
                bool result = _service.HasMatchAt(board, new Vector2Int(0, 0));

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void GivenNoMatches_ReturnsFalse()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 1, 2)
                    .SetupNextRow(2, 0, 1)
                    .SetupNextRow(1, 2, 0)
                    .Generate();

                // Act
                bool result = _service.HasMatchAt(board, new Vector2Int(0, 0));

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public void GivenSingleTileTypeSurroundedByAnother_ReturnsFalse()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(1, 1, 1)
                    .SetupNextRow(1, 0, 1)
                    .SetupNextRow(1, 1, 1)
                    .Generate();

                var result = _service.HasMatchAt(board, new Vector2Int(1, 1));

                Assert.IsFalse(result);
            }

            [Test]
            public void Given2x2SquareOfSameTile_ReturnsFalse()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 0, 2)
                    .SetupNextRow(0, 0, 1)
                    .SetupNextRow(1, 1, 1)
                    .Generate();

                var result = _service.HasMatchAt(board, new Vector2Int(0, 0));

                Assert.IsFalse(result);
            }
        }

        [TestFixture]
        public class GetAllMatchesTests : MatchesDetectionServiceTests
        {
            [Test]
            public void GivenBoardWithTwoMatch_ReturnsCorrectMatches()
            {
                var board = _boardGenerator.Setup(4, 4, _availableTiles)
                    .SetupNextRow(0, 2, 1, 1)
                    .SetupNextRow(2, 2, 1, 0)
                    .SetupNextRow(1, 0, 1, 0)
                    .SetupNextRow(1, 0, 2, 0)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(2, matches.Count);

                var match = new Match(new Vector2Int(2, 0), new Vector2Int(2, 2));
                Assert.IsTrue(matches.Contains(match));

                match = new Match(new Vector2Int(3, 1), new Vector2Int(3, 3));
                Assert.IsTrue(matches.Contains(match));
            }

            [Test]
            public void GivenTwoMatchesSideBySide_ReturnsCorrectMatches()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 1, 1)
                    .SetupNextRow(0, 1, 1)
                    .SetupNextRow(0, 2, 2)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(1, matches.Count);
                Assert.AreEqual(matches.First().From, new Vector2Int(0, 0));
                Assert.AreEqual(matches.First().To, new Vector2Int(0, 2));
            }

            [Test]
            public void GivenBothHorizontalAndVerticalMatches_ReturnsCorrectMatches()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 0, 0)
                    .SetupNextRow(0, 1, 2)
                    .SetupNextRow(0, 1, 2)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(2, matches.Count);

                var match = new Match(new Vector2Int(0, 0), new Vector2Int(2, 0));
                Assert.IsTrue(matches.Contains(match));

                match = new Match(new Vector2Int(0, 0), new Vector2Int(0, 2));
                Assert.IsTrue(matches.Contains(match));
            }

            [Test]
            public void GivenMultipleVerticalMatches_ReturnsCorrectMatches()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 1, 2)
                    .SetupNextRow(0, 1, 2)
                    .SetupNextRow(0, 1, 2)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(3, matches.Count);

                var match = new Match(new Vector2Int(0, 0), new Vector2Int(0, 2));
                Assert.IsTrue(matches.Contains(match));

                match = new Match(new Vector2Int(1, 0), new Vector2Int(1, 2));
                Assert.IsTrue(matches.Contains(match));

                match = new Match(new Vector2Int(2, 0), new Vector2Int(2, 2));
                Assert.IsTrue(matches.Contains(match));
            }

            [Test]
            public void GivenMultipleHorizontalMatches_ReturnsCorrectMatches()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 0, 0)
                    .SetupNextRow(1, 1, 1)
                    .SetupNextRow(2, 2, 2)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(3, matches.Count);


                var match = new Match(new Vector2Int(0, 0), new Vector2Int(2, 0));
                Assert.IsTrue(matches.Contains(match));

                match = new Match(new Vector2Int(0, 1), new Vector2Int(2, 1));
                Assert.IsTrue(matches.Contains(match));

                match = new Match(new Vector2Int(0, 2), new Vector2Int(2, 2));
                Assert.IsTrue(matches.Contains(match));
            }


            [Test]
            public void GivenTShape_ReturnsCorrectMatches()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 0, 0)
                    .SetupNextRow(1, 0, 2)
                    .SetupNextRow(1, 0, 2)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(2, matches.Count);

                var match = new Match(new Vector2Int(0, 0), new Vector2Int(2, 0));
                Assert.IsTrue(matches.Contains(match));

                match = new Match(new Vector2Int(1, 0), new Vector2Int(1, 2));
                Assert.IsTrue(matches.Contains(match));
            }

            [Test]
            public void GivenMultipleHorizontalMatches_ReturnsCorrectCount()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 0, 0)
                    .SetupNextRow(1, 1, 1)
                    .SetupNextRow(2, 2, 2)
                    .Generate();

                // Act
                var matches = _service.GetAllMatches(board);

                // Assert
                Assert.AreEqual(3, matches.Count); // Expecting three matches
            }

            [Test]
            public void GivenUniformBoard_ReturnsCorrectCount()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 0, 0)
                    .SetupNextRow(0, 0, 0)
                    .SetupNextRow(0, 0, 0)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(6, matches.Count);
            }

            [Test]
            public void GivenBoardWithNoConsecutiveThreeOfSameTile_ReturnsNoMatches()
            {
                var board = _boardGenerator.Setup(3, 3, _availableTiles)
                    .SetupNextRow(0, 1, 0)
                    .SetupNextRow(1, 0, 1)
                    .SetupNextRow(0, 1, 0)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(0, matches.Count);
            }

            [Test]
            public void GivenLargeBoardWithNoMatches_ReturnsEmpty()
            {
                var board = _boardGenerator.Setup(10, 10, _availableTiles)
                    .SetupNextRow(0, 1, 2, 0, 1, 2, 0, 1, 2, 0)
                    .SetupNextRow(1, 2, 0, 1, 2, 0, 1, 2, 0, 1)
                    .SetupNextRow(2, 0, 1, 2, 0, 1, 2, 0, 1, 2)
                    .SetupNextRow(1, 2, 0, 1, 2, 0, 1, 2, 0, 1)
                    .SetupNextRow(2, 0, 1, 2, 0, 1, 2, 0, 1, 2)
                    .SetupNextRow(1, 2, 0, 1, 2, 0, 1, 2, 0, 1)
                    .SetupNextRow(2, 0, 1, 2, 0, 1, 2, 0, 1, 2)
                    .SetupNextRow(1, 2, 0, 1, 2, 0, 1, 2, 0, 1)
                    .SetupNextRow(2, 0, 1, 2, 0, 1, 2, 0, 1, 2)
                    .SetupNextRow(1, 2, 0, 1, 2, 0, 1, 2, 0, 1)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.IsEmpty(matches);
            }

            [Test]
            public void GivenLargeBoardWithVerticalMatches_ReturnsCorrectCount()
            {
                var board = _boardGenerator.Setup(5, 5, _availableTiles)
                    .SetupNextRow(0, 1, 2, 0, 1)
                    .SetupNextRow(0, 1, 2, 0, 1)
                    .SetupNextRow(0, 1, 2, 0, 1)
                    .SetupNextRow(1, 2, 0, 1, 2)
                    .SetupNextRow(1, 2, 0, 1, 2)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(5, matches.Count);
            }

            [Test]
            public void GivenLargeBoardWithHorizontalMatches_ReturnsCorrectCount()
            {
                var board = _boardGenerator.Setup(10, 10, _availableTiles)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(1, 1, 1, 1, 1, 1, 1, 1, 1, 1)
                    .SetupNextRow(2, 2, 2, 2, 2, 2, 2, 2, 2, 2)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(1, 1, 1, 1, 1, 1, 1, 1, 1, 1)
                    .SetupNextRow(2, 2, 2, 2, 2, 2, 2, 2, 2, 2)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(1, 1, 1, 1, 1, 1, 1, 1, 1, 1)
                    .SetupNextRow(2, 2, 2, 2, 2, 2, 2, 2, 2, 2)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(10, matches.Count); // Assuming every row has a match
            }

            [Test]
            public void GivenLargeBoardWithSingleTileType_ReturnsCorrectCount()
            {
                var board = _boardGenerator.Setup(10, 10, _availableTiles)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .SetupNextRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    .Generate();

                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(20, matches.Count); // Assuming every row and every column has a match
            }

            [Test]
            public void GivenLargeBoardWithEdgeMatched_ReturnsCorrectCount()
            {
                var board = _boardGenerator.Setup(5, 5, _availableTiles)
                    .SetupNextRow(0, 0, 2, 0, 1)
                    .SetupNextRow(0, 1, 2, 0, 1)
                    .SetupNextRow(0, 1, 2, 0, 1)
                    .SetupNextRow(1, 2, 0, 1, 2)
                    .SetupNextRow(1, 1, 1, 1, 2)
                    .Generate();


                var matches = _service.GetAllMatches(board);

                Assert.AreEqual(5, matches.Count);
            }
        }
    }
}