using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MilestoneOne.source;

namespace MilestoneOne
{
    /// <summary>
    ///     Interaction logic for CreateOrder_UserControl.xaml
    /// </summary>
    public partial class CreateOrder_UserControl : UserControl
    {
        private readonly DatabaseHelper _databaseHelper = new DatabaseHelper();
        private readonly List<ProductInOrder> _listProductsInOrder = new List<ProductInOrder>();
        private List<Product> _listProductsInChoosing;

        public CreateOrder_UserControl()
        {
            InitializeComponent();
            InitDataSource();

            ProductInOrderListView.IsEnabled = false;
            ButtonCancel.IsEnabled = false;
            ButtonCompleteOrder.IsEnabled = false;
            ButtonAddOrder.IsEnabled = true;
        }

        private void InitDataSource()
        {
            _listProductsInChoosing = _databaseHelper.GetProducts();
            ListViewProduct.ItemsSource = _listProductsInChoosing;
            ListViewType.ItemsSource = _databaseHelper.GetTypes();
        }

        private void Button_AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button)) return;
            var product = button.DataContext as Product;
            if (ProductInOrderListView.IsEnabled == false)
                MessageBox.Show("Please press Add Product button to create new order!");
            else
                AddProductToOrder(product);
        }

        private void AddProductToOrder(Product product)
        {
            if (IsExistProductInOrder(product))
            {
                var productInOrder = GetProductInOrder(product);
                productInOrder.Quantity = productInOrder.Quantity + 1;
            }
            else
            {
                var productInOrder = new ProductInOrder {
                    ID = product.Id,
                    Name = product.Name,
                    ProductId = product.Id
                };
                if (product.Cost != null) productInOrder.Cost = (double) product.Cost;
                _listProductsInOrder.Add(productInOrder);
                productInOrder.Quantity = productInOrder.Quantity + 1;
            }

            ProductInOrderListView.ItemsSource = null;
            ProductInOrderListView.ItemsSource = _listProductsInOrder;
        }

        private bool IsExistProductInOrder(Product product)
        {
            foreach (var t in _listProductsInOrder)
                if (t.ID == product.Id)
                    return true;

            return false;
        }

        private ProductInOrder GetProductInOrder(Product product)
        {
            return _listProductsInOrder.FirstOrDefault(t => product.Id == t.ID);
        }

        private void TypeItem_MouseLeftButtonClick(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is StackPanel stack)) return;
            var type = stack.DataContext as Type;
            if (type != null && type.IdType == 1)
                _listProductsInChoosing = _databaseHelper.GetProducts();
            else
                _listProductsInChoosing = _databaseHelper.GetProductsByType(type);
            ListViewProduct.ItemsSource = _listProductsInChoosing;
        }

        private void Button_addOrder_Click(object sender, RoutedEventArgs e)
        {
            ProductInOrderListView.IsEnabled = true;
            ButtonAddOrder.IsEnabled = false;
            ButtonCancel.IsEnabled = true;
            ButtonCompleteOrder.IsEnabled = true;
        }

        private Order CreateOrderSendToOrderList(IReadOnlyCollection<ProductInOrder> listProductsInOrder)
        {
            var order = new Order();
            double totalMoney = 0;
            const string orderStatus = "Mới tạo";
            var orderId = Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture);
            var _listProductAtOrder = new List<Product>();
            var totalOfProductQuantity = 0;

            foreach (var t in _listProductsInChoosing)
                foreach (var t1 in listProductsInOrder)
                    if (t.Name == t1.Name)
                    {
                        totalMoney += Convert.ToDouble(t.Cost) * t1.Quantity;
                        totalOfProductQuantity += t1.Quantity;
                        _listProductAtOrder.Add(t);
                    }

            order.TongTien = totalMoney;
            order.SoLuongSanPham = totalOfProductQuantity;
            order.tinhTrang = orderStatus;
            order.danhSachSanPhamTrongDonHang = _listProductAtOrder;
            order.idHoaDon = orderId;

            return order;
        }

        private void Button_CompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ProductInOrderListView.ItemsSource == null)
            {
                MessageBox.Show("CART IS EMPTY!!!!");
            }
            else
            {
                ProductInOrderListView.IsEnabled = false;
                ProductInOrderListView.ItemsSource = null;
                ButtonCompleteOrder.IsEnabled = false;
                ButtonCancel.IsEnabled = false;
                ButtonAddOrder.IsEnabled = true;
                // Create new order 
                int idOrder = _databaseHelper.CreateOrder();
                List<ProductInOrderTable> productInOrders = ProductInOrder.ConvertToProductInOrderTable(_listProductsInOrder, idOrder);
                // Add to list product in order to database after creating ordder
                this._databaseHelper.SaveListProductsInOrderTable(productInOrders);
                OrderList_UserControl.UpdateListOrder();
                _listProductsInOrder.Clear();
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            ProductInOrderListView.IsEnabled = false;
            ProductInOrderListView.ItemsSource = null;
            ButtonCancel.IsEnabled = false;
            ButtonCompleteOrder.IsEnabled = false;
            ButtonAddOrder.IsEnabled = true;
            _listProductsInOrder.Clear();
        }
    }
}