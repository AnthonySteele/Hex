namespace Hex.Engine.Lookahead
{
    using System;
    using System.Text;
    using Hex.Board;

    /// <summary>
    /// The killer move heuristic is an extension to the A-B pruning
    /// essentialy the idea is that if when inspecting the look-ahead tree
    /// of move 1,1, you realise that the opponent
    /// must reply with 5,5 to save his skin (i.e. 5,5 causes an A-B cutoff)
    /// Then when inspecting the look-ahead tree of move 1,2
    /// (the next move considered, and equally arbitrary)
    /// Then inspecting 5,5 first may well produce an A-B cut-off again
    /// Since all else is being equal
    /// so, for each level of look-ahead, keep an ordered array
    /// of locations to play first, since they are the most profitable.
    /// list is of fixed size - discard if they fall of the end of the list
    /// Add to this list when an AB-cutoff happens
    /// Put them at the front of any list of candidate moves
    /// - depth can increase if need be, but not decrease
    /// no point in discarding empty arrays
    /// since they may be reused later
    /// </summary>
    public class GoodMoves
    {
        #region constants
        public const int GoodMovesCount = 10;
        public const int DefaultDepth = 5;

        #endregion

        #region data
        private int depth;
        private int[] count;
        private Location[][] moves;

        #endregion

        // constructor
        public GoodMoves()
        {
            this.SetDepth(DefaultDepth);
        }

        #region public properties

        public int Depth
        {
            get { return this.depth; }
            set { this.SetDepth(value); }
        }

        #endregion

        #region public methods

        public int GetCount(int countDepth)
        {
            if (countDepth >= this.Depth)
            {
                this.SetDepth(countDepth + 1);
            }

            return this.count[countDepth];
        }

        public void Clear()
        {
            for (int loopDepth = 0; loopDepth < this.Depth; loopDepth++)
            {
                this.count[loopDepth] = 0;
                this.moves[loopDepth] = new Location[GoodMovesCount];
            }
        }

        /// <summary>
        /// return the good moves at this depth
        /// </summary>
        /// <param name="moveDepth">the depth to get moves for</param>
        /// <returns>the good moves</returns>
        public Location[] GetGoodMoves(int moveDepth)
        {
            if (moveDepth >= this.Depth)
            {
                string message = string.Format("Depth {0} is too big for {1}", moveDepth, this.Depth);
                throw new ArgumentException(message);
            }

            int countAtDepth = this.count[moveDepth];

            // Debug.Assert(dCount <= GOOD_MOVES_COUNT);
            // get the ones that have been set
            Location[] result = new Location[countAtDepth];
            if (countAtDepth > 0)
            {
                Array.Copy(this.moves[moveDepth], result, countAtDepth);
            }

            return result;
        }

        /// <summary>
        /// Store a good move at the front of the list for the given depth
        /// Move the rest up
        /// If the good move was in the list but not that the front,
        /// bring it to the front
        /// If there are enough good moves already, one falls off the end
        /// else the count goes up
        /// </summary>
        /// <param name="moveDepth">the depth at which to add</param>
        /// <param name="insertLoc">the locaiton to add</param>
        public void AddGoodMove(int moveDepth, Location insertLoc)
        {
            if (insertLoc.IsNull())
            {
                return;
            }

            if (moveDepth >= this.Depth)
            {
                this.SetDepth(moveDepth + 1);
            }

            Location[] myDepthLocs = this.moves[moveDepth];

            Location currentLoc = insertLoc;

            bool foundInList = false;

            int max = this.count[moveDepth];
            if (max >= GoodMovesCount)
            {
                max = GoodMovesCount - 1;
            }

            for (int index = 0; index <= max; index++)
            {
                Location tempLoc = myDepthLocs[index];

                // insert and move the next move up one
                myDepthLocs[index] = currentLoc;
                currentLoc = tempLoc;

                // if the original inserted loc was in the list, stop moving up
                if ((index < max) && insertLoc.Equals(currentLoc))
                {
                    foundInList = true;
                    break;
                }
            }

            // has the stored move count increased?
            if ((!foundInList) && (this.count[moveDepth] < GoodMovesCount))
            {
                this.count[moveDepth]++;
            }
        }

        // when a move has been played, the future moves a step closer
        public void MoveUpRows()
        {
            for (int i = 0; i < (this.Depth - 1); i++)
            {
                this.moves[i] = this.moves[i + 1];
                this.count[i] = this.count[i + 1];
            }

            this.moves[this.Depth - 1] = new Location[GoodMovesCount];
            this.count[this.Depth - 1] = 0;
        }

        // good moves for a blank board
        public void DefaultGoodMoves(int boardSize, int maxDepth)
        {
            this.SetDepth(maxDepth);

            for (int loopDepth = 0; loopDepth < maxDepth; loopDepth++)
            {
                this.DefaultGoodMovesAtDepth(boardSize, loopDepth);
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Good moves with count {0} and depth {1}\n", GoodMovesCount, this.Depth);

            for (int loopDepth = 0; loopDepth < this.Depth; loopDepth++)
            {
                Location[] items = this.GetGoodMoves(loopDepth);

                if (this.GetCount(loopDepth) == 0)
                {
                    result.AppendFormat("No moves at depth {0}", loopDepth);
                }
                else
                {
                    foreach (Location item in items)
                    {
                        result.Append(item);
                        result.Append(" ");
                    }
                }

                result.Append("\n");
            }

            return result.ToString();
        }

        private void DefaultGoodMovesAtDepth(int boardSize, int queryDepth)
        {
            int center = boardSize / 2;

            // corners
            this.AddGoodMove(queryDepth, new Location(0, 0));
            this.AddGoodMove(queryDepth, new Location(0, boardSize - 1));

            // middle of the sizes
            this.AddGoodMove(queryDepth, new Location(0, center));
            this.AddGoodMove(queryDepth, new Location(center, 0));

            // one in from the middle of the sides
            this.AddGoodMove(queryDepth, new Location(1, center));
            this.AddGoodMove(queryDepth, new Location(center, 1));

            // one off the center
            this.AddGoodMove(queryDepth, new Location(center + 1, center));
            this.AddGoodMove(queryDepth, new Location(center, center + 1));

            // finally, best of all, the center
            this.AddGoodMove(queryDepth, new Location(center, center));
        }

        #endregion

        private void SetDepth(int newDepth)
        {
            if (newDepth > this.depth)
            {
                int currentDepth = this.depth;

                // lengthen the count array
                int[] oldCount = this.count;
                this.count = new int[newDepth];
                if (oldCount != null)
                {
                    Array.Copy(oldCount, 0, this.count, 0, oldCount.Length);
                }

                // lengthen the location array
                Location[][] oldMoves = this.moves;
                this.moves = new Location[newDepth][];
                if (oldMoves != null)
                {
                    Array.Copy(oldMoves, 0, this.moves, 0, oldMoves.Length);
                }

                for (int i = currentDepth; i < newDepth; i++)
                {
                    this.moves[i] = new Location[GoodMovesCount];
                }

                this.depth = newDepth;
            }
        }
    }
}
