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
    /// <summary>
    /// state of a cell - empty, or played by one of the players
    /// </summary>
    public enum Occupied
    {
        /// <summary>
        /// the cell has not been played
        /// </summary>
        Empty = 0, 

        /// <summary>
        /// Played by player X
        /// </summary>
        PlayerX, 

        /// <summary>
        /// Played by player Y
        /// </summary>
        PlayerY
    }
}