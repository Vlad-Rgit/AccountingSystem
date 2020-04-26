using AccountingSystem.Models;
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
    /// Interaction logic for AddScheduleUserControl.xaml
    /// </summary>
    public partial class AddScheduleUserControl : UserControl
    {
        public AddScheduleViewModel ViewModel { get; set; }

        public AddScheduleUserControl(string group)
        {
            ViewModel = new AddScheduleViewModel(group);
            InitializeComponent();
        }

        public AddScheduleUserControl(ScheduleView schedule)
        {
            ViewModel = new AddScheduleViewModel(schedule);
            InitializeComponent();
        }
    }
}
