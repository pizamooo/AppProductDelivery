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
    /// Логика взаимодействия для PageSuppliers.xaml
    /// </summary>
    public partial class PageSuppliers : Page
    {
        ProductDeliveryEntities DB = new ProductDeliveryEntities();
        public PageSuppliers()
        {
            InitializeComponent();
            DGSuppliers.ItemsSource = DB.Suppliers.ToList();

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = DGSuppliers.SelectedItem as Suppliers;

            if (selectedRows == null)
            {
                MessageBox.Show("Выберите строку для обновления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы действительно хотите обновить данные", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    MessageBox.Show($"Наименование: '{selectedRows.Name}', Адрес: '{selectedRows.Address}', Контактная информация: '{selectedRows.ContactInfo}'", "Отладочное сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

                    DB.Suppliers.AddOrUpdate(selectedRows);
                    DB.SaveChanges();


                    DGSuppliers.ItemsSource = DB.Suppliers.ToList();
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
            PageSuppliers page = new PageSuppliers();
            this.Content = page;
            Content = null;
        }
    }
}
