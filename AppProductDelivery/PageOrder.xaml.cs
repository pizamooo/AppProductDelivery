using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для PageOrder.xaml
    /// </summary>
    public partial class PageOrder : Page
    {
        private ProductDeliveryEntities _context;

        public PageOrder()
        {
            InitializeComponent();
            _context = new ProductDeliveryEntities();
            LoadData();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }
        private void LoadData()
        {
            var products = _context.Products.ToList();
            ComboBoxProducts.ItemsSource = products.Select(p => p.Name).Distinct().ToList();
            ComboBoxCategory.ItemsSource = products.Select(p => p.Category).Distinct().ToList();
            ComboBoxUnit.ItemsSource = products.Select(p => p.Unit).Distinct().ToList();
            ComboBoxSupplier.ItemsSource = _context.Suppliers.Select(s => s.Name).Distinct().ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ComboBoxProducts.SelectedItem as string;
            var selectedCategory = ComboBoxCategory.SelectedItem as string;
            var selectedUnit = ComboBoxUnit.SelectedItem as string;
            var selectedSupplier = ComboBoxSupplier.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedProduct) && !string.IsNullOrEmpty(selectedCategory) && !string.IsNullOrEmpty(selectedUnit) && selectedSupplier != null)
            {
                var product = _context.Products.FirstOrDefault(p => p.Name == selectedProduct);
                var supplier = _context.Suppliers.FirstOrDefault(s => s.Name == selectedSupplier);
                if (product != null && supplier != null)
                {
                    var order = new Orders()
                    {
                        OrderDate = DateTime.Now,
                        TotalAmount = (double)decimal.Parse(TextBoxSupplyPrice.Text)
                    };

                    var delivery = new Deliveries()
                    {
                        SupplierID = supplier.SupplierID,
                        ProductID = product.ProductID,
                        OrderID = order.OrderID,
                        DeliveryDate = DateTime.Now,
                        Quantity = int.Parse(TextBoxQuantity.Text),
                        UnitPrice = (double)decimal.Parse(TextBoxPrice.Text),
                        TotalAmount = (double)decimal.Parse(TextBoxTotalCost.Text)
                    };

                    order.Deliveries.Add(delivery);

                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    ShowCustomMessageBox("Данные успешно сохранены!");
                }
                else
                {
                    ShowCustomMessageBox("Продукт или поставщик не найден");
                }
            }
            else
            {
                ShowCustomMessageBox("Пожалуйста, выберите все необходимые данные");
            }
        }
        private void ShowCustomMessageBox(string message)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(message);
            customMessageBox.ShowDialog();
        }

        private void CalculateTotalCost()
        {
            if (double.TryParse(TextBoxPrice.Text, out double price) &&
                double.TryParse(TextBoxSupplyPrice.Text, out double supplyPrice) &&
                double.TryParse(TextBoxQuantity.Text, out double quantity))
            {
                double totalCost = (price * quantity) + supplyPrice;
                TextBoxTotalCost.Text = totalCost.ToString();
            }
            else
            {
                TextBoxTotalCost.Text = "0";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateTotalCost();
        }
    }
}
