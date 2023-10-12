using NUnit.Framework;
using Services;
using Services.Board;
using Settings;
using UnityEngine;

namespace Tests.EditMode
{
    [TestFixture]
    public class BoardGeneratorTests
    {
        private TileType[] _availableTiles;

        [SetUp]
        public void SetUp()
        {
            _availableTiles = new[]
            {
                new TileType(), new TileType(), new TileType()
            };
        }

        [Test]
        [Category("LongRunning")]
        public void Generate_With3TilesAndRepeated10000Times_GeneratesMatchFreeBoardAndNoExceptions()
        {
            var matchesDetectionService = new MatchDetectionService();
            var boardGenerator = new BoardGenerator(matchesDetectionService);
            var boardSize = new Vector2Int(10, 10);

            for (int i = 0; i < 10000; i++)
            {
                var board = boardGenerator.Generate(boardSize, _availableTiles);
                var matches = matchesDetectionService.GetAllMatches(board);

                if (matches.Count > 0)
                {
                    Assert.Fail("Error: was generated a board with a match");
                }
            }
            
            Assert.Pass();
        }
    }
}