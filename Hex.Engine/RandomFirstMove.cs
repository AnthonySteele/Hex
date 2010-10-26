//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Engine
{
    using System;

    using Hex.Board;

    /// <summary>
    /// Make a first mvoe that is unpredicatable yet good
    /// pick form a list of options
    /// </summary>
    public class RandomFirstMove
    {
        private readonly int boardSize;
        private readonly Random randomNumbers;

        public RandomFirstMove(int boardSize)
        {
            this.boardSize = boardSize;
            this.randomNumbers = new Random();
        }

        public int BoardSize
        {
            get { return this.boardSize; }
        }
        
        public Location RandomMove()
        {
            // center, corner or near edge
            int choice = this.randomNumbers.Next(10);

            switch (choice)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    return this.RandomMiddle();

                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return this.RandomNearEdge();

                default:
                    throw new Exception("Choice out of range:" + choice);
            }
        }

        private Location RandomNearEdge()
        {
            int middle = this.BoardSize / 2;

            // a chance to offset from the middle
            if ((this.BoardSize > 6) && this.RandomBool())
            {
                if (this.RandomBool())
                {
                    middle++;
                }
                else
                {
                    middle--;
                }
            }

            if (this.RandomBool())
            {
                // top
                return new Location(middle, this.BoardSize - 2);
            }
            
            // bottom
            return new Location(middle, 1);
        }

        private Location RandomMiddle()
        {
            int midPoint = (this.BoardSize / 2) - 1;
            Location midLocation = new Location(midPoint, midPoint);
            if ((this.BoardSize > 6) && this.RandomBool())
            {
                return this.RandomNeighbour(midLocation);
            }

            return midLocation;
        }

        private Location RandomNeighbour(Location loc)
        {
            HexBoardNeighbours neighbourFinder = new HexBoardNeighbours(this.BoardSize);
            Location[] neighbours = neighbourFinder.Neighbours(loc);

            return this.RandomElement(neighbours);
        }

        private Location RandomElement(Location[] locations)
        {
            int max = locations.Length;
            int selection = this.randomNumbers.Next(max);
            return locations[selection];
        }

        private bool RandomBool()
        {
            // equal chance of 1 or 0
            return this.randomNumbers.Next(2) == 0;
        }
    }
}
