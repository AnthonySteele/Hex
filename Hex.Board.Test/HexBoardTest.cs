namespace Hex.Board.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hex.Board;
    using NUnit.Framework;

    /// <summary>
    /// Test methods for class HexBoard
    /// </summary>
    [TestFixture]
    public class HexBoardTest
    {
        private const int BoardSize = 10;

        [Test]
        public void ConstructorSetsProperties()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            Assert.IsNotNull(hexBoard);
            Assert.AreEqual(BoardSize, hexBoard.Size);
            Assert.AreEqual(0, hexBoard.MovesPlayedCount);
        }

        [Test]
        public void MovesPlayedCountIncrementsAfterMove()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            Assert.AreEqual(0, hexBoard.MovesPlayedCount);
            hexBoard.PlayMove(1, 1, true);
            Assert.AreEqual(1, hexBoard.MovesPlayedCount);
        }

        [Test]
        public void MoveSetsCellOccupied()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            Assert.AreEqual(Occupied.Empty, hexBoard.GetCellOccupiedAt(1, 1));

            hexBoard.PlayMove(1, 1, true);
            Assert.AreEqual(Occupied.PlayerX, hexBoard.GetCellAt(1, 1).IsOccupied);
            Assert.AreEqual(Occupied.PlayerX, hexBoard.GetCellOccupiedAt(1, 1));
        }

        [Test]
        public void BoardLayoutTest()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            for (int x = 0; x < hexBoard.Size; x++)
            {
                for (int y = 0; y < hexBoard.Size; y++)
                {
                    Cell cell = hexBoard.GetCellAt(x, y);

                    Assert.IsNotNull(cell);
                    Assert.IsTrue(cell.X == x);
                    Assert.IsTrue(cell.Y == y);
                }
            }
        }

        [Test]
        public void RowsTest()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            Cell[] row = hexBoard.Row(true, true);
            DoTestRow(row, hexBoard.Size);
            
            row = hexBoard.Row(true, false);
            DoTestRow(row, hexBoard.Size);

            row = hexBoard.Row(false, true);
            DoTestRow(row, hexBoard.Size);

            row = hexBoard.Row(false, false);
            DoTestRow(row, hexBoard.Size);
        }

        [Test]
        public void FirstRowForPlayerX()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            Cell[] row = hexBoard.Row(true, true);

            foreach (Cell cell in row)
            {
                Assert.AreEqual(0, cell.Y);
            }            
        }

        [Test]
        public void FirstRowForPlayerY()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            Cell[] row = hexBoard.Row(false, true);

            foreach (Cell cell in row)
            {
                Assert.AreEqual(0, cell.X);
            }
        }

        [Test]
        public void LastRowForPlayerX()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            Cell[] row = hexBoard.Row(true, false);

            foreach (Cell cell in row)
            {
                Assert.AreEqual(BoardSize - 1, cell.Y);
            }
        }

        [Test]
        public void LastRowForPlayerY()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            Cell[] row = hexBoard.Row(false, false);

            foreach (Cell cell in row)
            {
                Assert.AreEqual(BoardSize - 1, cell.X);
            }
        }

        [Test]
        public void ClearTest()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            // set a cell
            hexBoard.PlayMove(1, 1, true);
            Assert.AreEqual(Occupied.PlayerX, hexBoard.GetCellOccupiedAt(1, 1));

            Location loc11 = new Location(1, 1);
            Assert.AreEqual(Occupied.PlayerX, hexBoard.GetCellOccupiedAt(1, 1));
            Assert.AreEqual(Occupied.PlayerX, hexBoard.GetCellOccupiedAt(loc11));

            // reset it
            hexBoard.Clear();
            Assert.AreEqual(Occupied.Empty, hexBoard.GetCellOccupiedAt(1, 1));
            Assert.AreEqual(Occupied.Empty, hexBoard.GetCellOccupiedAt(loc11));
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void DuplicatePlayTest()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            // set a cell
            hexBoard.PlayMove(1, 1, true);
            hexBoard.PlayMove(1, 1, true);
        }

        [Test]
        public void TestEqualsFail()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            Assert.IsFalse(hexBoard.Equals(null));
            Assert.IsFalse(hexBoard.Equals(3));
            Assert.IsFalse(hexBoard.Equals("Hello hex board"));
            Assert.IsFalse(hexBoard.Equals(new HexBoard(3)));
        }

        [Test]
        public void BoardCopyTest()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);
            HexBoard copyBoard = new HexBoard(hexBoard);

            Assert.IsTrue(copyBoard.Equals(hexBoard));

            copyBoard.PlayMove(0, 0, true);

            Assert.IsFalse(copyBoard.Equals(hexBoard));
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CopyFailTest()
        {
            HexBoard sourceBoard = new HexBoard(4);
            HexBoard copyBoard = new HexBoard(5);
            copyBoard.CopyStateFrom(sourceBoard);
        }

        [Test]
        public void ToStringTest()
        {
            HexBoard board = new HexBoard(4);
            board.PlayMove(1, 1, true);

            string out1 = board.ToString();
            Assert.IsFalse(String.IsNullOrEmpty(out1));

            board.PlayMove(2, 2, false);

            string out2 = board.ToString();
            Assert.IsFalse(String.IsNullOrEmpty(out2));

            Assert.AreNotEqual(out1, out2);
        }

        [Test]
        public void EmptyCellsTest()
        {
            const int SmallBoardSize = 4;
            const int BoardCellCount = SmallBoardSize * SmallBoardSize;
            HexBoard source = new HexBoard(SmallBoardSize);

            Location[] emptyCells1 = source.EmptyCells().ToArray();

            Assert.IsNotNull(emptyCells1);
            Assert.AreEqual(BoardCellCount, emptyCells1.Length);

            source.PlayMove(2, 2, false);
            Location[] emptyCells2 = source.EmptyCells().ToArray();

            Assert.IsNotNull(emptyCells2);
            Assert.AreEqual(BoardCellCount - 1, emptyCells2.Length);
        }

        [Test]
        public void GetCellAtTest()
        {
            const int SmallBoardSize = 4;
            HexBoard source = new HexBoard(SmallBoardSize);

            for (int x = 0; x < SmallBoardSize; x++)
            {
                for (int y = 0; y < SmallBoardSize; y++)
                {
                    Cell cell = source.GetCellAt(x, y);

                    Assert.AreEqual(x, cell.X);
                    Assert.AreEqual(y, cell.Y);
                }
            }
        }

        [Test]
        public void GetCellAtArrayNullTest()
        {
            const int SmallBoardSize = 4;
            HexBoard source = new HexBoard(SmallBoardSize);

            Location[] locs = null;

            Cell[] result = source.GetCellAt(locs);

            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void GetCellAtArrayOneTest()
        {
            const int SmallBoardSize = 4;
            HexBoard source = new HexBoard(SmallBoardSize);

            Location[] locs = new Location[1];
            locs[0] = new Location(1, 1);

            Cell[] result = source.GetCellAt(locs);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0].Location.X);
            Assert.AreEqual(1, result[0].Location.Y);
        }

        [Test]
        public void GetCellAtArrayTwoTest()
        {
            const int SmallBoardSize = 4;
            HexBoard source = new HexBoard(SmallBoardSize);

            Location[] locs = new Location[2];
            locs[0] = new Location(1, 1);
            locs[1] = new Location(2, 2);

            Cell[] result = source.GetCellAt(locs);

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0].Location.X);
            Assert.AreEqual(1, result[0].Location.Y);
            Assert.AreEqual(2, result[1].Location.X);
            Assert.AreEqual(2, result[1].Location.Y);
        }

        [Test]
        public void GetCellsTest()
        {
            HexBoard hexBoard = new HexBoard(BoardSize);

            Cell[,] cells = hexBoard.GetCells();
            Assert.AreEqual(cells[1, 1].IsOccupied, Occupied.Empty);
            Assert.AreEqual(cells[2, 2].IsOccupied, Occupied.Empty);
            Assert.AreEqual(cells[3, 3].IsOccupied, Occupied.Empty);

            hexBoard.PlayMove(1, 1, true);

            Assert.AreEqual(cells[1, 1].IsOccupied, Occupied.PlayerX);
            Assert.AreEqual(cells[2, 2].IsOccupied, Occupied.Empty);
            Assert.AreEqual(cells[3, 3].IsOccupied, Occupied.Empty);

            hexBoard.PlayMove(2, 2, false);

            Assert.AreEqual(cells[1, 1].IsOccupied, Occupied.PlayerX);
            Assert.AreEqual(cells[2, 2].IsOccupied, Occupied.PlayerY);
            Assert.AreEqual(cells[3, 3].IsOccupied, Occupied.Empty);
        }

        #region helpers

        private static void DoTestRow(ICollection<Cell> row, int size)
        {
            Assert.IsNotNull(row);
            Assert.AreEqual(row.Count, size);
            NoNullsInCellArray(row);
        }

        private static void NoNullsInCellArray(IEnumerable<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                Assert.IsNotNull(cell);
            }
        }

        #endregion
    }
}
