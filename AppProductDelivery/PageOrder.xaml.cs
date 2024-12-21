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
using System.Windows.Threading;
using DocumentFormat.OpenXml.Packaging;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;


namespace AppProductDelivery
{
    /// <summary>
    /// Логика взаимодействия для PageOrder.xaml
    /// </summary>
    public partial class PageOrder : Page
    {
        private ProductDeliveryEntities _context;
        private DispatcherTimer _timer;

        public PageOrder()
        {
            InitializeComponent();
            _context = new ProductDeliveryEntities();
            LoadData();
            InitializeTimer();
            Date.Text = DateTime.Now.ToString("g");
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); 
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Date.Text = DateTime.Now.ToString("g");
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

            string date = Date.Text;
            string products = ComboBoxProducts.Text;
            string category = ComboBoxCategory.Text;
            string unit = ComboBoxUnit.Text;
            string price = TextBoxPrice.Text;
            string suppliers = ComboBoxSupplier.Text;
            string supplyPrice = TextBoxSupplyPrice.Text;
            string quantity = TextBoxQuantity.Text;
            string totalCost = TextBoxTotalCost.Text;

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Word Documents (*.docx)|*.docx",
                Title = "Сохранить документ",
                FileName = "Document.docx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                CreateWordDocument(filePath, date, products, category, unit, price, suppliers, supplyPrice, quantity, totalCost);
            }
        }

        private void CreateWordDocument(string filePath, string date, string products, string category, string unit, string price, string suppliers, string supplyPrice, string quantity, string totalCost)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                AddTitle(body, "Накладная");
                AddParagraph(body, "Дата: " + date);

                AddTable(body, new string[] { "Продукт", "Категория", "Единица", "Цена", "Поставщик", "Цена поставки", "Количество", "Общая стоимость" },
                 new string[][] { new string[] { products, category, unit, price, suppliers, supplyPrice, quantity, totalCost } });

                mainPart.Document.Save();
            }

            ShowCustomMessageBox("Документ успешно сохранен!");
        }

        private void AddTitle(Body body, string text)
        {
            DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new Text(text)
            {
                Space = SpaceProcessingModeValues.Preserve
            })
            {
                RunProperties = new RunProperties(
                    new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" },
                    new FontSize() { Val = "36" },
                    new DocumentFormat.OpenXml.Wordprocessing.Bold(),
                    new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "000000" } // Черный цвет текста
                )
            })
            {
                ParagraphProperties = new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center }
                )
            };
            body.AppendChild(paragraph);
        }

        private void AddParagraph(Body body, string text)
        {
            DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new Text(text)
            {
                Space = SpaceProcessingModeValues.Preserve
            })
            {
                RunProperties = new RunProperties(
                    new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" },
                    new FontSize() { Val = "26" },
                    new DocumentFormat.OpenXml.Wordprocessing.Bold(),
                    new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "000000" } // Черный цвет текста
                )
            })
            {
                ParagraphProperties = new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center }
                )
            };
            body.AppendChild(paragraph);
        }

        private void AddTable(Body body, string[] headers, string[][] rows)
        {
            DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();

            TableProperties tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                ),
                new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Dxa }
            );

            table.AppendChild(tableProperties);

            DocumentFormat.OpenXml.Wordprocessing.TableRow headerRow = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
            foreach (var header in headers)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableCell cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new Text(header)
                {
                    Space = SpaceProcessingModeValues.Preserve
                })
                {
                    RunProperties = new RunProperties(
                        new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" },
                        new FontSize() { Val = "20" },
                        new DocumentFormat.OpenXml.Wordprocessing.Bold()
                    )
                }));
                headerRow.AppendChild(cell);
            }
            table.AppendChild(headerRow);

            foreach (var row in rows)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableRow dataRow = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                foreach (var cellText in row)
                {
                    DocumentFormat.OpenXml.Wordprocessing.TableCell cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new Text(cellText)
                    {
                        Space = SpaceProcessingModeValues.Preserve
                    })
                    {
                        RunProperties = new RunProperties(
                            new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" },
                            new FontSize() { Val = "16" }
                        )
                    }));
                    dataRow.AppendChild(cell);
                }
                table.AppendChild(dataRow);
            }

            body.AppendChild(table);
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

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowed(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c) && c != ',' && c != '.')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
