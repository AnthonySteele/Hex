//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Engine.Test.Slow
{
    using Hex.Board;
    using Hex.Engine.CandiateMoves;
    using Hex.Engine.Lookahead;
    using Hex.Engine.PathLength;

    using NUnit.Framework;

    /// <summary>
    /// Test methods for class Minimax on a 3 by 3 board
    /// </summary>
    [TestFixture]
    public class Minimax3By3Test
    {
        [Test]
        public void TestCalculateMoveLookahead1Player1()
        {
            HexBoard board = new HexBoard(3);
            PlayFourMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            MinimaxResult firstPlayerResult = minimax.DoMinimax(1, true);

            Location expectedMove = new Location(1, 1);
            Assert.AreEqual(expectedMove, firstPlayerResult.Move, "playerOneBestMove");
            AssertWinner(firstPlayerResult.Score, Occupied.PlayerX);
        }

        [Test]
        public void TestCalculateMoveLookahead1Player2()
        {
            HexBoard board = new HexBoard(3);
            PlayFourMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            MinimaxResult secondPlayerResult = minimax.DoMinimax(1, false);

            Location expectedMove = new Location(1, 1);
            Assert.AreEqual(expectedMove, secondPlayerResult.Move, "playerTwoBestMove");
            AssertWinner(secondPlayerResult.Score, Occupied.PlayerY);
        }

        [Test]
        public void TestCalculateMove2Situation()
        {
            HexBoard board = new HexBoard(3);
            PlayTwoMoves(board);

            // test score at this point
            PathLengthLoop pathLength = new PathLengthLoop(board);
            int playerScoreX = pathLength.PlayerScore(true);
            Assert.AreEqual(2, playerScoreX, "playerScoreX");

            int playerScoreY = pathLength.PlayerScore(false);
            Assert.AreEqual(2, playerScoreY, "playerScoreY");

            // no advantage
            int moveScore = pathLength.SituationScore();
            Assert.AreEqual(0, moveScore, "moveScore");
        }

        [Test]
        public void TestCalculateMove2SituationScoreMiddle()
        {
            HexBoard board = new HexBoard(3);
            PlayTwoMoves(board);

            // playing middle gets adavantage
            board.PlayMove(1, 1, true);

            PathLengthLoop pathLength = new PathLengthLoop(board);
            int moveScore = pathLength.SituationScore();
            Assert.AreEqual(1, moveScore);

            board.GetCellAt(1, 1).IsOccupied = Occupied.PlayerY;
            moveScore = pathLength.SituationScore();
            Assert.AreEqual(-1, moveScore);

            board.GetCellAt(1, 1).IsOccupied = Occupied.Empty;
        }

        [Test]
        public void TestCalculateMove1MinimaxPlayerX()
        {
            HexBoard board = new HexBoard(3);
            PlayTwoMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            MinimaxResult firstPlayerResult = minimax.DoMinimax(1, true);

            Location expectedPlay = new Location(0, 2);
            Assert.AreEqual(expectedPlay, firstPlayerResult.Move, "Wrong play location");
            Assert.IsFalse(MoveScoreConverter.IsWin(firstPlayerResult.Score));
        }

        [Test]
        public void TestCalculateMove1MinimaxPlayerY()
        {
            HexBoard board = new HexBoard(3);
            PlayTwoMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            MinimaxResult secondPlayerResult = minimax.DoMinimax(1, false);

            Location expectedPlay = new Location(0, 2);
            Assert.AreEqual(expectedPlay, secondPlayerResult.Move, "Wrong play location");
            Assert.IsFalse(MoveScoreConverter.IsWin(secondPlayerResult.Score));
        }
        
        [Test]
        public void TestCalculateMove2MinimaxPlayerX()
        {
            HexBoard board = new HexBoard(3);
            PlayTwoMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            MinimaxResult firstPlayerResult = minimax.DoMinimax(2, true);

            Location expectedPlay = new Location(0, 2);
            Assert.AreEqual(expectedPlay, firstPlayerResult.Move, "Wrong play location");
            Assert.IsFalse(MoveScoreConverter.IsWin(firstPlayerResult.Score));
        }

        [Test]
        public void TestCalculateMove2MinimaxPlayerY()
        {
            HexBoard board = new HexBoard(3);
            PlayTwoMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            MinimaxResult secondPlayerResult = minimax.DoMinimax(2, false);

            Location expectedPlay = new Location(0, 2);
            Assert.AreEqual(expectedPlay, secondPlayerResult.Move, "Wrong play location");
            Assert.IsFalse(MoveScoreConverter.IsWin(secondPlayerResult.Score));
        }

        [Test]
        public void TestCalculateMove3MinimaxPlayerX()
        {
            HexBoard board = new HexBoard(3);
            PlayTwoMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            MinimaxResult firstPlayerResult = minimax.DoMinimax(3, true);
            Location bestMoveLocation = firstPlayerResult.Move;
            int moveScore = firstPlayerResult.Score;

            Location exectedWin = new Location(0, 2);
            Assert.AreEqual(exectedWin, bestMoveLocation, "Wrong win location");
            AssertWinner(moveScore, Occupied.PlayerX);
        }

        [Test]
        public void TestCalculateMove3MinimaxPlayerY()
        {
            HexBoard board = new HexBoard(3);
            PlayTwoMoves(board);

            Minimax minimax = MakeMinimaxForBoard(board);
            MinimaxResult secondPlayerResult = minimax.DoMinimax(3, false);
            Location secondPlayerMoveLocation = secondPlayerResult.Move;
            int moveScore = secondPlayerResult.Score;

            Location exectedWin = new Location(0, 2);
            Assert.AreEqual(exectedWin, secondPlayerMoveLocation, "Wrong second player location");
            AssertWinner(moveScore, Occupied.PlayerY);
        }

        [Test]
        public void TestCalculateMove4()
        {
            HexBoard board = new HexBoard(3);

            board.PlayMove(0, 0, true); // playerX
            board.PlayMove(0, 1, false); // PlayerY
            board.PlayMove(0, 2, true); // playerX

            board.PlayMove(1, 0, false); // PlayerY
            board.PlayMove(1, 1, true); // PlayerX
            board.PlayMove(1, 2, false); // playerY

            Minimax minimax = MakeMinimaxForBoard(board);

            // test score at this point
            PathLengthLoop pathLength = new PathLengthLoop(board);
            int playerScore = pathLength.PlayerScore(true);
            Assert.AreEqual(1, playerScore);

            playerScore = pathLength.PlayerScore(false);
            Assert.AreEqual(1, playerScore);

            // only 3 cells are vacant
            minimax.DoMinimax(1, false);
            minimax.DoMinimax(1, true);

            minimax.DoMinimax(2, false);
            minimax.DoMinimax(2, true);

            minimax.DoMinimax(3, false);
            minimax.DoMinimax(3, true);

            minimax.DoMinimax(4, false);
            minimax.DoMinimax(4, true);

            minimax.DoMinimax(5, false);
            minimax.DoMinimax(5, true);
        }

        private static void PlayFourMoves(HexBoard playBoard)
        {
            // set up so the best move should be that the middle - a winning move for either player
            // can see this just looking at one move
            playBoard.PlayMove(1, 0, true); // PlayerX
            playBoard.PlayMove(2, 1, false); // PlayerY

            playBoard.PlayMove(0, 1, false); // PlayerX
            playBoard.PlayMove(1, 2, true); // PlayerY
        }

        private static void PlayTwoMoves(HexBoard playBoard)
        {
            // best move (and winning move ) is the middle (1,1) or corner (0, 2)
            // but it takes lookahead or 3 or more to see that
            // Should be quick to calc since there's only 9 cells
            // Search tree is not broad, we can go deep
            playBoard.PlayMove(1, 0, true); // PlayerX
            playBoard.PlayMove(2, 1, false); // PlayerY
        }

        private static Minimax MakeMinimaxForBoard(HexBoard board)
        {
            GoodMoves goodMoves = new GoodMoves();
            ICandidateMoves candidateMovesFinder = new CandidateMovesAll();

            Minimax result = new Minimax(board, goodMoves, candidateMovesFinder);
            result.GenerateDebugData = true;

            return result;
        }

        private static void AssertWinner(int score, Occupied winner)
        {
            Assert.IsTrue(MoveScoreConverter.IsWin(score), "Should have winner");
            Assert.AreEqual(winner, MoveScoreConverter.Winner(score), "Wrong winner");
        }
    }
}
