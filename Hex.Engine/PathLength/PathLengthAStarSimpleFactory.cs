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
    using Board;

    public class PathLengthAStarSimpleFactory : IPathLengthFactory
    {
        public PathLengthBase CreatePathLength(HexBoard board)
        {
            return new PathLengthAStar(board)
                {
                    UseNeighbours2 = false
                };
        }
    }
}