namespace Hex.Wpf
{
    using Hex.Wpf.Controls;
    using Hex.Wpf.Debug;
    using Hex.Wpf.Helpers;

    public class ShowDebugBoardCommand : GenericCommand<MainWindowViewModel>
    {
        public override void ExecuteOnValue(MainWindowViewModel value)
        {
            HexBoardViewModel boardViewModel = new HexBoardViewModel(value.HexBoard);
            boardViewModel.ShowDebugData = true;
            boardViewModel.CanPlay = false;
            boardViewModel.GetDebugDataCommand.Execute(boardViewModel);

            DebugBoardWindowViewModel debugWindowViewModel = new DebugBoardWindowViewModel();
            debugWindowViewModel.HexBoard = boardViewModel;

            DebugBoardWindow debugBoard = new DebugBoardWindow();
            debugBoard.DataContext = debugWindowViewModel;

            debugBoard.Show();
        }
    }
}
