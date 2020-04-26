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
using System.Windows.Shapes;

namespace AccountingSystem.Views
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public string Message { get; set; }
        public string Caption { get; set; }
        public bool IsYesNo { get; set; }
        public bool IsOk => !IsYesNo;

        public CustomMessageBox(string caption, string message, bool isYesNo = false)
        {
            Caption = caption;
            Message = message;
            IsYesNo = isYesNo;
            InitializeComponent();          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void YesClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void NoClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
