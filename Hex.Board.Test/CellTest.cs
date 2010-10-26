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
    using Hex.Board;
    using NUnit.Framework;

    /// <summary>
    /// Test methods for class Cell
    /// </summary>
    [TestFixture]
    public class CellTest
    {
        [Test]
        public void ConstructorTest()
        {
            Cell cell = new Cell(new Location(23, 34));

            Assert.AreEqual(23, cell.X);
            Assert.AreEqual(34, cell.Y);
            Assert.AreEqual(Occupied.Empty, cell.IsOccupied);
        }

        [Test]
        public void CopyConstructorTest()
        {
            Cell cell = new Cell(new Location(0, 0));
            Cell newCell = new Cell(cell);

            Assert.AreEqual(cell.X, newCell.X);
            Assert.AreEqual(cell.Y, newCell.Y);
            Assert.AreEqual(cell.IsOccupied, newCell.IsOccupied);
        }

        [Test]
        public void ToStringTest()
        {
            Cell cell = new Cell(new Location(0, 0));
            string outValue = cell.ToString();
            Assert.IsFalse(String.IsNullOrEmpty(outValue));
        }

        [Test]
        public void IsEmptyTest()
        {
            Cell cell = new Cell(new Location(0, 0));
            bool returnValue = cell.IsEmpty();
            Assert.IsTrue(returnValue);
        }

        [Test]
        public void IsOccupiedXTest()
        {
            Cell cell = new Cell(new Location(0, 0));

            cell.IsOccupied = Occupied.PlayerX;
            bool returnValue = cell.IsEmpty();
            Assert.IsFalse(returnValue);

            returnValue = cell.IsPlayer(true);
            Assert.IsTrue(returnValue);

            returnValue = cell.IsPlayer(false);
            Assert.IsFalse(returnValue);
        }

        [Test]
        public void IsOccupiedYTest()
        {
            Cell cell = new Cell(new Location(0, 0));

            cell.IsOccupied = Occupied.PlayerY;
            bool returnValue = cell.IsEmpty();
            Assert.IsFalse(returnValue);

            returnValue = cell.IsPlayer(true);
            Assert.IsFalse(returnValue);

            returnValue = cell.IsPlayer(false);
            Assert.IsTrue(returnValue);
        }

        [Test]
        public void ToStringWithPlayerX()
        {
            Cell cell = new Cell(new Location(2, 3));
            cell.IsOccupied = Occupied.PlayerX;

            string result = cell.ToString();

            Assert.AreEqual("2,3 X", result);
        }
    }
}
