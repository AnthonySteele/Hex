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
    using System.Collections.Generic;

    using Hex.Board;

    /// <summary>
    /// Base class for path length algorithyms
    /// </summary>
    public abstract class PathLengthBase
    {
        #region data

        protected HexBoard board;
        protected bool playerX;
        protected int[,] vals;

        protected bool[,] onPathData;
        #endregion

        #region abstract interface

        public abstract int PlayerScore(bool playerX);

        public abstract List<Location> GetCleanPath(bool playerX);

        #endregion
        
        #region calculate

        public int SituationScore()
        {
            int playerXScore = this.PlayerScore(true);

            if (playerXScore == 0)
            {
                // player x wins
                return MoveScoreConverter.ConvertWin(Occupied.PlayerX, 0);
            }

            if (playerXScore == PathLengthConstants.OffPath)
            {
                // player X loses -> player y wins
                return MoveScoreConverter.ConvertWin(Occupied.PlayerY, 0);
            }

            // no win yet. score for the situation
            int playerYScore = this.PlayerScore(false);

            return playerYScore - playerXScore;
        }

        #endregion

        #region expose values

        public int Value(Location loc)
        {
            return this.vals[loc.X, loc.Y];
        }

        public int Value(int x, int y)
        {
            return this.vals[x, y];
        }

        public bool IsOnPath(Location loc)
        {
            if (this.onPathData != null)
            {
                return this.onPathData[loc.X, loc.Y];
            }

            return true;
        }

        public bool IsOnPath(int x, int y)
        {
            if (this.onPathData != null)
            {
                return this.onPathData[x, y];
            }
            
            return true;
        }

        #endregion
    }
}
