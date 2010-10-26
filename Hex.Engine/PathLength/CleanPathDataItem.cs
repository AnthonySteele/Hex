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
    /// Data on the source of the score for a location
    /// </summary>
    public class CleanPathDataItem
    {
        private List<Location> sourceLocations;
        private bool valid = true;

        public int Score { get; set; }

        public List<Location> SourceLocations
        {
            get { return this.sourceLocations; }
            set { this.sourceLocations = value; }
        }

        public bool Valid
        {
            get { return this.valid; }
            set { this.valid = value; }
        }

        public bool HasSourceLocation(Location location)
        {
            return this.sourceLocations.Contains(location);
        }
    }
}
