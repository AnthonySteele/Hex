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
    
    using NUnit.Framework;

    [TestFixture]
    public class SizeSixBoardBugTest
    {
        [Test]
        public void GameLevel5Test()
        {
            HexGame game = new HexGame(6);
            SetupGame(game);

            // get next move for red 
            Location expectedBestMove = new Location(2, 5);

            // level 5 failed
            // choses 0,0.
            // proably failing to pick a near win over a distant one
            // a-b cutoff failure?
            TestBestMove(game, 5, expectedBestMove);
        }

        [Test]
        [Ignore("Very slow test")]
        public void GameIterateTest()
        {
            HexGame game = new HexGame(6);
            SetupGame(game);

            // get next move for red 
            Location[] expectedBestMove = new Location[7];

            expectedBestMove[1] = new Location(2, 5);
            expectedBestMove[2] = new Location(2, 5);
            expectedBestMove[3] = new Location(2, 5);
            expectedBestMove[4] = new Location(2, 5);
            expectedBestMove[5] = new Location(2, 5);
            expectedBestMove[6] = new Location(2, 5);

            // test levels 1-6
            for (int level = 1; level < 7; level++)
            {
                TestBestMove(game, level, expectedBestMove[level]);
            }
        }

        private static void SetupGame(HexGame game)
        {
            // red starts 
            game.Play(0, 5); // red
            game.Play(4, 1);
            game.Play(1, 3); // red
            game.Play(2, 1);
            game.Play(3, 1); // red
            game.Play(2, 2);
            game.Play(3, 2); // red
            game.Play(1, 4);
            game.Play(2, 3); // red
            game.Play(0, 4);
            game.Play(2, 4); // red
            game.Play(1, 5);
        }

        private static void TestBestMove(HexGame game, int level, Location expectedBestMove)
        {
            Minimax hexPlayer = new Minimax(game.Board, game.GoodMoves, new CandidateMovesAll());
            MinimaxResult bestMove = hexPlayer.DoMinimax(level, true);

            // test the location of the move
            Assert.AreEqual(expectedBestMove, bestMove.Move, "Wrong move at level " + level);

            // test the expected score
            if (level >= 3)
            {
                Assert.AreEqual(Occupied.PlayerX,  MoveScoreConverter.Winner(bestMove.Score));
            }
        }
    }
}
