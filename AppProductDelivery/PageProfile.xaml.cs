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
                var command = new SqlCommand("SELECT login, password, email, Name, LastName, MiddleName FROM Employees WHERE EmployeeID = @UserId", connection);
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

            try 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("UPDATE Employees SET login = @login, password = @password, email = @email, LastName = @LastName, Name = @Name, MiddleName = @MiddleName WHERE EmployeeID = @UserId", connection);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@login", LoginTextBox.Text);
                    command.Parameters.AddWithValue("@email", EmailTextBox.Text);
                    command.Parameters.AddWithValue("@password", PasswordTextBox.Text);
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
        private void ShowCustomMessageBox(string message)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(message);
            customMessageBox.ShowDialog();
        }
    }
}
