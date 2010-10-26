//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
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
