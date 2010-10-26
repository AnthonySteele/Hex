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
