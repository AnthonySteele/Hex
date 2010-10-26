//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Engine.CandiateMoves
{
    using System.Collections.Generic;

    using Hex.Board;
    using Hex.Engine.Lookahead;

    public class CandidateMovesSelective : ICandidateMoves
    {
        private const int EdgeInterval = 2;
        private const int InternalInterval = 3;

        private readonly GoodMoves goodMoves;
        private readonly int cellsPlayedCount;

        public CandidateMovesSelective(GoodMoves goodMoves, int cellsPlayedCount)
        {
            this.goodMoves = goodMoves;
            this.cellsPlayedCount = cellsPlayedCount;
        }

        /// <summary>
        /// // get the candidate moves
        /// </summary>
        /// <param name="board">the board to get moves from</param>
        /// <param name="lookaheadDepth">the lookahead depth</param>
        /// <returns>the candiate move locations</returns>
        public IEnumerable<Location> CandidateMoves(HexBoard board, int lookaheadDepth)
        {
            int maxListLength = (board.Size * board.Size) - this.cellsPlayedCount;

            // enough space for all the possible moves
            List<Location> result = new List<Location>(maxListLength);

            // mask out the ones that have been used - intialised to false
            bool[,] maskCellSelected = new bool[board.Size, board.Size];

            if (lookaheadDepth < this.goodMoves.Depth)
            {
                // good moves are found first
                Location[] myGoodMoves = this.goodMoves.GetGoodMoves(lookaheadDepth);

                // copy in the good moves
                foreach (Location goodMoveLoc in myGoodMoves)
                {
                    if (board.GetCellAt(goodMoveLoc).IsEmpty() && (!maskCellSelected[goodMoveLoc.X, goodMoveLoc.Y]))
                    {
                        result.Add(goodMoveLoc);
                        maskCellSelected[goodMoveLoc.X, goodMoveLoc.Y] = true;
                    }
                }
            }

            // copy in all moves where the cell is empty, 
            // and not already in by virtue of being a good move
            foreach (Cell testCell in board.GetCells())
            {
                if (testCell.IsEmpty() && (!maskCellSelected[testCell.X, testCell.Y]))
                {
                    if (IsIncluded(testCell, board) || HasFilledNeighbour(testCell, board))
                    {
                        result.Add(testCell.Location);
                    }
                }
            }

            return result.ToArray();
        }

        private static bool HasFilledNeighbour(Cell testCell, HexBoard board)
        {
            Cell[] neighbours = board.Neighbours(testCell);

            foreach (Cell neighbour in neighbours)
            {
                if (!neighbour.IsEmpty())
                {
                    return true;
                }
            }

            Cell[][] neighbours2 = board.Neighbours2(testCell);

            foreach (Cell[] triple in neighbours2)
            {
                if (!triple[0].IsEmpty())
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsIncluded(Cell testCell, HexBoard board)
        {
            int boardSize = board.Size;
            int x = testCell.X;
            int y = testCell.Y;

            // corner cells are always in
            if (((x == 0) || (x == (boardSize - 1))) &&
                ((y == 0) || (y == boardSize - 1)))
            {
                return true;
            }

            // cells along the edges
            if ((x == 0) || (x == (boardSize - 1)))
            {
                if ((y % EdgeInterval) == 0)
                {
                    return true;
                }
            }

            if ((y == 0) || (y == (boardSize - 1)))
            {
                if ((x % EdgeInterval) == 0)
                {
                    return true;
                }
            }

            // interior cells
            if ((x != 0) && (x != (boardSize - 1)) && (y != 0) && (y != (boardSize - 1)))
            {
                if (((x - 1) % InternalInterval == 0) && ((y - 1) % InternalInterval == 0))
                {
                    return true;
                }
            }

            // are any neighbours filled?
            var neighbours = board.Neighbours(testCell);
            foreach (Cell neighbourCell in neighbours)
            {
                if (!neighbourCell.IsEmpty())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
