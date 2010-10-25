namespace Hex.Engine.PathLength
{
    using System.Collections.Generic;
    using Hex.Board;

    /// <summary>
    /// Find the path length using an algorithym based on A*
    /// Trial, see if its faster than the other, board-sweeper
    /// </summary>
    public class PathLengthAStar : PathLengthBase
    {
        /*
         * This path length algorithym is based on the A* algorithym:
            
           All cells have a marker with these state: (not done, done)
         * All empty/occupied by player cells are marked "not done"
         * Cells occupied by the opponent are marked "done"
         * 
            All vacant/occupied by player cells in the first row 
            get a distance of 1 (if unoccupied), or zero (if occupied)
            And are put in the "in progress" list
             
            Sort the in progress list, lowest distance first

            Repeat
              - Take the first cell (lowest distance) off the in progress list
                - if this is in the end row, we're done. Return the value
                - if there are no cells in progress, we're done - no route

              - find all neighbours which are not done.
              - Update thier distance to min(current, distance from here) 
                 where "distance from here" is 1 if the cell is empty, 
                   0 if it's occupied by player.
              - Put these cells on the in progress list, in sorted order
              - mark this cell as "done" and remove it from the "in progress" list
          */

        private const int NeighbourBaseStep = 10;

        private bool[,] done;
        private List<Cell> inProgressCells;
        private List<int> inProgressCellDistances;

        private bool useNeighbours2 = true;
        private bool cleanPath;
        private CleanPathData cleanPathData;

        private int neighbourIncrement;
        private int neighbour2Increment;

        public PathLengthAStar(HexBoard board)
        {
            this.board = board;
        }
        
        public bool UseNeighbours2
        {
            get { return this.useNeighbours2; }
            set { this.useNeighbours2 = value; }
        }

        public bool CleanPath
        {
            get { return this.cleanPath; }
            set { this.cleanPath = value; }
        }

        /// <summary>
        /// calculates the length of the remainaing path across the board
        /// </summary>
        /// <param name="isPlayerX">true if the player is player x</param>
        /// <returns>the player score</returns>
        /// <remarks>
        /// When it gets to zero, you've won
        /// Done by initialising the value for each cell to a high number (OFF_PATH)
        /// and then to the min of all neighbours, plus 1 if it's empty,
        /// plus zero if this player has filled it already
        /// </remarks>
        public override int PlayerScore(bool isPlayerX)
        {
            this.InitialiseCalculation(isPlayerX);

            while (this.inProgressCells.Count > 0)
            {
                int resultPath = this.ProcessFirstInProgressCell();

                if (resultPath >= 0)
                {
                    return resultPath;
                }
            }

            // no path
            return PathLengthConstants.OffPath;
        }

        public override List<Location> GetCleanPath(bool isPlayerX)
        {
            bool saveCleanPath = this.cleanPath;
            List<Location> result;
            try
            {
                this.cleanPath = true;
                result = new List<Location>();

                this.PlayerScore(isPlayerX);

                if ((this.cleanPathData != null) && (this.cleanPathData.Count > 0))
                {
                    this.cleanPathData.Clean(isPlayerX, board.Size);
                    result.AddRange(this.cleanPathData.Keys);
                }
            }
            finally
            {
                this.cleanPath = saveCleanPath;
            }

            return result;
        }

        private int ProcessFirstInProgressCell()
        {
            Cell inProgressCell = this.inProgressCells[0];
            int distance = this.inProgressCellDistances[0];

            // is it in the last row?
            if (this.IsInLastRow(inProgressCell))
            {
                return distance;
            }

            // remove it
            this.inProgressCells.RemoveAt(0);
            this.inProgressCellDistances.RemoveAt(0);
            this.done[inProgressCell.X, inProgressCell.Y] = true;

            // get neighbours
            var neighbours = board.Neighbours(inProgressCell);
            foreach (Cell neighbour in neighbours)
            {
                // is the neighbour done?
                if (!this.done[neighbour.X, neighbour.Y])
                {
                    // calc a distance to the neighbour
                    int neighbourDistance = distance;
                    if (neighbour.IsEmpty())
                    {
                        if (this.useNeighbours2)
                        {
                            neighbourDistance += this.neighbourIncrement;
                        }
                        else
                        {
                            neighbourDistance++;
                        }
                    }

                    // add it
                    this.AddInProgressCell(neighbour, neighbourDistance);

                    if (this.cleanPath)
                    {
                        this.cleanPathData.SetScore(neighbour.Location, neighbourDistance, inProgressCell.Location);
                    }
                }
            }

            // shorter distance to a neighbour2 with between cells filled in
            if (this.useNeighbours2 && inProgressCell.IsPlayer(playerX))
            {
                // get neighbours 2
                var neighbours2 = board.Neighbours2(inProgressCell);
                foreach (Cell[] triple in neighbours2)
                {
                    Cell neighbour2 = triple[0];
                    if (!this.done[neighbour2.X, neighbour2.Y])
                    {
                        if (neighbour2.IsPlayer(playerX) && triple[1].IsEmpty() && triple[2].IsEmpty())
                        {
                            int neighbour2Distance = distance + this.neighbour2Increment;

                            // add it
                            this.AddInProgressCell(neighbour2, neighbour2Distance);

                            if (this.cleanPath)
                            {
                                this.cleanPathData.SetScore(
                                    neighbour2.Location, 
                                    neighbour2Distance,
                                    inProgressCell.Location, 
                                    triple[1].Location, 
                                    triple[2].Location);
                            }
                        }
                    }
                }

                // get cells that connect to an edge, something like neighbour 2
                // except the "neighbour" is the friendly board edge
                var edgeConnectors = board.BetweenEdge(inProgressCell.Location, playerX);
                if (edgeConnectors.Length == 2)
                {
                    if (edgeConnectors[0].IsEmpty() && edgeConnectors[1].IsEmpty())
                    {
                        int neighbour2Distance = distance + this.neighbour2Increment;

                        this.AddInProgressCell(edgeConnectors[0], neighbour2Distance);
                        this.AddInProgressCell(edgeConnectors[1], neighbour2Distance);

                        if (this.cleanPath)
                        {
                            this.cleanPathData.SetScore(
                                edgeConnectors[0].Location, 
                                neighbour2Distance,
                                inProgressCell.Location);
                            this.cleanPathData.SetScore(
                                edgeConnectors[1].Location, 
                                neighbour2Distance,
                                inProgressCell.Location);
                        }
                    }
                }
            }

            return -1;
        }

        private bool IsInLastRow(Cell cell)
        {
            if (playerX)
            {
                return cell.Y == (board.Size - 1);
            }

            return cell.X == (this.board.Size - 1);
        }
        
        private void InitialiseCalculation(bool isPlayerX)
        {
            if (this.useNeighbours2)
            {
                this.neighbourIncrement = NeighbourBaseStep;
                this.neighbour2Increment = this.neighbourIncrement - 1;
            }
            else
            {
                this.neighbourIncrement = 1;
                this.neighbour2Increment = 9999;
            }

            if (this.cleanPath)
            {
                this.cleanPathData = new CleanPathData();
            }

            int estimatedInProgressSize = board.Size * 2;

            this.inProgressCells = new List<Cell>(estimatedInProgressSize);
            this.inProgressCellDistances = new List<int>(estimatedInProgressSize);

            playerX = isPlayerX;

            // init to false
            this.done = new bool[board.Size, board.Size];
            vals = new int[board.Size, board.Size];

            // set to done=true where the opponent holds it
            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    vals[x, y] = PathLengthConstants.OffPath;

                    if (board.GetCellAt(x, y).IsPlayer(!playerX))
                    {
                        this.done[x, y] = true;
                    }
                }
            }

            // set up the starting row
            Cell[] startRow = board.Row(isPlayerX, true);

            foreach (Cell startCell in startRow)
            {
                // who owns this cell?
                if (startCell.IsEmpty())
                {
                    // one move to get to an empty cell
                    this.AddInProgressCell(startCell, this.neighbourIncrement);

                    if (this.cleanPath)
                    {
                        this.cleanPathData.SetScore(startCell.Location, this.neighbourIncrement);
                    }
                }
                else if (startCell.IsPlayer(playerX))
                {
                    // no moves to my filled cell
                    this.AddInProgressCell(startCell, 0);

                    if (this.cleanPath)
                    {
                        this.cleanPathData.SetScore(startCell.Location, this.neighbourIncrement);
                    }
                }
            }
        }

        private void AddInProgressCell(Cell cell, int value)
        {
            // is this cell in progress already?
            int existingIndex = this.inProgressCells.IndexOf(cell);
            if (existingIndex > -1)
            {
                if (value >= this.inProgressCellDistances[existingIndex])
                {
                    // we've found another route to a cell already in progress
                    // but it's not shorter, so it's of no interest
                    return;
                }

                // a new and shorter route to an in-progress cell
                // remove it from it from its current position
                // and add it at the new position as below
                this.inProgressCells.RemoveAt(existingIndex);
                this.inProgressCellDistances.RemoveAt(existingIndex);
            }

            // add the cell, in the right sorted position
            bool added = false;
            int loopMax = this.inProgressCells.Count; // does not vary during loop

            for (int loopIndex = 0; loopIndex < loopMax; loopIndex++)
            {
                if (value < this.inProgressCellDistances[loopIndex])
                {
                    this.inProgressCells.Insert(loopIndex, cell);
                    this.inProgressCellDistances.Insert(loopIndex, value);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                // put it on the end
                this.inProgressCells.Add(cell);
                this.inProgressCellDistances.Add(value);
            }

            vals[cell.X, cell.Y] = value;
        }
    }
}
