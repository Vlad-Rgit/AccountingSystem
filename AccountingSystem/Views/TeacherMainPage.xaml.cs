﻿using AccountingSystem.Models;
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
    /// Interaction logic for TeacherMainFrame.xaml
    /// </summary>
    public partial class TeacherMainPage : Page
    {
        public TeacherViewModel ViewModel { get; set; }

        public TeacherMainPage(TeacherView teacher)
        {
            ViewModel = new TeacherViewModel(teacher);
            InitializeComponent();
        }
    }
}
