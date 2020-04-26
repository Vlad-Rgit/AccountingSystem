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
    /// Interaction logic for JournalUserControl.xaml
    /// </summary>
    public partial class JournalUserControl : UserControl
    {
        public JournalViewModel ViewModel { get; } = new JournalViewModel();

        public JournalUserControl(bool isAdmin)
        {
            InitializeComponent();


            saveButton.Visibility = isAdmin == true ? Visibility.Visible : Visibility.Collapsed;

            var end =   DateTime.Parse("2021/1/1");
            for (DateTime i = DateTime.Parse("1900/1/1"); i < end; i = i.AddDays(1))
            {
                if (i.DayOfWeek != DayOfWeek.Monday)
                {
                    CalendarDateRange range = new CalendarDateRange(i);
                    datepicker.BlackoutDates.Add(range);
                }
            }
         
        }
    }
}
