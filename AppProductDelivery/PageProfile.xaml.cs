using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AppProductDelivery
{
    /// <summary>
    /// Логика взаимодействия для PageProfile.xaml
    /// </summary>
    public partial class PageProfile : Page
    {
        private int userId;
        public PageProfile(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadUserProfile(userId);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }

        private void LoadUserProfile(int userId)
        {
            string connectionString = "data source=DESKTOP-L92S7VB;initial catalog=ProductDelivery;integrated security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT login, password, email, Name, LastName, MiddleName, Position FROM Employees WHERE EmployeeID = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        LoginTextBox.Text = reader["login"].ToString();
                        PasswordTextBox.Text = reader["password"].ToString();
                        EmailTextBox.Text = reader["email"].ToString();
                        LastNameTextBox.Text = reader["LastName"].ToString();
                        NameTextBox.Text = reader["Name"].ToString();
                        MiddleNameTextBox.Text = reader["MiddleName"].ToString();
                        EmployeeTextBox.Text = reader["Position"].ToString();

                        var position = reader["Position"];
                        EmployeeTextBox.Text = position == DBNull.Value ? "Сотрудник" : position.ToString();
                    }
                    else
                    {
                        ShowCustomMessageBox("Ошибка");
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "data source=DESKTOP-L92S7VB;initial catalog=ProductDelivery;integrated security=True;";

            bool isValid = true;

            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;
            string email = EmailTextBox.Text;

            if (string.IsNullOrWhiteSpace(login) || login.Length < 5 || !IsValidLogin(login))
            {
                ShowCustomMessageBox("Логин должен содержать не менее 5 символов и не содержать специальных символов.");
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 5)
            {
                ShowCustomMessageBox("Логин должен содержать не менее 5 символов.");
                PasswordTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                PasswordTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (!IsValidEmail(email))
            {
                ShowCustomMessageBox("Некорректный формат email.");
                EmailTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                EmailTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (!isValid)
            {
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("UPDATE Employees SET login = @login, password = @password, email = @email, LastName = @LastName, Name = @Name, MiddleName = @MiddleName WHERE EmployeeID = @UserId", connection);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                    command.Parameters.AddWithValue("@Name", NameTextBox.Text);
                    command.Parameters.AddWithValue("@MiddleName", MiddleNameTextBox.Text);

                    command.ExecuteNonQuery();
                }
                ShowCustomMessageBox("Профиль успешно сохранен!");
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
    
        private bool IsValidLogin(string login)
        {
            string pattern = @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]";
            return !Regex.IsMatch(login, pattern);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                string specialCharPattern = @"[!#$%^&*()_+=\[{\]};:<>|/?,-]";
                if (Regex.IsMatch(email, specialCharPattern))
                    return false;

                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private void ShowCustomMessageBox(string message)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
