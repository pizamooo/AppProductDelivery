using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private int userId;
        private string login;
        private string userPosition;
        public MainWindowApp(string login, int userId, string userPosition)
        {
            InitializeComponent();
            this.userId = userId;
            this.login = login;
            this.userPosition = userPosition;
            LoginName.Text = $"{login}";
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
            if (!string.IsNullOrEmpty(userPosition) && (userPosition == "Директор" || userPosition == "Заместитель директора" || userPosition == "Бренд-Шеф"))
            {
                MainFrame.Navigate(new PageSuppliers());
            }
            else
            {
                ShowCustomMessageBox("Недостаточно прав для продолжения.");
            }
        }

        private void InfoAboutProducts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(userPosition) && (userPosition == "Директор" || userPosition == "Заместитель директора" || userPosition == "Бренд-Шеф"))
            { 
                MainFrame.Navigate(new PageAboutProducts());
            }
            else
            {
                ShowCustomMessageBox("Недостаточно прав для продолжения.");
            }
        }

        private void Delivery_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(userPosition) &&(userPosition == "Директор" || userPosition == "Заместитель директора" || userPosition == "Бренд-Шеф"))
            { 
                MainFrame.Navigate(new PageDelivery());
            }
            else
            {
                ShowCustomMessageBox("Недостаточно прав для продолжения.");
            }
        }

        private void LoginName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new PageProfile(userId));
        }

        private void OrderLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new PageOrder());
        }

        private void ShowCustomMessageBox(string message)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(message);
            customMessageBox.ShowDialog();
        }

        private void Journal_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
