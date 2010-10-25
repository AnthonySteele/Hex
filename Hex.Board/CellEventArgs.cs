namespace Hex.Board
{
    using System;

    /// <summary>
    /// event args for events about cells 
    /// </summary>
    public class CellEventArgs : EventArgs
    {
        private readonly Cell cell;

        public CellEventArgs(Cell cell)
        {
            this.cell = cell;
        }

        public Cell Cell
        {
            get { return this.cell; }
        }

        public Location Loc
        {
            get
            {
                if (this.cell == null)
                {
                    return Location.Null;
                }
                
                return this.cell.Location;
            }
        }
    }
}
