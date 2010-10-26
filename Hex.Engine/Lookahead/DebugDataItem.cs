//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Engine.Lookahead
{
    using Hex.Board;

    public class DebugDataItem
    {
        public int Lookahead { get; set; }

        public bool PlayerX { get; set; }

        public Location Location { get; set; }

        public int Alpha { get; set; }

        public int Beta { get; set; }

        public override string ToString()
        {
            string player = this.PlayerX ? "X" : "Y";
            return string.Format("Lookahead {0} Location {1} Player {2}", this.Lookahead, this.Location, player);
        }
    }
}
