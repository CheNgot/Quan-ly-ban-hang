using MilestoneOne.source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MilestoneOne.order_screen;

namespace MilestoneOne
{
    public partial class OrderList_UserControl : UserControl
    {
        private static readonly DatabaseHelper _databaseHelper = new DatabaseHelper();
        public static ItemsControl ItemsControl;
        public static Order orderReceive;

        int _currentPage = 1;
        int _itemsPerPage = 4;
        int _totalPages;

        public static void UpdateListOrder()
        {
            ItemsControl.ItemsSource = null;
            ItemsControl.ItemsSource = _databaseHelper.GetOrderTables();
        }

        public OrderList_UserControl()
        {
            InitializeComponent();
            ItemControlListOrder.ItemsSource = _databaseHelper.GetOrderTables();
            XuLyPhanTrang();
            ItemsControl = ItemControlListOrder;
        }

        private void XuLyPhanTrang()
        {
            List<OrderTable> productInOrderTables = _databaseHelper.GetOrderTables();
            _totalPages = productInOrderTables.Count / _itemsPerPage;
            if ((productInOrderTables.Count % _itemsPerPage) > 0)
                _totalPages += 1;
            ItemControlListOrder.ItemsSource = productInOrderTables
                .Skip((_currentPage - 1) * _itemsPerPage)
                .Take(_itemsPerPage);
            LabelPageNumber.Content = $"{_currentPage}";
        }

        private void Button_Previous_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                ItemControlListOrder.ItemsSource = _databaseHelper.GetOrderTables()
                    .Skip((_currentPage - 1) * _itemsPerPage)
                    .Take(_itemsPerPage);
                LabelPageNumber.Content = $"{_currentPage}";
            }
        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                ItemControlListOrder.ItemsSource = _databaseHelper.GetOrderTables()
                    .Skip((_currentPage - 1) * _itemsPerPage)
                    .Take(_itemsPerPage);
                LabelPageNumber.Content = $"{_currentPage}";
            }
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is StackPanel stack)
            {
                var info = stack.DataContext as OrderTable;
                var screen = new Order_Detail(_databaseHelper.GetOrderTable(info.Id));
                screen.ShowDialog();
            }

            ItemControlListOrder.ItemsSource = _databaseHelper.GetOrderTables();
        }

        public void LoadPage()
        {
            ItemControlListOrder.ItemsSource = _databaseHelper.GetOrderTables();
        }

        private void btn_DelOrder_click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button)) return;
            OrderTable order = button.DataContext as OrderTable;
            int idOrder = order.Id;
            _databaseHelper.DelOrder(idOrder);
            LoadPage();
        }
    }
}
