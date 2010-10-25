namespace Hex.Board
{
    /// <summary>
    /// AFS 22 August 2004
    /// a cell on a hex board
    /// each cell is essentialy an x,y location
    /// and a state (empty or played by player 1 or 2)
    /// location cannot change but state can
    /// </summary>
    public class Cell
    {
        #region data

        // x,y cooordinates
        private Location location;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the Cell class at an x,y position
        /// </summary>
        /// <param name="location">the new cell's location</param>
        public Cell(Location location)
        {
            this.location = location;
            this.IsOccupied = Occupied.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Cell class by copying a cell
        /// </summary>
        /// <param name="originalCell">the cell to copy</param>
        public Cell(Cell originalCell)
        {
            if (originalCell != null)
            {
                // copy the state
                this.location = originalCell.Location;
                this.IsOccupied = originalCell.IsOccupied;
            }
        }
        #endregion

        public Occupied IsOccupied { get; set; }

        public Location Location
        {
            get { return this.location; }
        }

        public int X
        {
            get { return this.location.X; }
        }

        public int Y
        {
            get { return this.location.Y; }
        }

        public bool IsEmpty()
        {
            return this.IsOccupied == Occupied.Empty;
        }

        public bool IsPlayer(bool playerX)
        {
            if (playerX)
            {
                return this.IsOccupied == Occupied.PlayerX;
            }

            return this.IsOccupied == Occupied.PlayerY;
        }

        public override string ToString()
        {
            return this.location + " " + OccupiedHelper.OccupiedToString(this.IsOccupied);
        }
    }
}