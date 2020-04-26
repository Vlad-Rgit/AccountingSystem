using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccountingSystem.Commands
{
    public class RelayCommand<T> : ICommand
    {

        private Func<T, bool> _canExecute;
        private Action<T> _execute;


        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
            => (_execute, _canExecute) = (execute, canExecute);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
            => _canExecute == null ? true : _canExecute((T)parameter);

        public void Execute(object parameter)
            => _execute((T)parameter);

    }
}
