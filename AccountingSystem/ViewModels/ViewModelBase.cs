using AccountingSystem.Commands;
using AccountingSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected Context _context;

        public event Action CloseRequest;
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand CloseCommand { get; }

        public ViewModelBase()
        {
            _context = new Context();

            CloseCommand = new RelayCommand(Close);
        }

        public void OnPropertyChanged([CallerMemberName]string callerName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }

        public virtual void Close()
        {
            CloseRequest?.Invoke();
        }
    }
}
