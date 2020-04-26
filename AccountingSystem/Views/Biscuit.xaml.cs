using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AccountingSystem.Views
{
   //Всплывающее уведомление
    public partial class Biscuit : UserControl
    {

        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(string), typeof(Biscuit), new PropertyMetadata(""));

        public static readonly DependencyProperty ShowDurationProperty =
             DependencyProperty.Register("ShowDuration", typeof(double), typeof(Biscuit), new PropertyMetadata(5d));

        public static readonly DependencyProperty IdentificatorProperty =
    DependencyProperty.Register("Identificator", typeof(string), typeof(Biscuit), new PropertyMetadata(null));

        public string Notification
        {
            get { return (string)GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        public double ShowDuration
        {
            get { return (double)GetValue(ShowDurationProperty); }
            set { SetValue(ShowDurationProperty, value); }
        }

        public string Identificator
        {
            get { return (string)GetValue(IdentificatorProperty); }
            set { SetValue(IdentificatorProperty, value); }
        }


        private static LinkedList<Biscuit> Biscuits { get; }
                                = new LinkedList<Biscuit>();

        public Biscuit()
        {
            InitializeComponent();
        }

        public void Show()
        {
            DoubleAnimation anim = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            anim.Completed += Hide;

            this.BeginAnimation(Biscuit.OpacityProperty, anim);
        }

        public async void Hide(object sender, EventArgs args)
        {
            double duration = ShowDuration;

            await Task.Run(() =>
            {
                var end = DateTime.Now.AddSeconds(duration);

                while (DateTime.Now <= end)
                {
                    Thread.Sleep(10);
                }
            });


            DoubleAnimation anim = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            this.BeginAnimation(Biscuit.OpacityProperty, anim);
        }

        public static void ShowBiscuit(string identificator)
        {
            Biscuit biscuit = Biscuits.FirstOrDefault(b => b.Identificator == identificator);

            if (biscuit == null)
                return;

            biscuit.Show();
        }

        private void Bisquit_Loaded(object sender, RoutedEventArgs e)
        {
            Biscuits.AddLast(this);
        }

        private void Bisquit_Unloaded(object sender, RoutedEventArgs e)
        {
            Biscuits.Remove(this);
        }
    }
}
