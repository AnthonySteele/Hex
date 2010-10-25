namespace Hex.Board.Test
{
    using System.Collections.Generic;

    using Hex.Board;
    using NUnit.Framework;

    /// <summary>
    ///  Test the neighbours functions in HexBoardNeighbours
    /// </summary>
    [TestFixture]
    public class BoardNeighboursTest
    {
        [Test]
        public void CreateTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(5);

            Assert.IsNotNull(testBoard);
            Assert.AreEqual(5, testBoard.BoardSize);
        }

        [Test]
        public void IsOnBoardTrueTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(5);

            Assert.IsTrue(testBoard.IsOnBoard(new Location(0, 0)));
            Assert.IsTrue(testBoard.IsOnBoard(new Location(2, 2)));
            Assert.IsTrue(testBoard.IsOnBoard(new Location(0, 4)));
            Assert.IsTrue(testBoard.IsOnBoard(new Location(4, 0)));
            Assert.IsTrue(testBoard.IsOnBoard(new Location(4, 4)));
        }

        [Test]
        public void IsOnBoardFalseTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(5);

            Assert.IsFalse(testBoard.IsOnBoard(new Location(-1, -1)));
            Assert.IsFalse(testBoard.IsOnBoard(new Location(2, -1)));
            Assert.IsFalse(testBoard.IsOnBoard(new Location(0, 5)));
            Assert.IsFalse(testBoard.IsOnBoard(new Location(5, 0)));
            Assert.IsFalse(testBoard.IsOnBoard(new Location(-1, 5)));
        }

        [Test]
        public void NeighboursMiddleTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(5, 5);

            Location[] outValue = testBoard.Neighbours(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(6, outValue.Length);

            Assert.AreEqual(6, testBoard.NeighbourCount(inValue));
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursOriginTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(0, 0);

            Location[] outValue = testBoard.Neighbours(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(2, outValue.Length);
            Assert.AreEqual(2, testBoard.NeighbourCount(inValue));
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursFarTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(9, 9);

            Location[] outValue = testBoard.Neighbours(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(2, outValue.Length);
            Assert.AreEqual(2, testBoard.NeighbourCount(inValue));
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursEdgeTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(5, 0);

            Location[] outValue = testBoard.Neighbours(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(4, outValue.Length);
            Assert.AreEqual(4, testBoard.NeighbourCount(inValue));
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursPoleTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(9, 0);

            Location[] outValue = testBoard.Neighbours(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(3, outValue.Length);
            Assert.AreEqual(3, testBoard.NeighbourCount(inValue));
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursOffTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(5);
            Location inValue = new Location(5, 0);

            Location[] outValue = testBoard.Neighbours(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(0, outValue.Length);
            Assert.AreEqual(0, testBoard.NeighbourCount(inValue));
        }

        private static void TestNeighbours(HexBoardNeighbours testBoard, Location testLoc, IEnumerable<Location> neighbours)
        {
            TestOnBoard(testBoard, testLoc);

            Location offBoard = new Location(testBoard.BoardSize, testBoard.BoardSize - 1);

            foreach (Location neighbour in neighbours)
            {
                // check is on board
                TestOnBoard(testBoard, neighbour);

                // check that the cells are actuallly neighbours
                Assert.IsTrue(testBoard.AreNeighbours(testLoc, neighbour));

                // not neighbours with off-board cell
                Assert.IsFalse(testBoard.AreNeighbours(testLoc, offBoard));

                // not neighbours with self
                Assert.IsFalse(testBoard.AreNeighbours(testLoc, testLoc));
            }
        }

        private static void TestOnBoard(HexBoardNeighbours testBoard, Location neighbour)
        {
            Assert.IsTrue(testBoard.IsOnBoard(neighbour));

            Assert.GreaterOrEqual(neighbour.X, 0);
            Assert.GreaterOrEqual(neighbour.Y, 0);

            Assert.Less(neighbour.X, testBoard.BoardSize);
            Assert.Less(neighbour.Y, testBoard.BoardSize);
        }
    }
}
