namespace Hex.Engine.Test.CandiateMoves
{
    using System.Collections.Generic;
    using System.Linq;
    using Hex.Board;
    using Hex.Engine.CandiateMoves;
    using NUnit.Framework;

    [TestFixture]
    public class CandiateMovesAllTest
    {
        private const int BoardSize = 10;
        private const int BoardCellCount = BoardSize * BoardSize;

        [Test]
        public void CreateTest()
        {
            CandidateMovesAll allMoves = new CandidateMovesAll();
            Assert.IsNotNull(allMoves);
        }

        [Test]
        public void CountEmptyTest()
        {
            CandidateMovesAll allMoves = new CandidateMovesAll();
            HexBoard testBoard = new HexBoard(BoardSize);

            Location[] moves = allMoves.CandidateMoves(testBoard, 0).ToArray();

            Assert.AreEqual(BoardCellCount, moves.Length);
        }

        [Test]
        public void CountOneMoveTest()
        {
            CandidateMovesAll allMoves = new CandidateMovesAll();
            HexBoard testBoard = new HexBoard(BoardSize);

            testBoard.PlayMove(5, 5, true);

            IEnumerable<Location> moves = allMoves.CandidateMoves(testBoard, 0);

            Assert.AreEqual(BoardCellCount - 1, moves.Count());
        }

        [Test]
        public void CountDownTest()
        {
            CandidateMovesAll allMoves = new CandidateMovesAll();
            HexBoard testBoard = new HexBoard(BoardSize);

            int emptyCellCount = BoardSize * BoardSize;

            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    // as cells are played, less empty cells are left
                    testBoard.PlayMove(x, y, true);
                    emptyCellCount--;

                    IEnumerable<Location> moves = allMoves.CandidateMoves(testBoard, 0);
                    Assert.AreEqual(emptyCellCount, moves.Count());
                }
            }
        }
    }
}
