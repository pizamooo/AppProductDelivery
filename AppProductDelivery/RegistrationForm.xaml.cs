﻿using System;
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
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Создание анимации для исчезновения уведомления
            var notificationAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
            var notificationStoryboard = new Storyboard();
            notificationStoryboard.Children.Add(notificationAnimation);
            Storyboard.SetTarget(notificationAnimation, BeforeGreeting);
            Storyboard.SetTargetProperty(notificationAnimation, new PropertyPath(UIElement.OpacityProperty));

            // Запуска анимации исчезновения уведомления
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
    }
}
