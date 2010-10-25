namespace Hex.Wpf.Helpers
{
    using System;
    using System.Windows.Input;
    
    public class ActionCommand<T> : ICommand
    {
      private readonly Action<T> action;
      private bool enabled;

      public ActionCommand(Action<T> action)
      {
          this.action = action;
          this.Enabled = true;
      }

      public event EventHandler CanExecuteChanged;
  
      public bool Enabled
      {
          get
          {
              return this.enabled;
          }

          set
          {
              this.enabled = value;
              var canExecuteChanged = this.CanExecuteChanged;

              if (canExecuteChanged != null)
              {
                  canExecuteChanged(this, EventArgs.Empty);
              }
          }
      }

      public bool CanExecute(object parameter)
      {
          return this.Enabled;
      }

      public void Execute(object parameter)
      {
          this.action((T)parameter);
      }
  }
}
