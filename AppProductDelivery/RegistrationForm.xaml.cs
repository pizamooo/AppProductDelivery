using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        public RegistrationForm()
        {
            InitializeComponent();

            AcceptTerm.Checked += AcceptTerm_Checked;
            AcceptTerm.Unchecked += AcceptTerm_Checked;
            Login.TextChanged += Login_TextChanged;
            Email.TextChanged += Email_TextChanged;
            RetryPasword.PasswordChanged += RetryPasword_PasswordChanged;
            Password.PasswordChanged += Password_PasswordChanged;
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

                // Настраивании анимации для второй картинки
                var imageStoryboard = new Storyboard();
                imageStoryboard.Children.Add(imageScaleAnimationX);
                imageStoryboard.Children.Add(imageScaleAnimationY);
                imageStoryboard.Children.Add(imageOpacityAnimation);
                Storyboard.SetTarget(imageScaleAnimationX, ImageGreeting);
                Storyboard.SetTargetProperty(imageScaleAnimationX, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                Storyboard.SetTarget(imageScaleAnimationY, ImageGreeting);
                Storyboard.SetTargetProperty(imageScaleAnimationY, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                Storyboard.SetTarget(imageOpacityAnimation, ImageGreeting);
                Storyboard.SetTargetProperty(imageOpacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                // Текст и вторая картинка
                Greeting.Visibility = Visibility.Visible;
                ImageGreeting.Visibility = Visibility.Visible;

                // Запуск анимации
                textStoryboard.Begin();
                imageStoryboard.Begin();
            };

            notificationStoryboard.Begin();
        }

        //Проверка для включения кнопки регистрации при условии, что чекбокс, пароль и логин введены
        private void UpdateSignUpButtonState()
        {
            bool isUsernameFilled = !string.IsNullOrEmpty(Login.Text);
            bool isEmailFilled = !string.IsNullOrEmpty(Email.Text);
            bool isPasswordFilled = !string.IsNullOrEmpty(Password.Password);
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
        //Конвертация пароля в string для считывания кнопки
        private void RetryPasword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateSignUpButtonState();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateSignUpButtonState();
        }
    }
}
