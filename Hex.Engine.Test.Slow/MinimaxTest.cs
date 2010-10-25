namespace Hex.Engine.Test.Slow
{
    using Hex.Board;
    using Hex.Engine.CandiateMoves;
    using Hex.Engine.Lookahead;
    using Hex.Engine.PathLength;

    using NUnit.Framework;

    /// <summary>
    /// Test methods for class Minimax
    /// </summary>
    [TestFixture]
    public class MinimaxTest
    {
        [Test]
        public void TestCalculateMove3PathLength()
        {
            HexBoard board = new HexBoard(5);
            PlayFourMoves(board);

            // test score at this point
            PathLengthLoop pathLength = new PathLengthLoop(board);
            int playerScore = pathLength.PlayerScore(true);
            Assert.AreEqual(3, playerScore);

            playerScore = pathLength.PlayerScore(false);
            Assert.AreEqual(3, playerScore);

            // no advantage
            int moveScore = pathLength.SituationScore();
            Assert.AreEqual(moveScore, 0);
        }

        [Test]
        public void TestCalculateMove3PlayerX()
        {
            HexBoard board = new HexBoard(5);
            PlayFourMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);

            MinimaxResult firstPlayerResult = minimax.DoMinimax(4, true);

            Location firstPlayerExpectedMove = new Location(2, 1);
            Assert.AreEqual(firstPlayerExpectedMove, firstPlayerResult.Move, "Wrong first player location");
        }

        [Test]
        public void TestCalculateMove3PlayerY()
        {
            HexBoard board = new HexBoard(5);
            PlayFourMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);

            // test score at this point
            PathLengthLoop pathLength = new PathLengthLoop(board);
            int playerScore = pathLength.PlayerScore(true);
            Assert.AreEqual(3, playerScore);

            playerScore = pathLength.PlayerScore(false);
            Assert.AreEqual(3, playerScore);

            MinimaxResult secondPlayerResult = minimax.DoMinimax(4, false);

            Location secondPlayerExpectedMove = new Location(1, 2);
            Assert.AreEqual(secondPlayerExpectedMove, secondPlayerResult.Move, "Wrong second player location");
        }

        [Test]
        public void TestNoLingering()
        {
            // prefer a quick win to a slow one
            HexBoard board = new HexBoard(5);
            Minimax minimax = MakeMinimaxForBoard(board);

            // diagonal  - one move to win
            board.PlayMove(4, 0, true); // playerX
            board.PlayMove(3, 1, true); // playerX
            board.PlayMove(2, 2, true); // playerX
            board.PlayMove(1, 3, true); // playerX

            for (int depth = 1; depth < 6; depth++)
            {
                DoTestTestNoLingeringWin(minimax, depth);
            }
        }

        [Test]
        public void TestNoLingering2()
        {
            // prefer a quick win to a slow one
            HexBoard board = new HexBoard(5);
            Minimax minimax = MakeMinimaxForBoard(board);

            // diagonal  - one move to win
            board.PlayMove(4, 0, true); // playerX
            board.PlayMove(3, 1, true); // playerX
            // this.board.PlayMove(2, 2, true); // playerX
            board.PlayMove(1, 3, true); // playerX
            board.PlayMove(0, 4, true); // playerX

            for (int depth = 1; depth < 6; depth++)
            {
                DoTestTestNoLingering2Win(minimax, depth);
            }
        }

        [Test]
        public void TestMinimax3()
        {
            HexBoard board = new HexBoard(5);
            Minimax minimax = MakeMinimaxForBoard(board);

            /*
                on a 5 * 5 board, red(playerx) has 3, 0 and 1, 4
                needs to play 2,2 to win - should know this at look ahead 5
             */

            board.PlayMove(3, 0, true);
            board.PlayMove(1, 4, true);

            MinimaxResult bestMove = minimax.DoMinimax(5, true);
            Location expectedMove = new Location(2, 2);

            Assert.IsTrue(MoveScoreConverter.IsWin(bestMove.Score), "No win " + bestMove.Score);
            Assert.AreEqual(expectedMove, bestMove.Move, "Wrong expected move");
        }

        [Test]
        public void TestMinimax4()
        {
            HexBoard board = new HexBoard(5);
            Minimax minimax = MakeMinimaxForBoard(board);

            /*
                on a 5 * 5 board, red(playerx) has 3, 0 and 1, 4  and 2,2
                    PlayerX has won, even if PlayerY goes next
                     should know this at look ahead 5
             */

            board.PlayMove(3, 0, true);
            board.PlayMove(1, 4, true);
            board.PlayMove(2, 2, true);

            MinimaxResult bestMove = minimax.DoMinimax(4, false);

            AssertWinner(bestMove.Score, Occupied.PlayerX);
        }

        private static Minimax MakeMinimaxForBoard(HexBoard board)
        {
            GoodMoves goodMoves = new GoodMoves();
            ICandidateMoves candidateMovesFinder = new CandidateMovesAll();

            Minimax result = new Minimax(board, goodMoves, candidateMovesFinder);
            result.GenerateDebugData = true;

            return result;
        }

        private static void PlayFourMoves(HexBoard board)
        {
            board.PlayMove(2, 0, true); // playerX
            board.PlayMove(0, 2, false); // PlayerY
            board.PlayMove(2, 4, true); // playerX
            board.PlayMove(4, 2, false); // PlayerY            
        }
        
        private static void AssertWinner(int score, Occupied winner)
        {
            Assert.IsTrue(MoveScoreConverter.IsWin(score), "Should have winner");
            Assert.AreEqual(winner, MoveScoreConverter.Winner(score), "Wrong winner");
        }

        private static void DoTestTestNoLingeringWin(Minimax minimax, int depth)
        {
            MinimaxResult result = minimax.DoMinimax(depth, true);

            int bestMoveScore = result.Score;

            // play here to win
            Location expectedMove = new Location(0, 4);
            Assert.AreEqual(expectedMove, result.Move, "Wrong best move at depth " + depth);

            AssertWinner(bestMoveScore, Occupied.PlayerX);
        }

        private static void DoTestTestNoLingering2Win(Minimax minimax, int depth)
        {
            MinimaxResult bestMove = minimax.DoMinimax(depth, true);

            // play here to win
            Location expectedMove = new Location(2, 2);
            Assert.AreEqual(expectedMove, bestMove.Move, "Wrong best move at depth " + depth);
            AssertWinner(bestMove.Score, Occupied.PlayerX);
        }
    }
}
