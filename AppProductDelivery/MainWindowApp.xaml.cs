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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppProductDelivery
{
    /// <summary>
    /// Логика взаимодействия для MainWindowApp.xaml
    /// </summary>
    public partial class MainWindowApp : Window
    {
        public MainWindowApp(string login)
        {
            InitializeComponent();
            LoginName.Text = $" {login}";
        }

        //кнопка скрытия
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            animation.Completed += (s, args) =>
            {
                this.WindowState = WindowState.Minimized;
                this.StateChanged += Window_StateChanged;
            };
            this.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                var animation = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5))
                };
                this.BeginAnimation(UIElement.OpacityProperty, animation);
                this.StateChanged -= Window_StateChanged;
            }
        }
        //кнопка закрытия
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            animation.Completed += (s, args) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, animation);
        }
        //передвижение окна
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        //выход на авторизацию
        private void ExitToBack_Click(object sender, RoutedEventArgs e)
        {
            LogInForm window = new LogInForm();
            window.Show();
            this.Close();
        }

        private void Suppliers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new PageSuppliers());
        }

        private void InfoAboutProducts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new PageAboutProducts());
        }

        private void Delivery_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new PageDelivery());
        }
    }
}
