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
    /// Логика взаимодействия для AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow : Window
    {

        private Window _loginWindow;
        public AdminLoginWindow(Window LogInForm)
        {
            InitializeComponent();
            _loginWindow = LogInForm;
        }

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

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                ShowCustomMessageBox("Пожалуйста, введите логин и пароль администратора.");
                return;
            }

            string connectionString = "data source=DESKTOP-L92S7VB;initial catalog=ProductDelivery;integrated security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Position FROM Employees WHERE Login COLLATE Latin1_General_CS_AS = @login AND Password COLLATE Latin1_General_CS_AS = @password";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string role = result.ToString().Trim();
                        OpenRegistrationWindow(role);
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

        private void OpenRegistrationWindow(string role)
        {
            switch (role)
            {
                case "Директор":
                    RegistrationForm directorRegistrationWindow = new RegistrationForm();
                    _loginWindow.Close();
                    directorRegistrationWindow.Show();
                    break;
                case "Заместитель директора":
                    RegistrationForm deputyDirectorRegistrationWindow = new RegistrationForm();
                    _loginWindow.Close();
                    deputyDirectorRegistrationWindow.Show();
                    break;
                case "Бренд-Шеф":
                    RegistrationForm brandChefRegistrationWindow = new RegistrationForm();
                    _loginWindow.Close();
                    brandChefRegistrationWindow.Show();
                    break;
                default: 
                    this.Close();
                    ShowCustomMessageBox("Доступ запрещен");
                    break;
            }
        }
        private void ShowCustomMessageBox(string message)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
