//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Board
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Class to calculate which cells neighbour other cells
    /// hexagonal geometry
    /// The board is composed of hexagons,
    /// and has the general shape of a parallelogram
    /// 0,0 is the east pole, and boardSize, boardSize is the west pole
    /// the x axis runs down and right, the y axis up and right
    /// drawing a diagram helps
    /// Each loc has 6 neighbours unless it is on an edge
    /// </summary>
    public class HexBoardNeighbours
    {
        private readonly int boardSize;

        public HexBoardNeighbours(int boardSize)
        {
            this.boardSize = boardSize;
        }

        public int BoardSize
        {
            get { return this.boardSize; }
        }

        public bool IsOnBoard(Location loc)
        {
            return (loc.X >= 0) && (loc.Y >= 0) &&
                (loc.X < this.BoardSize) && (loc.Y < this.BoardSize);
        }

        public int NeighbourCount(Location loc)
        {
            if (!this.IsOnBoard(loc))
            {
                return 0;
            }

            bool xLow = loc.X == 0;
            bool xHigh = loc.X == (this.BoardSize - 1);
            bool yLow = loc.Y == 0;
            bool yHigh = loc.Y == (this.BoardSize - 1);

            if (xLow || yHigh || xHigh || yLow)
            {
                // east and west pole cells have 2 neighbours
                if ((xLow && yLow) || (xHigh && yHigh))
                {
                    return 2;
                }

                // top and bottom poles have 3 neighbours
                if ((xLow && yHigh) || (xHigh && yLow))
                {
                    return 3;
                }

                // edge cells have 4 neighbours
                return 4;
            }

            // interior cells have 6 neighbours
            return 6;
        }

        public bool AreNeighbours(Location location1, Location location2)
        {
            if (!this.IsOnBoard(location1) || !this.IsOnBoard(location2))
            {
                return false;
            }

            int xDif = location1.X - location2.X;
            int yDif = location1.Y - location2.Y;

            // too far away
            if (Math.Abs(xDif) > 1)
            {
                return false;
            }

            if (Math.Abs(yDif) > 1)
            {
                return false;
            }

            // 9 cells have a diff of 1 or less (3 *3 square)
            // two of these are not hex-neighbours, and one is the same cell
            // the other three are neighbours 

            // same cell
            if ((xDif == 0) && (yDif == 0))
            {
                return false;
            }

            // not neighbours 
            if ((xDif == 1) && (yDif == 1))
            {
                return false;
            }

            if ((xDif == -1) && (yDif == -1))
            {
                return false;
            }

            return true;
        }

        public Location[] Neighbours(Location location)
        {
            List<Location> result = new List<Location>();

            if (this.IsOnBoard(location))
            {
                int x = location.X;
                int y = location.Y;

                result.Add(new Location(x - 1, y));
                result.Add(new Location(x, y - 1));
                result.Add(new Location(x + 1, y));
                result.Add(new Location(x, y + 1));

                // the above and below neighbours
                result.Add(new Location(x - 1, y + 1));
                result.Add(new Location(x + 1, y - 1));
            }

            // remove locations off the edge of the board
            for (int loopCounter = result.Count - 1; loopCounter >= 0; loopCounter--)
            {
                if (!this.IsOnBoard(result[loopCounter]))
                {
                    result.RemoveAt(loopCounter);
                }    
            }

            return result.ToArray();
        }

        public Location[][] Neighbours2(Location location)
        {
            List<Location[]> result = new List<Location[]>();
            int x = location.X;
            int y = location.Y;

            if (this.IsOnBoard(location))
            {
                AddTriple(result,
                    x + 1, y + 1,
                    x + 1, y,
                    x,     y + 1);

                AddTriple(result,
                    x + 2, y - 1,
                    x + 1, y,
                    x + 1, y - 1);


                AddTriple(result,
                    x + 1, y - 2,
                    x + 1, y - 1,
                    x,     y - 1);

                AddTriple(result,
                    x - 1, y - 1,
                    x,     y - 1,
                    x - 1, y);

                AddTriple(result,
                    x - 2, y + 1,
                    x - 1, y,
                    x - 1, y + 1);

                AddTriple(result,
                    x - 1, y + 2,
                    x - 1, y + 1,
                    x,     y + 1);
            }

            // remove locations off the edge of the board
            for (int loopCounter = result.Count - 1; loopCounter >= 0; loopCounter--)
            {
                if (!this.IsOnBoard(result[loopCounter][0]) ||
                    !this.IsOnBoard(result[loopCounter][1]) ||
                    !this.IsOnBoard(result[loopCounter][2]))
                {
                    result.RemoveAt(loopCounter);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// The edge that you are trying to connect to 
        /// can be used like a filled neighbour2
        /// </summary>
        /// <param name="loc"> The location to test</param>
        /// <param name="playerX">true if this is player X</param>
        /// <returns>
        /// the cells between the location and the edge
        /// </returns>
        public Location[] BetweenEdge(Location loc, bool playerX)
        {
            if (playerX)
            {
                if ((loc.Y == 1) && (loc.X < (this.BoardSize - 1)))
                {
                    Location[] result = new Location[2];
                    result[0] = new Location(loc.X, 0);
                    result[1] = new Location(loc.X + 1, 0);
                    return result;
                }
                
                if ((loc.Y == this.BoardSize - 2) && (loc.X > 0))
                {
                    Location[] result = new Location[2];
                    result[0] = new Location(loc.X - 1, this.BoardSize - 1);
                    result[1] = new Location(loc.X, this.BoardSize - 1);
                    return result;
                }

                return new Location[0];
            }

            if ((loc.X == 1) && (loc.Y < (this.BoardSize - 1)))
            {
                Location[] result = new Location[2];
                result[0] = new Location(0, loc.Y);
                result[1] = new Location(0, loc.Y + 1);
                return result;
            }

            if ((loc.X == this.BoardSize - 2) && (loc.Y > 0))
            {
                Location[] result = new Location[2];
                result[0] = new Location(this.BoardSize - 1, loc.Y - 1);
                result[1] = new Location(this.BoardSize - 1, loc.Y);
                return result;
            }

            return new Location[0];
        }

        /// <summary>
        /// Worker proc for Neighbours2
        /// Add three cells to the list
        /// </summary>
        /// <param name="result">the list to populate</param>
        /// <param name="x1">the first x co-ordinate</param>
        /// <param name="y1">the first y co-ordinate</param>
        /// <param name="x2">the second x co-ordinate</param>
        /// <param name="y2">the second y co-ordinate</param>
        /// <param name="x3">the third x co-ordinate</param>
        /// <param name="y3">the third y co-ordinate</param>
        private static void AddTriple(List<Location[]> result, int x1, int y1, int x2, int y2, int x3, int y3)
        {
            Location[] triple = new Location[3];

            triple[0] = new Location(x1, y1);
            triple[1] = new Location(x2, y2);
            triple[2] = new Location(x3, y3);

            result.Add(triple);
        }
    }
}
