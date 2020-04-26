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
    /// Interaction logic for AddTeacherUserControl.xaml
    /// </summary>
    public partial class AddTeacherUserControl : UserControl
    {
        public AddTeacherViewModel ViewModel { get; }

        public AddTeacherUserControl(TeacherView teacher)
        {
            ViewModel = new AddTeacherViewModel(teacher);
            InitializeComponent();
        }


        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
