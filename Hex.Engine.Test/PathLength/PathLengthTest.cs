namespace Hex.Engine.Test.PathLength
{
    using System.Collections.Generic;

    using Hex.Board;
    using Hex.Engine.PathLength;
    using NUnit.Framework;

    /// <summary>
    /// Test methods for PathLength classes
    /// </summary>
    [TestFixture]
    public class PathLengthTest
    {
        private const int BoardSize = 7;

        [Test]
        public void AStarTest()
        {
            AllTests(new PathLengthAStarFactory());
        }

        [Test]
        public void AStarNoNeighbours2Test()
        {
            AllTests(new PathLengthAStarSimpleFactory());
        }

        [Test]
        public void LoopTest()
        {
            AllTests(new PathLengthLoopFactory());
        }

        private static void AllTests(IPathLengthFactory pathLengthFactory)
        {
            PlayerScoreBlankBoard(pathLengthFactory);
            PlayerScoreBaricaded(pathLengthFactory);
            PlayerScoreAlmostBarricaded(pathLengthFactory);
            PlayerScorePartBarricade(pathLengthFactory);
            PlayerScoreZigZag(pathLengthFactory);
        }

        private static void PlayerScoreBlankBoard(IPathLengthFactory pathLengthFactory)
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            PathLengthBase pathLength = pathLengthFactory.CreatePathLength(hexBoard);

            // blank board
            int xScore = pathLength.PlayerScore(true);
            int yScore = pathLength.PlayerScore(false);
            Assert.AreEqual(xScore, yScore, "x and y score");

            // no advantage
            int equalMoveScore = pathLength.SituationScore();
            Assert.AreEqual(equalMoveScore, 0, "equalMoveScore");

            // all cells still on the clean path
            pathLength.PlayerScore(true);
            List<Location> cleanPathX = pathLength.GetCleanPath(true);

            pathLength.PlayerScore(false);
            List<Location> cleanPathY = pathLength.GetCleanPath(false);

            const int AllCellsCount = BoardSize * BoardSize;
            Assert.AreEqual(AllCellsCount, cleanPathX.Count, "Clean path x should have all cells");
            Assert.AreEqual(AllCellsCount, cleanPathY.Count, "Clean path y should have all cells");
        }

        private static void PlayerScoreBaricaded(IPathLengthFactory pathLengthFactory)
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            PathLengthBase pathLength = pathLengthFactory.CreatePathLength(hexBoard);

            // baricaded board
            for (int y = 0; y < hexBoard.Size; y++)
            {
                hexBoard.PlayMove(2, y, true);
            }

            int xScore = pathLength.PlayerScore(true);
            Assert.AreEqual(xScore, 0);

            int yScore = pathLength.PlayerScore(false);
            Assert.IsTrue(yScore > hexBoard.Size);

            // winning advantage to player 1
            int winMoveScore = pathLength.SituationScore();
            AssertWinner(winMoveScore, Occupied.PlayerX);
        }

        private static void PlayerScoreAlmostBarricaded(IPathLengthFactory pathLengthFactory)
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            PathLengthBase pathLength = pathLengthFactory.CreatePathLength(hexBoard);

            // almost barricated board
            for (int y = 0; y < hexBoard.Size; y++)
            {
                if (y != 3)
                {
                    hexBoard.PlayMove(2, y, true);
                }
            }

            int xScore = pathLength.PlayerScore(true);
            Assert.IsTrue(xScore > 0);

            int yScore = pathLength.PlayerScore(false);
            Assert.IsTrue(yScore > xScore);

            // strong advantage to player 1
            int advantageMoveScore = pathLength.SituationScore();
            Assert.IsTrue(advantageMoveScore > 0);
        }

        private static void PlayerScorePartBarricade(IPathLengthFactory pathLengthFactory)
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            PathLengthBase pathLength = pathLengthFactory.CreatePathLength(hexBoard);

            // partly baricaded board
            for (int y = 0; y < hexBoard.Size; y++)
            {
                if ((y % 2) == 0)
                {
                    hexBoard.PlayMove(2, y, true);
                }
            }

            int xScore = pathLength.PlayerScore(true);
            Assert.IsTrue(xScore > 0);

            int yScore = pathLength.PlayerScore(false);
            Assert.IsTrue(yScore > xScore);

            // some advantage to player 1
            int advantageMoveScore = pathLength.SituationScore();
            Assert.IsTrue(advantageMoveScore > 0);
        }

        private static void PlayerScoreZigZag(IPathLengthFactory pathLengthFactory)
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            PathLengthBase pathLength = pathLengthFactory.CreatePathLength(hexBoard);

            for (int y = 0; y < hexBoard.Size - 1; y++)
            {
                hexBoard.PlayMove(2, y, true);
                hexBoard.PlayMove(5, hexBoard.Size - (1 + y), true);

                int xScore = pathLength.PlayerScore(true);
                Assert.IsTrue(xScore > 0);

                int yScore = pathLength.PlayerScore(false);
                Assert.IsTrue(yScore >= hexBoard.Size);
                if (y > (hexBoard.Size / 2))
                {
                    Assert.IsTrue(yScore > xScore);
                }

                // some advantage to player 1
                int advantageMoveScore = pathLength.SituationScore();
                Assert.IsTrue(advantageMoveScore >= y);
            }
        }

        private static void AssertWinner(int score, Occupied winner)
        {
            Assert.IsTrue(MoveScoreConverter.IsWin(score), "Should have winner");
            Assert.AreEqual(winner, MoveScoreConverter.Winner(score), "Wrong winner");
        }
    }
}
