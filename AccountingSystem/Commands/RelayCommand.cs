using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccountingSystem.Commands
{
    public class RelayCommand : ICommand
    {
        protected readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ??
                       throw new ArgumentNullException(nameof(execute));

            _canExecute = canExecute;
        }

        public virtual bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public virtual void Execute(object parameter)
        {
            _execute();
        }
    }
}
