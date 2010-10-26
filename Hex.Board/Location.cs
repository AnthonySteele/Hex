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
    using System.Linq;

    /// <summary>
    /// AFS 22 August 2004
    /// immutable, lightweight struct to store an x,y location on the board
    /// </summary>
    public struct Location
    {
        private static readonly Location NullLocation = new Location(-1, -1);

        private readonly int x;
        private readonly int y;

        /// <summary>
        /// Initializes a new instance of the Location struct 
        /// </summary>
        /// <param name="x">X co-ordinate</param>
        /// <param name="y">Y co-ordinate</param>
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Location Null
        {
            get { return NullLocation; }
        }
        
        public int X
        {
            get
            {
                return this.x;
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }
        }

        public static bool operator ==(Location location1, Location location2)
        {
            return location1.Equals(location2);
        }

        public static bool operator !=(Location location1, Location location2)
        {
            return !location1.Equals(location2);
        }

        public bool Equals(Location otherLocation)
        {
            return (this.X == otherLocation.X) && (this.Y == otherLocation.Y);
        }

        public bool Equals(int otherX, int otherY)
        {
            return (this.X == otherX) && (this.Y == otherY);
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject == null)
            {
                return false;
            }

            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            return this.Equals((Location)otherObject);
        }

        /// <summary>
        /// return an int that depends on x and y, 
        /// and is unique for unique x and y
        /// assuming x and y will be small positive integers
        /// </summary>
        /// <returns>the object's hash code</returns>
        public override int GetHashCode()
        {
            const int HashXFactor = 1024;
            return (this.X * HashXFactor) + this.Y;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.X, this.Y);
        }

        public bool IsNull()
        {
            return (this.X == -1) && (this.Y == -1);
        }

        public bool IsInList(IEnumerable<Location> locations)
        {
            Location testLocation = this;
            return locations.Any(testLocation.Equals);
        }

        public int ListIndex(Location[] locations)
        {
            for (int loopIndex = 0; loopIndex < locations.Length; loopIndex++)
            {
                if (this.Equals(locations[loopIndex]))
                {
                    return loopIndex;
                }
            }

            return -1;
        }

        /// <summary>
        /// number of grid cells up/down plus across
        /// </summary>
        /// <param name="otherLocation">the location to compare</param>
        /// <returns>the straight-line distance</returns>
        public int ManhattanDistance(Location otherLocation)
        {
            return Math.Abs(this.X - otherLocation.X) + Math.Abs(this.Y - otherLocation.Y);
        }
    }
}

