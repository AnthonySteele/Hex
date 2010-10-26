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

    /// <summary>
    /// Get candiate moves
    /// return all posiblities, in no particular order
    /// </summary>
    public class CandidateMovesAll : ICandidateMoves
    {
        public IEnumerable<Location> CandidateMoves(HexBoard board, int lookaheadDepth)
        {
            return board.EmptyCells();
        }
    }
}
