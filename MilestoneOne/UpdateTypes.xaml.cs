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
    /// Interaction logic for UpdateTypes.xaml
    /// </summary>
    public partial class UpdateTypes : Window

    {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        public static Type temp;
        public UpdateTypes(Type type)
        {   
            InitializeComponent();
            Txttypename.Text = type.TypeName;
            temp = type;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateType(object sender, RoutedEventArgs e)
        {
            if (Txttypename.Text != "")
            {
                temp.TypeName = Txttypename.Text;

                Close();
            }
            else
            {
                MessageBox.Show("Not allow empty !!!!", "Warning!!!!");
            }
        }
    }
}
