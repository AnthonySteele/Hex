//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Engine.PathLength
{
    using System;
    using System.Collections.Generic;
    using Hex.Board;

    /// <summary>
    /// hex engine path length calculator
    /// </summary>
    /// <remarks>
    /// calculates how many cells does the player have to play in order to win 
    /// knowing the path length remaining tells you who's winning
    /// if it gets to zero, the player has connected the sides
    /// if it is infinite, the other player has connected the sides
    /// </remarks>
    public class PathLengthLoop : PathLengthBase
    {
        public PathLengthLoop(HexBoard board)
        {
            this.board = board;
        }

        public override int PlayerScore(bool isPlayerX)
        {
            this.CalculateVals(isPlayerX);

            Cell[] finalRow = board.Row(isPlayerX, false);
            int minScore = this.MinVal(finalRow);
            return minScore;
        }

        /// <summary>
        /// calculates the length of the remainaing path across the board
        /// </summary>
        /// <param name="isPlayerX">true if it's player x, false if it's player y</param>
        /// <remarks>
        /// When it gets to zero, you've won
        /// Done by initialising the value for each cell to a high number (OFF_PATH)
        /// and then to the min of all neighbours, plus 1 if it's empty,
        /// plus zero if this player has filled it already
        /// </remarks>
        public void CalculateVals(bool isPlayerX)
        {
            this.InitialiseCalculation(isPlayerX);

            // iterate until stable
            while (this.PathStep())
            {
            }
        }

        public override List<Location> GetCleanPath(bool isPlayerX)
        {
            this.ResetOnPath();
            this.DetectOnPath(isPlayerX);

            List<Location> result = new List<Location>();

            foreach (Cell cell in board.GetCells())
            {
                if (this.onPathData[cell.X, cell.Y])
                {
                    result.Add(cell.Location);
                }
            }

            return result;
        }

        private void InitialiseCalculation(bool isPlayerX)
        {
            playerX = isPlayerX;
            vals = new int[board.Size, board.Size];

            // init to off_path
            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    vals[x, y] = PathLengthConstants.OffPath;
                }
            }

            // set up the starting row
            Cell[] startRow = board.Row(isPlayerX, true);

            foreach (Cell startCell in startRow)
            {
                int initialValue = PathLengthConstants.OffPath;

                // who owns this cell
                if (startCell.IsEmpty())
                {
                    initialValue = 1;
                }
                else if (startCell.IsPlayer(playerX))
                {
                    initialValue = 0;
                }

                this.SetValByLoc(startCell.Location, initialValue);
            }
        }
        
        /// <summary>
        /// update all cells from neighbours.
        /// return true if any changes were made
        /// </summary>
        /// <returns>true if work was done</returns>
        private bool PathStep()
        {
            bool workDone = false;

            foreach (Cell cell in board.GetCells())
            {
                // ignore cells held by the enemy
                if (cell.IsPlayer(!playerX))
                {
                    continue;
                }

                bool cellWorkDone = this.UpdateCellFromNeighbour(cell);
                workDone = workDone || cellWorkDone;
            }

            return workDone;
        }
        
        /// <summary>
        /// number of steps needed to reach a cell is the min of the steps
        /// needed to reach a neighbour, plus one (unles it's filled)
        /// </summary>
        /// <param name="cell">the cell to update</param>
        /// <returns>true if work was done</returns>
        private bool UpdateCellFromNeighbour(Cell cell)
        {
            var neighbours = board.Neighbours(cell);
            int closest = this.MinVal(neighbours);

            if (cell.IsEmpty() && closest < PathLengthConstants.OffPath)
            {
                closest++;
            }

            if (closest < Value(cell.Location))
            {
                this.SetValByLoc(cell.Location, closest);
                return true;
            }

            return false;
        }
        
        private int MinVal(IEnumerable<Cell> cells)
        {
            int result = PathLengthConstants.OffPath;
            foreach (Cell cell in cells)
            {
                int val = Value(cell.Location);
                if (val < result)
                {
                    result = val;
                }
            }

            return result;
        }

        private void SetValByLoc(Location loc, int value)
        {
            if (value < 0)
            {
                throw new Exception("Valid value");
            }

            vals[loc.X, loc.Y] = value;
        }
        
        private void ResetOnPath()
        {
            if (this.onPathData == null)
            {
                return;
            }

            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    this.onPathData[x, y] = true;
                }
            }
        }

        /// <summary>
        /// After calculating the vals, some cells are on the shortest path,
        /// and some are not. The object of this step is to determine which ones are
        /// </summary>
        /// <param name="isPlayerX">true if the player is player x</param>
        private void DetectOnPath(bool isPlayerX)
        {
            this.onPathData = new bool[board.Size, board.Size];
            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    // cells held by the enemy are never on the path,
                    // the rest might be 
                    bool enemyCell = board.GetCellAt(x, y).IsPlayer(!isPlayerX);
                    this.onPathData[x, y] = !enemyCell;
                }
            }

            // look at the final row - the min value is the end of the path
            // other cells with higher values are not on the path
            Cell[] finalRow = board.Row(isPlayerX, false);

            int minFinal = this.MinVal(finalRow);

            foreach (Cell finalItem in finalRow)
            {
                if (vals[finalItem.X, finalItem.Y] != minFinal)
                {
                    this.onPathData[finalItem.X, finalItem.Y] = false;
                }
            }

            /*  now each cell not on the final row
                must have a neighbour on the path with a higher value
                or it's not on the path 
            while (PathStep(aPlayerX))
                ;
             */
        }

        private bool CleanPathStep(bool isPlayerX)
        {
            bool workDone = false;
            int maxX = board.Size;
            int maxY = board.Size;

            if (isPlayerX)
            {
                maxY--;
            }
            else
            {
                maxX--;
            }

            for (int liX = 0; liX < maxX; liX++)
            {
                for (int liY = 0; liY < maxY; liY++)
                {
                    if (this.onPathData[liX, liY])
                    {
                        Cell cell = board.GetCellAt(liX, liY);
                        var neighbours = board.Neighbours(cell);

                        int currentVal = vals[liX, liY];
                        bool cellSafe = false;

                        foreach (Cell neighb in neighbours)
                        {
                            if (this.onPathData[neighb.X, neighb.Y])
                            {
                                int neigbVal = vals[neighb.X, neighb.Y];

                                if (neighb.IsPlayer(isPlayerX))
                                {
                                    neigbVal++;
                                }

                                if (neigbVal == currentVal + 1)
                                {
                                    // cell is safe
                                    cellSafe = true;
                                    break;
                                }
                            }
                        }

                        if (!cellSafe)
                        {
                            this.onPathData[liX, liY] = false;
                            workDone = true;
                        }
                    }
                }
            }

            return workDone;
        }
    }
}