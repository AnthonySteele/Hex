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
