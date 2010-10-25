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
