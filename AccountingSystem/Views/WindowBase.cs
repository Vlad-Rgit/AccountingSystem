using AccountingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AccountingSystem.Views
{
    public class WindowBase<TViewModel> : Window
        where TViewModel : ViewModelBase, new()
    {
        public TViewModel ViewModel { get; set; }

        public WindowBase()
        {
            ViewModel = new TViewModel();
            ViewModel.CloseRequest += Close;
        }

        public void OnDrag(object sender, EventArgs args)
        {
            DragMove();
        }
    }
}
