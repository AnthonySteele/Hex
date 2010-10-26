//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Board.Test
{
    using System.Collections.Generic;

    using Hex.Board;
    using NUnit.Framework;

    /// <summary>
    /// Test the neighbours2 functions in HexBoardNeighbours
    /// </summary>
    [TestFixture]
    public class BoardNeighbours2Test
    {
        [Test]
        public void NeighboursMiddleTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(5, 5);

            Location[][] outValue = testBoard.Neighbours2(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(6, outValue.Length);
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursOriginTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(0, 0);

            Location[][] outValue = testBoard.Neighbours2(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(1, outValue.Length);
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursNearOriginTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(1, 1);

            Location[][] outValue = testBoard.Neighbours2(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(4, outValue.Length);
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursFarTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(9, 9);

            Location[][] outValue = testBoard.Neighbours2(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(1, outValue.Length);
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursEdgeTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(5, 0);

            Location[][] outValue = testBoard.Neighbours2(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(3, outValue.Length);
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursNearEdgeTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(10);
            Location inValue = new Location(5, 1);

            Location[][] outValue = testBoard.Neighbours2(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(5, outValue.Length);
            TestNeighbours(testBoard, inValue, outValue);
        }

        [Test]
        public void NeighboursOffTest()
        {
            HexBoardNeighbours testBoard = new HexBoardNeighbours(5);
            Location inValue = new Location(5, 0);

            Location[][] outValue = testBoard.Neighbours2(inValue);

            Assert.IsNotNull(outValue);
            Assert.AreEqual(0, outValue.Length);
        }

        private static void TestNeighbours(HexBoardNeighbours testBoard, Location testLoc, IEnumerable<Location[]> neighbourGroups)
        {
            TestOnBoard(testBoard, testLoc);

            foreach (Location[] neighbours in neighbourGroups)
            {
                Location neighbour2 = neighbours[0];
                Location between1 = neighbours[1];
                Location between2 = neighbours[2];

                TestOnBoard(testBoard, neighbour2);
                TestOnBoard(testBoard, between1);
                TestOnBoard(testBoard, between2);

                // that the betweens are neighbours of start, end eand each other
                Assert.IsTrue(testBoard.AreNeighbours(between1, between2));

                Assert.IsTrue(testBoard.AreNeighbours(testLoc, between1));
                Assert.IsTrue(testBoard.AreNeighbours(testLoc, between2));

                Assert.IsTrue(testBoard.AreNeighbours(neighbour2, between1));
                Assert.IsTrue(testBoard.AreNeighbours(neighbour2, between2));

                // but not neighbours of each other
                Assert.IsFalse(testBoard.AreNeighbours(testLoc, neighbour2));
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
