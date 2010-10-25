namespace Hex.Engine.Test.CandiateMoves
{
    using System.Collections.Generic;
    using System.Linq;

    using Hex.Board;
    using Hex.Engine.CandiateMoves;
    using Hex.Engine.Lookahead;
    using NUnit.Framework;

    [TestFixture]
    public class CandiateMovesAllSortedTest
    {
        private const int BoardSize = 10;
        private const int BoardCellCount = BoardSize * BoardSize;

        [Test]
        public void TestCreate()
        {
            GoodMoves goods = new GoodMoves();
            CandidateMovesAllSorted moves = new CandidateMovesAllSorted(goods, 0);
            Assert.IsNotNull(moves);
        }

        [Test]
        public void CountOneMoveTest()
        {
            GoodMoves goods = new GoodMoves();
            CandidateMovesAllSorted moveFinder = new CandidateMovesAllSorted(goods, 0);
            HexBoard testBoard = new HexBoard(BoardSize);

            testBoard.PlayMove(5, 5, true);

            IEnumerable<Location> moves = moveFinder.CandidateMoves(testBoard, 0);

            Assert.AreEqual(BoardCellCount - 1, moves.Count());
        }
    }
}
