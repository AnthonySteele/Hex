//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Wpf.Controls
{
    using Hex.Wpf.Helpers;
    using Hex.Wpf.Model;

    public class DoComputerMoveCommand : GenericCommand<HexBoardViewModel>
    {
        private readonly int lookaheadDepth;
        private HexBoardViewModel currentViewModel;

        public DoComputerMoveCommand(int lookaheadDepth)
        {
            this.lookaheadDepth = lookaheadDepth;
        }

        public override void ExecuteOnValue(HexBoardViewModel value)
        {
            this.currentViewModel = value;
            ComputerMoveCalculator moveCalculator = new ComputerMoveCalculator(value.Game, this.MoveCompleted, this.lookaheadDepth);
            moveCalculator.Move();
        }

        private void MoveCompleted(ComputerMoveData computerMoveData)
        {
            HexCellViewModel cellToPlay = this.currentViewModel.GetCellAtLocation(computerMoveData.Move);
            if (cellToPlay != null)
            {
                cellToPlay.PlayCell();
                this.currentViewModel.SetLastMoveDuration(computerMoveData.Time);
            }
        }
    }
}
