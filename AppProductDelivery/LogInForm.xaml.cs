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
    /// Логика взаимодействия для LogInForm.xaml
    /// </summary>
    public partial class LogInForm : Window
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void TextBlock_MouseLeftButtonDownBack(object sender, MouseButtonEventArgs e)
        {
            AdminLoginWindow window = new AdminLoginWindow(this);
            window.ShowDialog();
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
        //посмотреть пароль
        private void CheckPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordRetrySneaky.Text = Password.Password;
            Password.Visibility = Visibility.Collapsed;
            PasswordRetrySneaky.Visibility = Visibility.Visible;
        }

        private void CheckPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            Password.Password = PasswordRetrySneaky.Text;
            PasswordRetrySneaky.Visibility = Visibility.Collapsed;
            Password.Visibility = Visibility.Visible;
        }
        //обработка событии кнопки
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                ShowCustomMessageBox("Пожалуйста, введите логин и пароль.");
                return;
            }

            string connectionString = "data source=DESKTOP-L92S7VB;initial catalog=ProductDelivery;integrated security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM Employees WHERE Login COLLATE Latin1_General_CS_AS = @login AND Password COLLATE Latin1_General_CS_AS = @password";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 1)
                    {
                        MainWindowApp window = new MainWindowApp(login);
                        window.Show();
                        this.Close();
                    }
                    else
                    {
                        ShowCustomMessageBox("Неверный логин или пароль.");
                    }
                }

                catch (SqlException sqlEx)
                {
                    ShowCustomMessageBox("Ошибка базы данных: " + sqlEx.Message);
                }

                catch (Exception ex)
                {
                    ShowCustomMessageBox("Ошибка: " + ex.Message);
                }
            }
        }

        private void ShowCustomMessageBox(string message)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(message);
            customMessageBox.ShowDialog();
        }

        private void PasswordRetrySneaky_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password.Password = PasswordRetrySneaky.Text;
        }
    }
}
