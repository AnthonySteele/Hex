namespace Hex.Board
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// AFS 22 August 2004
    /// hex board
    /// Which is a square array of cells
    /// with hex geometry to determine which cells neighbour on which
    /// </summary>
    public class HexBoard
    {
        private Cell[,] cells;
        private int size;
        private int movesPlayedCount;

        private Cell[,][] neighbours;
        private Cell[,][][] neighbours2;
        private Cell[,][] playerXBetweenEdge;
        private Cell[,][] playerYBetweenEdge;

        /// <summary>
        /// Initializes a new instance of the HexBoard class 
        /// </summary>
        /// <param name="size">the size of the board</param>
        public HexBoard(int size)
        {
            this.InitCells(size);
        }

        /// <summary>
        /// Initializes a new instance of the HexBoard class from an existing HexBoard
        /// </summary>
        /// <param name="originalBoard">the board to copy</param>
        public HexBoard(HexBoard originalBoard)
        {
            if (originalBoard != null)
            {
                this.InitCells(originalBoard.Size);
                this.CopyStateFrom(originalBoard);
            }
        }

        public int Size
        {
            get { return this.size; }
        }

        public int MovesPlayedCount
        {
            get { return this.movesPlayedCount; }
        }

        public Cell[,] GetCells()
        {
            return this.cells;
        }

        #region workers to inspect state

        /// <summary>
        /// get the cell at the specfied location
        /// </summary>
        /// <param name="x">the x-cordinate</param>
        /// <param name="y">the y-cordinate</param>
        /// <returns>the cell at the location</returns>
        public Cell GetCellAt(int x, int y)
        {
            return this.cells[x, y];
        }

        public Cell GetCellAt(Location loc)
        {
            return this.cells[loc.X, loc.Y];
        }

        /// <summary>
        /// get an array of cells for an array of locations
        /// </summary>
        /// <param name="locations">the locations to process</param>
        /// <returns>the cells at the locations</returns>
        public Cell[] GetCellAt(Location[] locations)
        {
            if (locations == null)
            {
                return new Cell[0];
            }

            Cell[] result = new Cell[locations.Length];

            for (int loopIndex = 0; loopIndex < locations.Length; loopIndex++)
            {
                result[loopIndex] = this.GetCellAt(locations[loopIndex]);
            }

            return result;
        }

        public Cell[][] GetCellsAt(Location[][] locs)
        {
            Cell[][] result = new Cell[locs.GetLength(0)][];

            for (int loopIndex = 0; loopIndex < locs.GetLength(0); loopIndex++)
            {
                result[loopIndex] = this.GetCellAt(locs[loopIndex]);
            }

            return result;
        }

        // get the cells at the specified locations
        public Occupied GetCellOccupiedAt(Location loc)
        {
            return this.GetCellAt(loc).IsOccupied;
        }

        public Occupied GetCellOccupiedAt(int x, int y)
        {
            return this.GetCellAt(x, y).IsOccupied;
        }

        /// <summary>
        /// get the start (or end) row for player 1 (or 2)
        /// </summary>
        /// <param name="playerX">is the player queried player X</param>
        /// <param name="start">start row or end row</param>
        /// <returns>the cells in the row</returns>
        public Cell[] Row(bool playerX, bool start)
        {
            Cell[] result = new Cell[this.Size];

            // first or last row/col
            int col = start ? 0 : this.Size - 1;

            for (int loopIndex = 0; loopIndex < this.Size; loopIndex++)
            {
                if (playerX)
                {
                    result[loopIndex] = this.GetCellAt(loopIndex, col);
                }
                else
                {
                    result[loopIndex] = this.GetCellAt(col, loopIndex);
                }
            }

            return result;
        }

        /// <summary>
        /// get all empty cells
        /// </summary>
        /// <returns>all empty cells</returns>
        public IEnumerable<Location> EmptyCells()
        {
            foreach (Cell cell in this.cells)
            {
                if (cell.IsEmpty())
                {
                    yield return cell.Location;
                }
            }
        }

        /// <summary>
        /// are two boards the same, ie same state in all cells
        /// </summary>
        /// <param name="otherBoard">the other board</param>
        /// <returns>the equality boolean</returns>
        public bool Equals(HexBoard otherBoard)
        {
            if (otherBoard == null)
            {
                return false;
            }

            if (otherBoard.Size != this.Size)
            {
                return false;
            }

            foreach (Cell cell in this.cells)
            {
                if (cell.IsOccupied != otherBoard.GetCellAt(cell.Location).IsOccupied)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region set the state
        public void Clear()
        {
            /*  for once we can use the foreach statement,
             * since we don't need to know the X and Y locaion of the cells
             * or do it in any particular order
             */
            foreach (Cell cell in this.cells)
            {
                cell.IsOccupied = Occupied.Empty;
            }
        }

        /// <summary>
        /// set self equal to the other board 
        /// </summary>
        /// <param name="otherBoard">the board to copy</param>
        public void CopyStateFrom(HexBoard otherBoard)
        {
            if (otherBoard != null)
            {
                if (otherBoard.Size != this.Size)
                {
                    throw new Exception("Board sizes do not match");
                }

                foreach (Cell cell in this.cells)
                {
                    cell.IsOccupied = otherBoard.GetCellAt(cell.Location).IsOccupied;
                }

                this.movesPlayedCount = otherBoard.MovesPlayedCount;
            }
        }

        #endregion

        #region neighbours

        /// <summary>
        /// need to know which cells neighbour on which.
        /// There can be up to 6 neighbours in hex geometry
        /// For speed, generate this data once, 
        /// and keep it on the board
        /// For each cell on the board (2-d array of cells)
        /// there is an array of neighbours
        /// Array is square in 2 dimensions and jagged in the third
        /// </summary>
        /// <param name="cell">the cell to get neighbours for</param>
        /// <returns>the list oif neighbours</returns>
        public Cell[] Neighbours(Cell cell)
        {
            return this.Neighbours(cell.Location);
        }

        public Cell[] Neighbours(Location loc)
        {
            return this.neighbours[loc.X, loc.Y];
        }

        public Cell[][] Neighbours2(Cell cell)
        {
            return this.Neighbours2(cell.Location);
        }

        public Cell[][] Neighbours2(Location loc)
        {
            return this.neighbours2[loc.X, loc.Y];
        }

        public Cell[][] EmptyNeighbours2(Cell cell)
        {
            return this.EmptyNeighbours2(cell.Location);
        }

        public Cell[] BetweenEdge(Location loc, bool playerX)
        {
            if (playerX)
            {
                return this.playerXBetweenEdge[loc.X, loc.Y];
            }

            return this.playerYBetweenEdge[loc.X, loc.Y];
        }

        /// <summary>
        /// Get neighbour2 cells where the between cells are empty
        /// </summary>
        /// <param name="location">the location to get neighbours for</param>
        /// <returns>the neigbour2 cells</returns>
        public Cell[][] EmptyNeighbours2(Location location)
        {
            var result = new List<Cell[]>();
            var triples = this.Neighbours2(location);

            foreach (var triple in triples)
            {
                if (triple[1].IsEmpty() && triple[2].IsEmpty())
                {
                    result.Add(triple);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region play a move

        public void PlayMove(int x, int y, bool playerX)
        {
            this.PlayMove(new Location(x, y), playerX);
        }

        /// <summary>
        /// a player puts thier mark in an empty cell 
        /// </summary>
        /// <param name="location">the locaion played</param>
        /// <param name="playerX">whihc player</param>
        public void PlayMove(Location location, bool playerX)
        {
            Cell cell = this.GetCellAt(location);
            if (!cell.IsEmpty())
            {
                throw new Exception("Cell played is not empty at " + cell);
            }

            cell.IsOccupied = playerX ? Occupied.PlayerX : Occupied.PlayerY;

            this.movesPlayedCount++;
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Board {0} \n", this.Size);

            for (int y = this.Size - 1; y >= 0; y--)
            {
                for (int x = 0; x < this.Size; x++)
                {
                    Cell cell = this.GetCellAt(x, y);
                    result.Append(OccupiedHelper.OccupiedToString(cell.IsOccupied));
                    if (x < (this.Size - 1))
                    {
                        result.Append("  ");
                    }
                }

                result.Append("\n");
            }

            return result.ToString();
        }

        #endregion

        /// <summary>
        /// create a new board of specified size
        /// </summary>
        /// <param name="newSize">the size of the board</param>
        private void InitCells(int newSize)
        {
            this.size = newSize;
            this.movesPlayedCount = 0;
            this.cells = new Cell[this.Size, this.Size];

            for (int x = 0; x < this.size; x++)
            {
                for (int y = 0; y < this.size; y++)
                {
                    // a new location at x, y
                    Location loc = new Location(x, y);
                    this.cells[x, y] = new Cell(loc);
                }
            }

            this.InitNeighbours();
        }

        private void InitNeighbours()
        {
            HexBoardNeighbours neighboursCalc = new HexBoardNeighbours(this.Size);

            // init the cached neighbours arrays
            this.neighbours = new Cell[this.Size, this.Size][];
            this.neighbours2 = new Cell[this.Size, this.Size][][];
            this.playerXBetweenEdge = new Cell[this.Size, this.Size][];
            this.playerYBetweenEdge = new Cell[this.Size, this.Size][];

            foreach (Cell cell in this.cells)
            {
                Location[] neigbourLocations = neighboursCalc.Neighbours(cell.Location);
                this.neighbours[cell.X, cell.Y] = this.GetCellAt(neigbourLocations);

                Location[][] neighbour2Locations = neighboursCalc.Neighbours2(cell.Location);
                this.neighbours2[cell.X, cell.Y] = this.GetCellsAt(neighbour2Locations);

                Location[] localPlayerXBetweenEdge = neighboursCalc.BetweenEdge(cell.Location, true);
                this.playerXBetweenEdge[cell.X, cell.Y] = this.GetCellAt(localPlayerXBetweenEdge);

                Location[] localPlayerYBetweenEdge = neighboursCalc.BetweenEdge(cell.Location, false);
                this.playerYBetweenEdge[cell.X, cell.Y] = this.GetCellAt(localPlayerYBetweenEdge);
            }
        }
    }
}
