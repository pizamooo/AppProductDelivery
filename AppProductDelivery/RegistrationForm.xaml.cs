using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
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

namespace AppProductDelivery
{
    /// <summary>
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
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

                // Настраивание анимации для текста
                var textStoryboard = new Storyboard();
                textStoryboard.Children.Add(textScaleAnimationX);
                textStoryboard.Children.Add(textScaleAnimationY);
                textStoryboard.Children.Add(textOpacityAnimation);
                Storyboard.SetTarget(textScaleAnimationX, Greeting);
                Storyboard.SetTargetProperty(textScaleAnimationX, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
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
            bool isPasswordFilled = !string.IsNullOrEmpty(Password.Text);
            bool isRetryPasswordFilled = !string.IsNullOrEmpty(RetryPasword.Password);
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

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSignUpButtonState();
        }

        private void RetryPasword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateSignUpButtonState();
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

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            animation.Completed += (s, args) => this.Hide();
            this.BeginAnimation(UIElement.OpacityProperty, animation);
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
        // Нажатие чекбокса просмотреть пароль
        private void CheckPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordRetrySneaky.Text = RetryPasword.Password;
            RetryPasword.Visibility = Visibility.Collapsed;
            PasswordRetrySneaky.Visibility = Visibility.Visible;
        }

        private void CheckPassword_Uncheked(object sender, RoutedEventArgs e)
        {
            RetryPasword.Password = PasswordRetrySneaky.Text;
            PasswordRetrySneaky.Visibility = Visibility.Collapsed;
            RetryPasword.Visibility = Visibility.Visible;
        }
        // проверка на наличие спец символов и соответствие пароля
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(Login.Text) || !IsValidLogin(Login.Text))
            {
                Login.Tag  = true;
                isValid = false;
            }
            else
            {
                Login.Tag  = false;
            }

            if (string.IsNullOrWhiteSpace(Password.Text))
            {
                Password.Tag = true;
                isValid = false;
            }
            else
            {
                Password.Tag  = false;
            }

            if (RetryPasword.Password != Password.Text && PasswordRetrySneaky.Text != Password.Text)
            {
                RetryPasword.Tag  = true;
                PasswordRetrySneaky.Tag = true;
                isValid = false;
            }
            else
            {
                RetryPasword.Tag  = false;
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
                MessageBox.Show("Регистрация успешно пройдена!");
            }
            else
            {
                MessageBox.Show("Пожалуйста исправьте ошибки!.");
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
    }
}
