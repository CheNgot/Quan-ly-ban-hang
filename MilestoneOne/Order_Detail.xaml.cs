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
using System.Windows.Shapes;
using MilestoneOne.source;

namespace MilestoneOne.order_screen
{
    /// <summary>
    /// Interaction logic for Order_Detail.xaml
    /// </summary>
    public partial class Order_Detail : Window
    {
        private BindingList<string> orderStatus = new BindingList<string>();
        public OrderTable newOrder;

        private void init()
        {
            orderStatus.Add("Mới tạo");
            orderStatus.Add("Hoàn tất");
            orderStatus.Add("Đã hủy");
            combobox_OrderStatus.ItemsSource = orderStatus;
            
            
        }

        public Order_Detail(OrderTable orderTable)
        {
            InitializeComponent();
            init();
            DataContext = orderTable;
            newOrder = orderTable;
            
            
            
            
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            newOrder.Status = combobox_OrderStatus.SelectedValue.ToString();

            this.Hide();
        }
    }
}
