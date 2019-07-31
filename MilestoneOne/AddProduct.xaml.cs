using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MilestoneOne.source;

namespace MilestoneOne
{
    /// <summary>
    ///     Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();
        private string stringNewPath;
        private string stringOldPath;
        private int check;

        public AddProduct()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            Cbtypename.ItemsSource = _database.GetTypesName();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_Product(object sender, RoutedEventArgs e)
        {
            if (check != 0 && Txtname.Text != "" && Txtamout.Text != "" && Txtcost.Text != "")
            {
                var product = new Product
                {
                    Name = Txtname.Text,
                    Cost = double.Parse(Txtcost.Text),
                    Amount = int.Parse(Txtamout.Text),
                    IdType = _database.GetIdType(Cbtypename.SelectedItem.ToString()),
                    Path = stringNewPath
                };
                _database.InsertProduct(product);
                if (AppHelper.CheckExistFile(product.Path))
                {
                    AppHelper.DeleteFile(product.Path);
                }
                AppHelper.CopyFile(stringOldPath, product.Path);

                Close();
            }
            else
            {
                MessageBox.Show("Not allow empty !!!!", "Warning!!!!");
            }
        }

        private void Pick_Image(object sender, RoutedEventArgs e)
        {
            var op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };

            if (op.ShowDialog() == true)
            {
                stringOldPath = op.FileName;
                AppHelper.CreateImageFolderIfNotExist();
                var stringNameFile = _database.GetLastIdProduct() + Path.GetExtension(op.FileName);
                stringNewPath = stringNameFile;

                // show ản preview bằng đường dẫn của file gốc 
                Img.Source = new BitmapImage(new Uri(Path.GetFullPath(stringOldPath)));
                check = 1;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "." && IsNumber(e.Text) == false) e.Handled = true;
            else if (e.Text == ".")
            {
                if (((TextBox) sender).Text.IndexOf(e.Text, StringComparison.Ordinal) > -1) e.Handled = true;
            }
        }

        private bool IsNumber(string text)
        {
            return int.TryParse(text, out _);
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}