namespace Hex.Wpf.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class HexBoard : UserControl
    {
        public HexBoard()
        {
            InitializeComponent();
        }

        private void HexBoardSizeChanged(object sender, SizeChangedEventArgs e)
        {
            HexBoardViewModel viewModel = this.DataContext as HexBoardViewModel;
            if (viewModel != null)
            {
                viewModel.SetActualBoardSize(e.NewSize);
            }
        }

        private void HexBoardDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HexBoardViewModel viewModel = this.DataContext as HexBoardViewModel;
            if (viewModel != null)
            {
                viewModel.SetActualBoardSize(new Size(this.ActualWidth, this.ActualHeight));
            }
        }
    }
}
