namespace Hex.Engine.Test
{
    using System.Collections.Generic;
    using Hex.Board;
    using NUnit.Framework;

    /// <summary>
    /// Test methods for class HexGame
    /// </summary>
    [TestFixture]
    public class HexGameTest
    {        
        [Test]
        public void TestPlay()
        {
            HexGame hexGame = new HexGame(7);
            const int X = 3;
            const int Y = 3;

            Assert.IsTrue(hexGame.Board.GetCellOccupiedAt(X, Y) == Occupied.Empty);

            hexGame.Play(X, Y);
            Assert.IsTrue(hexGame.Board.GetCellOccupiedAt(X, Y) == Occupied.PlayerX);

            hexGame.Clear();
            Assert.IsTrue(hexGame.Board.GetCellOccupiedAt(X, Y) == Occupied.Empty);
        }

        [Test]
        public void TestPathLength1()
        {
            HexGame hexGame = new HexGame(7);
            Assert.IsTrue(hexGame.HasWon() == Occupied.Empty);

            int xPathLength = hexGame.PlayerScore(true);
            int yPathLength = hexGame.PlayerScore(false);

            // clean the path, get cells on the path
            List<Location> xPath = hexGame.GetCleanPath(true);
            List<Location> yPath = hexGame.GetCleanPath(false);

            Assert.AreEqual(xPath.Count, yPath.Count);
            Assert.AreEqual(xPath.Count, hexGame.Board.Size * hexGame.Board.Size);

            Assert.AreEqual(xPathLength, yPathLength);

            for (int x = 0; x < hexGame.Board.Size; x++)
            {
                for (int y = 0; y < hexGame.Board.Size; y++)
                {
                    Location loc = new Location(x, y);

                    Assert.IsTrue(xPath.Contains(loc));
                    Assert.IsTrue(yPath.Contains(loc));
                }
            }
        }

        [Test]
        public void TestPathLength2()
        {
            HexGame hexGame = new HexGame(7);
            /* player1 has played at 3, 3
               thier shortest path has narrowed to ones passing through this */
            hexGame.Play(3, 3);

            /* These points are on player X's shortest path */
            Location[] xPath = new Location[19];

            xPath[0] = new Location(0, 6);
            xPath[1] = new Location(1, 5);
            xPath[2] = new Location(1, 6);
            xPath[3] = new Location(2, 4);
            xPath[4] = new Location(2, 5);
            xPath[5] = new Location(2, 6);
            xPath[6] = new Location(3, 0);
            xPath[7] = new Location(3, 1);
            xPath[8] = new Location(3, 2);
            xPath[9] = new Location(3, 3);
            xPath[10] = new Location(3, 4);
            xPath[11] = new Location(3, 5);
            xPath[12] = new Location(3, 6);
            xPath[13] = new Location(4, 0);
            xPath[14] = new Location(4, 1);
            xPath[15] = new Location(4, 2);
            xPath[16] = new Location(5, 0);
            xPath[17] = new Location(5, 1);
            xPath[18] = new Location(6, 0);

            Assert.IsTrue(hexGame.HasWon() == Occupied.Empty);

            int xPathLength = hexGame.PlayerScore(true);
            int yPathLength = hexGame.PlayerScore(false);

            // clean the path, get cells on the path
            List<Location> xPathActual = hexGame.GetCleanPath(true);

            Assert.IsTrue(xPathLength < yPathLength);

            for (int x = 0; x < hexGame.Board.Size; x++)
            {
                for (int y = 0; y < hexGame.Board.Size; y++)
                {
                    Cell cell = hexGame.Board.GetCellAt(x, y);

                    bool xOnPath = cell.Location.IsInList(xPath);
                    bool xOnPathActual = xPathActual.Contains(cell.Location);

                    Assert.AreEqual(xOnPath, xOnPathActual, "X Path at " + cell.Location);
                }
            }
        }

