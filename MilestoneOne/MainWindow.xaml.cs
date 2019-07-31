using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using MilestoneOne.source;
using MilestoneOne.charts_screen;

namespace MilestoneOne
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ExcelHelper _excelHelper = new ExcelHelper();
        private readonly DatabaseHelper _databaseHelper = new DatabaseHelper();
        private Type _currentType;
        

        public MainWindow()
        {
            InitializeComponent();
            InitDataSource();
            _currentType = new Type
            {
                TypeName = "All",
                IdType = 1
            };
        }

        private void InitDataSource()
        {
            ListView.ItemsSource = _databaseHelper.GetProducts();
            ListViewType.ItemsSource = _databaseHelper.GetTypes();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is StackPanel stack)
            {
                var info = stack.DataContext as Product;
                var screen = new UpdateProduct(info);
                screen.ShowDialog();
            }

            _databaseHelper.UpdateProduct(UpdateProduct._data);
            ListView.ItemsSource = _databaseHelper.GetProducts();
        }

        private void ButtonImportExcelClick(object sender, RoutedEventArgs e)
        {
            var theDialog = new OpenFileDialog {Title = "Open Excel File", Filter = "Excel files|*.xlsx"};

            try
            {
                if (theDialog.ShowDialog() == true)
                {
                    var path = theDialog.InitialDirectory + theDialog.FileName;
                    var products = _excelHelper.ReadFileExcel(path);
                    _databaseHelper.InsertListProducts(products);
                    ListView.ItemsSource = _currentType.IdType == 1 ? _databaseHelper.GetProducts() : _databaseHelper.GetProductsByType(_currentType);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot read that file: " + ex.Message);
            }
        }

        private void TypeItemListView_MouseLeftClick(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is StackPanel stack)) return;
            var type = stack.DataContext as Type;
            if (type != null && type.IdType == 1) ListView.ItemsSource = _databaseHelper.GetProducts();
            else ListView.ItemsSource = _databaseHelper.GetProductsByType(type);
            _currentType = type;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListViewType.MaxWidth = Width - ButtonAddType.Width;
        }

        private void ButtonAddProductClick(object sender, RoutedEventArgs e)
        {
            var addProduct = new AddProduct();
            addProduct.ShowDialog();
            ListView.ItemsSource = _databaseHelper.GetProducts();
        }

        private void ButtonAddTypeClick(object sender, RoutedEventArgs e)
        {
            var addTypes = new AddTypes();
            addTypes.ShowDialog();
            ListViewType.ItemsSource = _databaseHelper.GetTypes();
        }

        private void updateTypeMenuitem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menu)
            {
                var info = menu.DataContext as Type;
                var screen = new UpdateTypes(info);
                screen.ShowDialog();
            }

            var name = UpdateTypes.temp.TypeName;
            var id = UpdateTypes.temp.IdType;
            _databaseHelper.UpdateType(id, name);
            ListViewType.ItemsSource = _databaseHelper.GetTypes();
        }

        private void deleteTypeMenuitem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menu)) return;
            var info = menu.DataContext as Type;

            var products = _databaseHelper.GetProductsByType(info);
            var count = 0;
            foreach (var unused in products) count++;

            if (count != 0)
            {
                MessageBox.Show("Vẫn còn dữ liệu không được xóa", "Thông báo");
            }
            else
            {
                if (info != null) _databaseHelper.DeleteType(info.IdType);
                ListViewType.ItemsSource = _databaseHelper.GetTypes();
            }
        }

        private void deleteProductMenuitem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menu)) return;
            if (menu.DataContext is Product info) _databaseHelper.DeleteProduct(info.Id);
            if (_currentType == null || _currentType.IdType == 1) ListView.ItemsSource = _databaseHelper.GetProducts();
            else ListView.ItemsSource = _databaseHelper.GetProductsByType(_currentType);
        }

        private void ButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            Order_Screen screen = new Order_Screen();
            screen.ShowDialog();
        }

        private void ButtonCharts_Click(object sender, RoutedEventArgs e)
        {
            mainScreenOfCharts screen = new mainScreenOfCharts();
            screen.ShowDialog();
        }
    }
}