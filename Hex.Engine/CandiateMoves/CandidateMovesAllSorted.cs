namespace Hex.Engine.CandiateMoves
{
    using System.Collections.Generic;

    using Hex.Board;
    using Hex.Engine.Lookahead;

    /// <summary>
    /// Get candiate moves
    /// return all possible moves, with good moves first
    /// </summary>
    public class CandidateMovesAllSorted : ICandidateMoves
    {
        private readonly GoodMoves goodMoves;
        private readonly int cellsPlayedCount;

        public CandidateMovesAllSorted(GoodMoves goodMoves, int cellsPlayedCount)
        {
            this.goodMoves = goodMoves;
            this.cellsPlayedCount = cellsPlayedCount;
        }

        /// <summary>
        ///  get candidate moves - 
        ///  this list is comprehensive - it will contain all empty cells on the board
        ///  and sorted - good moves come first
        ///  the list returned may be longer than the valid cells
        ///  but then will have a null location at the end
        ///  Actual length is (BoardSize ^ 2) - number of moves already played
        /// </summary>
        /// <param name="board">the board to read</param>
        /// <param name="lookaheadDepth">the current depth of lookahead</param>
        /// <returns>the locations</returns>
        public IEnumerable<Location> CandidateMoves(HexBoard board, int lookaheadDepth)
        {
            // No potential good moves? return them all then
            if (this.goodMoves.GetCount(lookaheadDepth) == 0)
            {
                return board.EmptyCells();
            }

            int maxListLength = (board.Size * board.Size) - this.cellsPlayedCount;

            // enough space for all the possible moves
            Location[] result = new Location[maxListLength];
            int resultIndex = 0;

            // mask out the ones that have been used - intialised to false
            bool[,] maskCellSelected = new bool[board.Size, board.Size];

            // good moves are found first
            Location[] myGoodMoves = this.goodMoves.GetGoodMoves(lookaheadDepth);

            // copy in the good moves
            foreach (Location goodMoveLoc in myGoodMoves)
            {
                if (board.GetCellAt(goodMoveLoc).IsEmpty() && (!maskCellSelected[goodMoveLoc.X, goodMoveLoc.Y]))
                {
                    result[resultIndex] = goodMoveLoc;
                    resultIndex++;
                    maskCellSelected[goodMoveLoc.X, goodMoveLoc.Y] = true;
                }
            }

            // copy in all moves where the cell is empty;
            // and not already in by virtue of being a good move
            foreach (Cell testCell in board.GetCells())
            {
                if (testCell.IsEmpty() && (!maskCellSelected[testCell.X, testCell.Y]))
                {
                    result[resultIndex] = testCell.Location;
                    resultIndex++;
                }
            }

            // null marker at the end
            if (resultIndex < maxListLength)
            {
                result[resultIndex] = Location.Null;
            }

            return result;
        }
    }
}