        [Test]
        public void TestPathLength3()
        {
            HexGame hexGame = new HexGame(7);
            /* player1 has played in a line
                 thier shortest path has narrowed to ones passing through this */
            hexGame.Board.PlayMove(3, 0, true);
            hexGame.Board.PlayMove(3, 2, true);
            hexGame.Board.PlayMove(3, 4, true);
            hexGame.Board.PlayMove(3, 6, true);

            /* These points are on player X's shortest path */
            Location[] xPath = new Location[7];

            xPath[0] = new Location(3, 0);
            xPath[1] = new Location(3, 1);
            xPath[2] = new Location(3, 2);
            xPath[3] = new Location(3, 3);
            xPath[4] = new Location(3, 4);
            xPath[5] = new Location(3, 5);
            xPath[6] = new Location(3, 6);

            // all of the board, except corners, is still on the y path
            Location[] yNoPath = new Location[6];
            yNoPath[0] = new Location(0, 0);
            yNoPath[1] = new Location(1, 0);
            yNoPath[2] = new Location(2, 0);
            yNoPath[3] = new Location(4, 6);
            yNoPath[4] = new Location(5, 6);
            yNoPath[5] = new Location(6, 6);

            Assert.IsTrue(hexGame.HasWon() == Occupied.Empty);

            int xPathLength = hexGame.PlayerScore(true);
            int yPathLength = hexGame.PlayerScore(false);

            // clean the path, get cells on the path
            List<Location> xPathActual = hexGame.GetCleanPath(true);
            List<Location> yPathActual = hexGame.GetCleanPath(false);

            Assert.IsTrue(xPathLength < yPathLength);

            for (int x = 0; x < hexGame.Board.Size; x++)
            {
                for (int y = 0; y < hexGame.Board.Size; y++)
                {
                    Cell cell = hexGame.Board.GetCellAt(x, y);

                    bool xOnPath = cell.Location.IsInList(xPath);
                    bool yOnPath = !cell.IsPlayer(true) && !cell.Location.IsInList(yNoPath);

                    Assert.AreEqual(xPathActual.Contains(cell.Location), xOnPath, "X " + cell);
                    Assert.AreEqual(yPathActual.Contains(cell.Location), yOnPath, "Y " + cell);
                }
            }
        }

        [Test]
        public void TestPathLength4()
        {
            HexGame hexGame = new HexGame(7);
            /* player1 has played in a line
                 thier shortest path has narrowed to ones passing through this
                 these pairs have two cells inbetween  */
            hexGame.Board.PlayMove(3, 0, true);
            hexGame.Board.PlayMove(2, 2, true);
            hexGame.Board.PlayMove(1, 4, true);
            hexGame.Board.PlayMove(0, 6, true);

            /* These points are on player X's shortest path */
            Location[] xPath = new Location[10];

            xPath[0] = new Location(0, 5);
            xPath[1] = new Location(0, 6);
            xPath[2] = new Location(1, 3);
            xPath[3] = new Location(1, 4);
            xPath[4] = new Location(1, 5);
            xPath[5] = new Location(2, 1);
            xPath[6] = new Location(2, 2);
            xPath[7] = new Location(2, 3);
            xPath[8] = new Location(3, 0);
            xPath[9] = new Location(3, 1);

            Assert.IsTrue(hexGame.HasWon() == Occupied.Empty);

            int xPathLength = hexGame.PlayerScore(true);
            int yPathLength = hexGame.PlayerScore(false);

            // clean the path, get cells on the path
            List<Location> xPathActual = hexGame.GetCleanPath(true);

            Assert.IsTrue(xPathLength < yPathLength);

            for (int x = 0; x < hexGame.Board.Size; x++)
            {
                for (int y = 0; y < hexGame.Board.Size; y++)
                {
                    Cell cell = hexGame.Board.GetCellAt(x, y);

                    bool xOnPath = cell.Location.IsInList(xPath);
                    bool xOnPathActual = xPathActual.Contains(cell.Location);

                    Assert.AreEqual(xOnPath, xOnPathActual, "X " + cell);
                }
            }
        }
    }
}
