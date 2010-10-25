namespace Hex.Engine.Test.Slow
{
    using System.Collections.Generic;
    using System.Linq;

    using Hex.Board;
    using Hex.Engine.CandiateMoves;
    using Hex.Engine.Lookahead;

    using NUnit.Framework;

    [TestFixture]
    public class MinimaxCutoffTest
    {
        [Test]
        public void WinEndsTheSearchForPlayerX()
        {
            HexBoard board = new HexBoard(5);
            PlayToWinInOneMove(board, true);
            
            Minimax minimax = MakeMinimaxForBoard(board);
            const int SearchDepth = 4;

            MinimaxResult bestMove = minimax.DoMinimax(SearchDepth, true);

            Location win = new Location(0, 4);
            Assert.AreEqual(win, bestMove.Move, "wrong win location");

            // test that locations after 0, 4 aren't even looked at. A win ends the search
            IList<Location> locationsExamined = minimax.DebugDataItems
                .Where(d => d.Lookahead == SearchDepth)
                .Select(d => d.Location).ToList();

            Assert.IsTrue(locationsExamined.Count > 0, "No locations examined");
            Assert.IsTrue(locationsExamined.Contains(win), "Locations examined does not contain win");
            Location unexpected44 = new Location(4, 4);
            Assert.IsFalse(locationsExamined.Contains(unexpected44), "Should not have examined location " + unexpected44 + " after win");

            Location unexpected23 = new Location(2, 3);
            Assert.IsFalse(locationsExamined.Contains(unexpected23), "Should not have examined location " + unexpected23 + " after win");
        }

        [Test]
        public void WinEndsTheSearchForPlayerY()
        {
            HexBoard board = new HexBoard(5);
            PlayToWinInOneMove(board, false);

            Minimax minimax = MakeMinimaxForBoard(board);
            const int SearchDepth = 4;

            MinimaxResult bestMove = minimax.DoMinimax(SearchDepth, false);
            Location win = new Location(0, 3);

            Assert.AreEqual(win, bestMove.Move, "wrong win location");

            // do: test that locations after 0, 4 aren't even looked at. A win ends the search
            IList<Location> locationsExamined = minimax.DebugDataItems
                .Where(d => d.Lookahead == SearchDepth)
                .Select(d => d.Location).ToList();

            Assert.IsTrue(locationsExamined.Count > 0, "No locations examined");
            Assert.IsTrue(locationsExamined.Contains(win), "Locations examined does not contain win");
            Location unexpected = new Location(4, 4);
            Assert.IsFalse(locationsExamined.Contains(unexpected), "Should not have examined location " + unexpected + " after win");
        }

        [Test]
        public void GoodMoveAtLevel2()
        {
            HexBoard board = new HexBoard(6);
            PlayToWinInThreeMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);

            MinimaxResult bestMove = minimax.DoMinimax(2, true);

            Location win = new Location(2, 2);
            Assert.AreEqual(win, bestMove.Move, "Wrong play location");
        }

        [Test]
        public void GoodMoveAtLevel3()
        {
            HexBoard board = new HexBoard(6);
            PlayToWinInThreeMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);

            MinimaxResult bestMove = minimax.DoMinimax(3, true);
            Location win = new Location(1, 3);

            Assert.AreEqual(win, bestMove.Move, "Wrong play location");
        }

        [Test]
        public void RightMoveAtLevel4()
        {
            HexBoard board = new HexBoard(6);
            PlayToWinInThreeMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);

            MinimaxResult bestMove = minimax.DoMinimax(4, true);

            Location win = new Location(1, 3);

            Assert.AreEqual(win, bestMove.Move, "Wrong play location");
        }

        [Test]
        public void RightMoveAtLevel5PlayerX()
        {
            HexBoard board = new HexBoard(6);
            PlayToWinInThreeMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            
            MinimaxResult bestMove = minimax.DoMinimax(5, true);

            Location win = new Location(1, 3);

            Assert.AreEqual(win, bestMove.Move, "Wrong play location");
        }

        [Test]
        public void RightMoveAtLevel5PlayerY()
        {
            HexBoard board = new HexBoard(6);
            PlayToWinInThreeMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);

            MinimaxResult bestMove = minimax.DoMinimax(5, false);

            List<Location> playerYWinningLocations = new List<Location>
                {
                    new Location(1, 3),
                    new Location(2, 2)
                };
            Assert.IsTrue(playerYWinningLocations.Contains(bestMove.Move), "Wrong play location");
        }

        private static void PlayToWinInOneMove(HexBoard board, bool playerX)
        {
            // one move at 0,4 to win
            board.PlayMove(4, 0, playerX); 
            board.PlayMove(3, 1, playerX); 
            board.PlayMove(2, 2, playerX); 
            board.PlayMove(1, 3, playerX); 
        }

        private static void PlayToWinInThreeMoves(HexBoard board)
        {
            // win in 3 moves
            board.PlayMove(2, 0, true); // playerX
            board.PlayMove(2, 1, true); // playerX
            board.PlayMove(2, 4, true); // playerX
            board.PlayMove(2, 5, true); // playerX            

            board.PlayMove(0, 2, false); // playery
            board.PlayMove(3, 2, false); // playery
            board.PlayMove(4, 2, false); // playery
            board.PlayMove(5, 2, false); // playery            
        }

        private static Minimax MakeMinimaxForBoard(HexBoard board)
        {
            GoodMoves goodMoves = new GoodMoves();
            ICandidateMoves candidateMovesFinder = new CandidateMovesAll();

            Minimax result = new Minimax(board, goodMoves, candidateMovesFinder);
            result.GenerateDebugData = true;

            return result;
        }
    }
}
