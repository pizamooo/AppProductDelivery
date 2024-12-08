using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Data;
using System.Data.SqlClient;

namespace AppProductDelivery
{
    /// <summary>
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        private int userId;
        public RegistrationForm()
        {
            InitializeComponent();
            StartAnimationInRegistrationForm();
        }

        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Создание анимации для исчезновения приветственного текста
            var notificationAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
            var notificationStoryboard = new Storyboard();
            notificationStoryboard.Children.Add(notificationAnimation);
            Storyboard.SetTarget(notificationAnimation, BeforeGreeting);
            Storyboard.SetTargetProperty(notificationAnimation, new PropertyPath(UIElement.OpacityProperty));

            // Запуска анимации исчезновения приветственного текста
            notificationStoryboard.Completed += (s, args) =>
            {
                // Создание анимации для появления текста и второй картинки
                var textScaleAnimationX = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                var textScaleAnimationY = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                var textOpacityAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));

                var imageScaleAnimationX = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                var imageScaleAnimationY = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                var imageOpacityAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));

                var textStoryboard = new Storyboard();
                textStoryboard.Children.Add(textScaleAnimationX);
                textStoryboard.Children.Add(textScaleAnimationY);
                textStoryboard.Children.Add(textOpacityAnimation);
                Storyboard.SetTarget(textScaleAnimationX, Greeting);
                Storyboard.SetTargetProperty(textScaleAnimationX, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                // Настраивание анимации для текста
                Storyboard.SetTarget(textScaleAnimationY, Greeting);
                Storyboard.SetTargetProperty(textScaleAnimationY, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                Storyboard.SetTarget(textOpacityAnimation, Greeting);
                Storyboard.SetTargetProperty(textOpacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                // Текст и вторая картинка
                Greeting.Visibility = Visibility.Visible;

                // Запуск анимации
                textStoryboard.Begin();
            };

            notificationStoryboard.Begin();
        }

        //Проверка для включения кнопки регистрации при условии, что чекбокс, пароль и логин введены
        private void UpdateSignUpButtonState()
        {
            bool isUsernameFilled = !string.IsNullOrEmpty(Login.Text);
            bool isEmailFilled = !string.IsNullOrEmpty(Email.Text);
            bool isPasswordFilled = !string.IsNullOrEmpty(Password.Password);
            bool isRetryPasswordFilled = !string.IsNullOrEmpty(RetryPassword.Password);
            bool isTermsAccepted = AcceptTerm.IsChecked == true;

            SignUp.IsEnabled = isUsernameFilled && isPasswordFilled && isTermsAccepted && isEmailFilled && isRetryPasswordFilled;
        }

        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSignUpButtonState();
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSignUpButtonState();
        }

        private void AcceptTerm_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSignUpButtonState();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateSignUpButtonState();
        }

        private void RetryPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateSignUpButtonState();
        }

        private void PasswordRetrySneaky_TextChanged(object sender, TextChangedEventArgs e)
        {
            RetryPassword.Password = PasswordRetrySneaky.Text;
        }

        private void StartAnimationInRegistrationForm()
        {
            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 1.05,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            // Принятие анимации к изображению
            ImageScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            ImageScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        //кнопка минимизации
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

        // Нажатие чекбокса просмотреть пароль
        private void CheckPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordRetrySneaky.Text = RetryPassword.Password;
            RetryPassword.Visibility = Visibility.Collapsed;
            PasswordRetrySneaky.Visibility = Visibility.Visible;
        }

        private void CheckPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            RetryPassword.Password = PasswordRetrySneaky.Text;
            PasswordRetrySneaky.Visibility = Visibility.Collapsed;
            RetryPassword.Visibility = Visibility.Visible;
        }

        // проверка на наличие спец символов и соответствие пароля для кнопки регистрации
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            string login = Login.Text;
            string password = Password.Password;
            string email = Email.Text;

            if (string.IsNullOrWhiteSpace(Login.Text) || Login.Text.Length < 5 || !IsValidLogin(Login.Text))
            {
                Login.Tag  = true;
                isValid = false;
            }
            else
            {
                Login.Tag  = false;
            }

            if (string.IsNullOrWhiteSpace(Password.Password) || Password.Password.Length < 5)
            {
                Password.Tag = true;
                isValid = false;
            }
            else
            {
                Password.Tag  = false;
            }

            if (RetryPassword.Password != Password.Password && PasswordRetrySneaky.Text != Password.Password)
            {
                RetryPassword.Tag  = true;
                PasswordRetrySneaky.Tag = true;
                isValid = false;
            }
            else
            {
                RetryPassword.Tag  = false;
                PasswordRetrySneaky.Tag = false;
            }

            if (!IsValidEmail(Email.Text))
            {
                Email.Tag  = true;
                isValid = false;
            }
            else
            {
                Email.Tag  = false;
            }

            if (isValid)
            {
                int userId = RegisterUser(login, password, email);
                if (userId != -1)
                {
                    ShowCustomMessageBox("Регистрация успешна!");
                    MainWindowApp window = new MainWindowApp(login, userId);
                    window.Show();
                    this.Close();
                }
                else
                {
                    ShowCustomMessageBox("Ошибка регистрации. Возможно такой логин уже существует");
                }
            }
            else
            {
                ShowCustomMessageBox("Пожалуйста исправьте ошибки!");
            }
        }
        //Функции для проверки спец знаков
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
        //Движение мышки
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        // проверка на наличие спец символов и соответствие пароля для кнопки войти
        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            LogInForm window = new LogInForm();
            window.Show();
            this.Close();
        }

        private int RegisterUser(string login, string password, string email)
        {
            string connectionString="data source=DESKTOP-L92S7VB;initial catalog=ProductDelivery;integrated security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand checkUserCmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE login = @login", connection);
                    checkUserCmd.Parameters.AddWithValue("@login", login);
                    int userExists = (int)checkUserCmd.ExecuteScalar();
                    if (userExists > 0)
                        return -1;

                    SqlCommand insertUserCmd = new SqlCommand("INSERT INTO Employees (login, password, email) VALUES (@login,@password,@email); SELECT SCOPE_IDENTITY();", connection);
                    insertUserCmd.Parameters.AddWithValue("@login", login);
                    insertUserCmd.Parameters.AddWithValue("@password", password);
                    insertUserCmd.Parameters.AddWithValue("@email", email);


                    int userId = Convert.ToInt32(insertUserCmd.ExecuteScalar());
                    return userId;
                }
                catch (SqlException sqlEx)
                {
                    ShowCustomMessageBox("SQL error: " + sqlEx.Message);
                    return -1;
                }
                catch (Exception ex)
                {
                    ShowCustomMessageBox("Database error: " + ex.Message);
                    return -1;
                }
            }
        }

        private void ShowCustomMessageBox(string message)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(message);
            customMessageBox.ShowDialog();
        }

    }
}

