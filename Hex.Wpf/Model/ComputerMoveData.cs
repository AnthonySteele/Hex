//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Wpf.Model
{
    using System;
    using Hex.Board;

    public class ComputerMoveData
    {
        public Location Move { get; set; }

        public TimeSpan Time { get; set; }

        public bool IsGameWon { get; set; }

        public bool PlayerX { get; set; }
    }
}