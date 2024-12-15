using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для PageAboutProducts.xaml
    /// </summary>
    public partial class PageAboutProducts : Page
    {
        ProductDeliveryEntities DB = new ProductDeliveryEntities();
        public PageAboutProducts()
        {
            InitializeComponent();
            DGProducts.ItemsSource = DB.Products.ToList();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = DGProducts.SelectedItem as Products;

            if (selectedRows == null)
            {
                MessageBox.Show("Выберите строку для обновления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (MessageBox.Show("Вы действительно хотите обновить данные", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    MessageBox.Show($"Наименование: '{selectedRows.Name}', Категория: '{selectedRows.Category}', Единица продукта: '{selectedRows.Unit}'", "Отладочное сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

                    DB.Products.AddOrUpdate(selectedRows);
                    DB.SaveChanges();

                    DGProducts.ItemsSource = DB.Products.ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PageAboutProducts page = new PageAboutProducts();
            this.Content = page;
            Content = null;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = DGProducts.SelectedItem as Products;

            if (selectedRows == null)
            {
                MessageBox.Show("Выберите строку для добавления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedRows != null && MessageBox.Show("Вы действительно хотите обновить данные", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    MessageBox.Show($"Наименование: '{selectedRows.Name}', Категория: '{selectedRows.Category}', Единица продукта: '{selectedRows.Unit}'", "Отладочное сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

                    DB.Products.Add(selectedRows);
                    DB.SaveChanges();
                    DGProducts.ItemsSource = DB.Products.ToList();
                }

                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = DGProducts.SelectedItem as Products;

            if (selectedRows == null)
            {
                MessageBox.Show("Выберите строку для добавления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedRows != null && MessageBox.Show("Вы действительно хотите обновить данные", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    MessageBox.Show($"Наименование: '{selectedRows.Name}', Категория: '{selectedRows.Category}', Единица продукта: '{selectedRows.Unit}'", "Отладочное сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

                    DB.Products.Remove(selectedRows);
                    DB.SaveChanges();
                    DGProducts.ItemsSource = DB.Products.ToList();
                }

                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}
