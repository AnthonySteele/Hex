namespace Hex.Engine.Test.PathLength
{
    using System;
    using System.Collections.Generic;
    
    using Hex.Board;
    using Hex.Engine.PathLength;
    using NUnit.Framework;
    
    /// <summary>
    /// Test the new path length alg.
    /// It should return the same results as the standard alg.
    /// Hopefully in less time
    /// </summary>
    [TestFixture]
    public class PathLengthAStarTest
    {
        [Test]
        public void BoardsTest()
        {
            const int RandSeed = 42;
            const int BoardCount = 100;
            const int BoardSize = 10;

            // repeatable random
            Random rands = new Random(RandSeed);

            for (int boardIndex = 0; boardIndex < BoardCount; boardIndex++)
            {
                HexBoard board = RandomBoard(BoardSize, 10 + boardIndex, rands);
                Assert.IsNotNull(board, "Board is null " + boardIndex);
                CheckPathLength(board, boardIndex);
            }
        }

        [Test]
        public void SpeedTest()
        {
            const int RandSeed = 24;
            const int BoardCount = 100;
            const int BoardSize = 12;

            Random rands = new Random(RandSeed);
            HexBoard[] boards = new HexBoard[BoardCount];

            // make some boards
            for (int loopIndex = 0; loopIndex < BoardCount; loopIndex++)
            {
                boards[loopIndex] = RandomBoard(BoardSize, 10 + loopIndex, rands);
            }

            // time it the old way
            DateTime oldStart = DateTime.Now;

            foreach (HexBoard board in boards)
            {
                PathLengthLoop oldPath = new PathLengthLoop(board);
                oldPath.PlayerScore(true);
                oldPath.PlayerScore(false);
            }

            DateTime oldEnd = DateTime.Now;

            // and the new way
            DateTime newStart = DateTime.Now;

            foreach (HexBoard board in boards)
            {
                PathLengthAStar newPath = new PathLengthAStar(board);
                newPath.PlayerScore(true);
                newPath.PlayerScore(false);
            }

            DateTime newEnd = DateTime.Now;

            TimeSpan oldDuration = oldEnd - oldStart;
            TimeSpan newDuration = newEnd - newStart;

            double ratio = newDuration.Milliseconds / (double)oldDuration.Milliseconds;

            Assert.IsTrue(ratio < 1.0);
        }

        // [Test]
        // this is really only run  to inspect the ratios
        // which are consistently around 0.2
        // so the A* version takes about 1/5 of the time of the loops 
        public void TestTimesPerCellFilled()
        {
            const int MaxCellsFilled = 50;
            Dictionary<int, double> ratios = new Dictionary<int, double>();

            for (int loopIndex = 0; loopIndex < MaxCellsFilled; loopIndex++)
            {
                double ratio = GetRatio(loopIndex);

                ratios.Add(loopIndex, ratio);
            }

            Assert.IsNotNull(ratios);
        }

        private static double GetRatio(int cellFilledCount)
        {
            const int RandSeed = 24;
            const int BoardCount = 50;
            const int BoardSize = 12;

            Random rands = new Random(RandSeed + cellFilledCount);
            HexBoard[] boards = new HexBoard[BoardCount];

            // make some boards
            for (int loopIndex = 0; loopIndex < BoardCount; loopIndex++)
            {
                boards[loopIndex] = RandomBoard(BoardSize, cellFilledCount, rands);
            }

            // time it the old way
            DateTime oldStart = DateTime.Now;

            foreach (HexBoard board in boards)
            {
                PathLengthLoop oldPath = new PathLengthLoop(board);
                oldPath.PlayerScore(true);
                oldPath.PlayerScore(false);
            }

            DateTime oldEnd = DateTime.Now;

            // and the new way
            DateTime newStart = DateTime.Now;

            foreach (HexBoard board in boards)
            {
                PathLengthAStar newPath = new PathLengthAStar(board);
                newPath.PlayerScore(true);
                newPath.PlayerScore(false);
            }

            DateTime newEnd = DateTime.Now;

            TimeSpan oldDuration = oldEnd - oldStart;
            TimeSpan newDuration = newEnd - newStart;

            double ratio = newDuration.Milliseconds / (double)oldDuration.Milliseconds;

            return ratio;
        }

        private static HexBoard RandomBoard(int size, int moves, Random rands)
        {
            HexBoard result = new HexBoard(size);

            for (int loopCount = 0; loopCount < moves; loopCount++)
            {
                int x = rands.Next(size);
                int y = rands.Next(size);

                if (result.GetCellOccupiedAt(x, y) == Occupied.Empty)
                {
                    result.PlayMove(x, y, (loopCount % 2 == 0));
                }
            }

            return result;
        }

        private static void CheckPathLength(HexBoard board, int boardIndex)
        {
            // first do it with the old path length
            PathLengthLoop oldPath = new PathLengthLoop(board);
            int oldX = oldPath.PlayerScore(true);
            int oldY = oldPath.PlayerScore(false);

            // now the new
            PathLengthAStar newPath = new PathLengthAStar(board)
                {
                    UseNeighbours2 = false
                };

            int newX = newPath.PlayerScore(true);
            int newY = newPath.PlayerScore(false);

            Assert.AreEqual(oldX, newX, "X not equal at board index" + boardIndex);
            Assert.AreEqual(oldY, newY, "Y not equal at board index" + boardIndex);
        }
    }
}
