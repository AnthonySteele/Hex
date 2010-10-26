//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Wpf.Debug
{
    using System.Windows.Input;

    using Hex.Wpf.Controls;

    public class DebugBoardWindowViewModel : BaseViewModel
    {
        public DebugBoardWindowViewModel()
        {
            this.DoComputerMoveCommand = new DoComputerMoveCommand();
        }

        public HexBoardViewModel HexBoard { get; set; }

        public ICommand DoComputerMoveCommand { get; set; }
    }
}
