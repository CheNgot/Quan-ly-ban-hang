using MilestoneOne.source;
using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MilestoneOne
{
    /// <summary>
    /// Interaction logic for AddTypes.xaml
    /// </summary>
    public partial class AddTypes : Window
    {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        public AddTypes()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void AddNewType(object sender, RoutedEventArgs e)
        {   
            if(Txttypename.Text!="")
            {
                string typename = Txttypename.Text;
                databaseHelper.InsertType(typename);
                Close();
            }
            else
            {
                MessageBox.Show("Not allow empty !!!!", "Warning!!!!");
            }
           
        }
    }
}
