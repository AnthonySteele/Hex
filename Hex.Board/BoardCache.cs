namespace Hex.Board
{
    using System.Collections.Generic;
    
    /// <summary>
    /// A cache of boards
    /// Based on the premise that making boards is expensive
    /// and we only use 5-10 of them at a time
    /// so keep some around for reuse
    /// </summary>
    public class BoardCache
    {
        #region private data

        private readonly List<HexBoard> available = new List<HexBoard>();
        private readonly List<HexBoard> inUse = new List<HexBoard>();
        private readonly int boardSize;

        #endregion

        public BoardCache(int boardSize)
        {
            this.boardSize = boardSize;
        }

        #region public data

        /// <summary>
        ///  Gets the size of the boards to be made
        /// </summary>
        public int BoardSize
        {
            get { return this.boardSize; }
        }

        /// <summary>
        /// Gets the count of all boards created
        /// </summary>
        public int BoardCount
        {
            get { return this.available.Count + this.inUse.Count; }
        }

        public int AvailableCount
        {
            get
            {
                return this.available.Count;
            }
        }

        #endregion

        #region operations
        public HexBoard GetBoard()
        {
            HexBoard result;

            if (this.available.Count == 0)
            {
                // no boards available, so make a new one
                result = new HexBoard(this.BoardSize);
            }
            else
            {
                // return any available board - less jiggering to take the end one
                int boardIndex = this.available.Count - 1;
                result = this.available[boardIndex];
                this.available.RemoveAt(boardIndex);
            }

            // board is now in use
            this.inUse.Add(result);

            return result;
        }

        /// <summary>
        /// board is no longer in use 
        /// So put in on the in-use list
        /// </summary>
        /// <param name="board">the board to release</param>
        public void Release(HexBoard board)
        {
            this.inUse.Remove(board);
            this.available.Add(board);
        }

        #endregion
    }
}
