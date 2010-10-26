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
    using System;
    using System.Collections.Generic;

    using Hex.Board;
    using NUnit.Framework;

    /// <summary>
    /// Tests on class HexBoard, methods to do with neighbours
    /// </summary>
    [TestFixture]
    public class BoardGetNeighboursTest
    {
        private const int BoardSize = 10;

        [Test]
        public void BoardNeighboursTest()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            for (int x = 0; x < hexBoard.Size; x++)
            {
                for (int y = 0; y < hexBoard.Size; y++)
                {
                    Cell cell = hexBoard.GetCellAt(x, y);

                    var neigbours = hexBoard.Neighbours(cell);

                    Assert.IsNotNull(neigbours);

                    /* all cells have at least 2 neighbours, and at most 6 */
                    Assert.GreaterOrEqual(neigbours.Length, 2);
                    Assert.LessOrEqual(neigbours.Length, 6);

                    /* cells not on the edge will have 6 neighbours */
                    if ((x > 0) && (y > 0) && (x < (hexBoard.Size - 1)) && (y < (hexBoard.Size - 1)))
                    {
                        Assert.AreEqual(neigbours.Length, 6);
                    }

                    NoNullsInCellArray(neigbours);

                    foreach (Cell neibCell in neigbours)
                    {
                        DoTestNeighbour(cell, neibCell, hexBoard);
                    }
                }
            }
        }

        [Test]
        public void BoardNeighbours2Test()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            for (int x = 0; x < hexBoard.Size; x++)
            {
                for (int y = 0; y < hexBoard.Size; y++)
                {
                    Cell cell = hexBoard.GetCellAt(x, y);

                    var neigbours2 = hexBoard.Neighbours2(cell);

                    Assert.IsNotNull(neigbours2);

                    /* all cells have at least 1 neighbours, and at most 6 */
                    Assert.Greater(neigbours2.GetLength(0), 0);
                    Assert.Less(neigbours2.GetLength(0), 7);

                    foreach (Cell[] triplet in neigbours2)
                    {
                        TestNeighbour2Triplet(cell, triplet);
                    }
                }
            }
        }

        [Test]
        public void BetweenEdgeTest()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            for (int x = 0; x < hexBoard.Size; x++)
            {
                for (int y = 0; y < hexBoard.Size; y++)
                {
                    Location testLoc = new Location(x, y);

                    Cell[] resultX = hexBoard.BetweenEdge(testLoc, true);

                    if (y == 1)
                    {
                        // second or second-last row
                        if (x < hexBoard.Size - 1)
                        {
                            Assert.AreEqual(2, resultX.Length, testLoc.ToString());
                        }
                    }
                    else if (y == hexBoard.Size - 2)
                    {
                        // second or second-last row
                        if (x > 0)
                        {
                            Assert.AreEqual(2, resultX.Length, testLoc.ToString());
                        }
                    }
                    else
                    {
                        Assert.AreEqual(0, resultX.Length, testLoc.ToString());
                    }

                    Cell[] resultY = hexBoard.BetweenEdge(testLoc, false);

                    if (x == 1)
                    {
                        // second or second-last row
                        if (y < hexBoard.Size - 1)
                        {
                            Assert.AreEqual(2, resultY.Length, testLoc.ToString());
                        }
                    }
                    else if (x == hexBoard.Size - 2)
                    {
                        // second or second-last row
                        if (y > 0)
                        {
                            Assert.AreEqual(2, resultY.Length, testLoc.ToString());
                        }
                    }
                    else
                    {
                        Assert.AreEqual(0, resultY.Length, testLoc.ToString());
                    }
                }
            }
        }

        [Test]
        public void TestGetEmptyNeighbours2()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            // get the neighbours2 with nothing inbetween
            Cell focus = hexBoard.GetCellAt(3, 3);
            Cell[][] neighbours2EmptyBoard = hexBoard.EmptyNeighbours2(focus);

            // should be six sets
            Assert.AreEqual(6, neighbours2EmptyBoard.Length);

            // playing an ajoining cell removes 2 sets
            hexBoard.PlayMove(3, 4, false);
            Cell[][] neighbours2PlayedCell = hexBoard.EmptyNeighbours2(focus);

            Assert.AreEqual(4, neighbours2PlayedCell.Length);
        }

        [Test]
        public void TestGetEmptyNeighbours2Edge()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            // get the neighbours2 with nothing in between
            Cell focus = hexBoard.GetCellAt(3, 0);
            Cell[][] neighbours2EmptyBoard = hexBoard.EmptyNeighbours2(focus);

            Assert.AreEqual(3, neighbours2EmptyBoard.Length);
        }

        #region helpers

        private static void NoNullsInCellArray(IEnumerable<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                Assert.IsNotNull(cell);
            }
        }
        
        private static void TestNeighbour2Triplet(Cell cell, Cell[] triplet)
        {
            Assert.AreEqual(3, triplet.Length);
            NoNullsInCellArray(triplet);

            var testBoard = new HexBoardNeighbours(BoardSize);
            var testLoc = cell.Location;
            var neighbour2 = triplet[0].Location;
            var between1 = triplet[1].Location;
            var between2 = triplet[2].Location;

            Assert.IsTrue(testBoard.AreNeighbours(between1, between2));

            Assert.IsTrue(testBoard.AreNeighbours(testLoc, between1));
            Assert.IsTrue(testBoard.AreNeighbours(testLoc, between2));

            Assert.IsTrue(testBoard.AreNeighbours(neighbour2, between1));
            Assert.IsTrue(testBoard.AreNeighbours(neighbour2, between2));

            // but not neighbours to each other
            Assert.IsFalse(testBoard.AreNeighbours(testLoc, neighbour2));
        }

        private static void DoTestNeighbour(Cell cell, Cell neibCell, HexBoard board)
        {
            Assert.IsTrue(cell.Location.ManhattanDistance(neibCell.Location) < 3, "Neigbour is too far away");

            var neighboursTest = new HexBoardNeighbours(BoardSize);

            Assert.IsTrue(neighboursTest.AreNeighbours(cell.Location, neibCell.Location));

            // reflexive. If B is a neighbour of A, then B's neighbours must include A
            var neibs = board.Neighbours(neibCell);
            int index = Array.IndexOf(neibs, cell);
            Assert.IsTrue(index >= 0, "Cell is not neighbour's neighbour");
        }

        #endregion
    }
}
