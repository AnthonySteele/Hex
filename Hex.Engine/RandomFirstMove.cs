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
        #region data
        private readonly int boardSize;
        private readonly Random randomNumbers;
        #endregion

        #region properties

        public int BoardSize
        {
            get { return this.boardSize; }
        }
        #endregion

        #region constructor

        public RandomFirstMove(int boardSize)
        {
            this.boardSize = boardSize;
            this.randomNumbers = new Random();
        }
        #endregion

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
                    return RandomMiddle();
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return RandomNearEdge();
                default:
                    throw new Exception("Choice out of range:" + choice);

            }
        }

        private Location RandomNearEdge()
        {
            int middle = BoardSize / 2;

            // a chance to offset from the middle
            if ((BoardSize > 6) && RandomBool())
            {
                if (RandomBool())
                {
                    middle++;
                }
                else
                {
                    middle--;
                }
            }

            if (RandomBool())
            {
                // top
                return new Location(middle, BoardSize - 2);
            }
            
            // bottom
            return new Location(middle, 1);
        }

        private Location RandomMiddle()
        {
            int midPoint = (BoardSize / 2) - 1;
            Location midLocation = new Location(midPoint, midPoint);
            if ((BoardSize > 6) && RandomBool())
            {
                return RandomNeighbour(midLocation);
            }

            return midLocation;
        }

        private Location RandomNeighbour(Location loc)
        {
            HexBoardNeighbours neighbourFinder = new HexBoardNeighbours(BoardSize);
            Location[] neighbours = neighbourFinder.Neighbours(loc);

            return RandomElement(neighbours);
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
            return (this.randomNumbers.Next(2) == 0);
        }

    }
}
