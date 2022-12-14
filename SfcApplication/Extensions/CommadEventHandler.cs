using System;
using System.Windows.Input;

namespace SfcApplication.Extensions
{
    public class CommadEventHandler<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action<T> action;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action((T)parameter);
        }
        public CommadEventHandler(Action<T> action)
        {
            this.action = action;

        }
    }
}
