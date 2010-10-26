//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Wpf
{
    using System.Windows.Input;

    using Hex.Wpf.Controls;
    using Hex.Wpf.Helpers;

    public class MainWindowViewModel : BaseViewModel
    {
        private readonly HexBoardViewModel hexBoard;
        private readonly ActionCommand<MainWindowViewModel> clearCommand;
        private readonly ActionCommand<MainWindowViewModel> computerMoveCommand;
        private readonly ICommand showDebugBoardCommand;

        public MainWindowViewModel(HexBoardViewModel hexBoard)
        {
            this.hexBoard = hexBoard;
            this.hexBoard.OnCellPlayed += this.HexBoardCellPlayed;

            this.clearCommand = new ActionCommand<MainWindowViewModel>(vm => vm.ClearBoard());
            this.clearCommand.Enabled = false;

            this.showDebugBoardCommand = new ShowDebugBoardCommand();

            this.computerMoveCommand = new ActionCommand<MainWindowViewModel>(vm => vm.DoComputerMove());
        }

        public ICommand ClearCommand
        {
            get { return this.clearCommand; }
        }

        public ICommand ShowDebugBoardCommand
        {
            get { return this.showDebugBoardCommand; }
        }

        public ICommand ComputerMoveCommand
        {
            get { return this.computerMoveCommand; }
        }

        public HexBoardViewModel HexBoard
        {
            get { return this.hexBoard;  }
        }

        private void ClearBoard()
        {
            this.hexBoard.ClearBoard();
            this.clearCommand.Enabled = false;
        }

        private void HexBoardCellPlayed()
        {
            this.clearCommand.Enabled = true;
        }

        private void DoComputerMove()
        {
            this.HexBoard.DoComputerMoveCommand.Execute(this.HexBoard);
        }
    }
}
