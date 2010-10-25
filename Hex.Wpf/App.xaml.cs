using Hex.Wpf.Controls;

namespace Hex.Wpf
{
    using System.Windows;
    using Hex.Wpf.SelectGame;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ShowGameTypeDialog()
        {
            SelectGameWindow selectGameWindow = new SelectGameWindow();
            selectGameWindow.DataContext = new SelectGameViewModel(
                vm =>
                    {
                        ShowMainWindow(vm);
                        selectGameWindow.Close();
                    }, 
                vm => selectGameWindow.Close());

            selectGameWindow.Show();
        }

        private void ShowMainWindow(SelectGameViewModel selectGameViewModel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel(new HexBoardViewModel(selectGameViewModel));

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = viewModel;
            this.MainWindow = mainWindow;

            mainWindow.Show();
        }
        
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            this.ShowGameTypeDialog();
        }
    }
}
