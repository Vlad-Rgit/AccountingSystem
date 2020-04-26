using AccountingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AccountingSystem.Views
{
    /// <summary>
    /// Interaction logic for StudentsUserControl.xaml
    /// </summary>
    public partial class StudentsUserControl : UserControl
    {
        public StudentsViewModel ViewModel { get; set; }

        public StudentsUserControl(bool isToEdit)
        {
            ViewModel = new StudentsViewModel(isToEdit);
            InitializeComponent();
        }
    }
}
