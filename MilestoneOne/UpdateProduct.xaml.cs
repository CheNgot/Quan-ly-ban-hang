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
    ///     Interaction logic for UpdateProduct.xaml
    /// </summary>
    public partial class UpdateProduct : Window
    {
        public static Product _data;
        private readonly Product temp;
        private readonly DatabaseHelper _database = new DatabaseHelper();
        private int check;
        private string stringNewPath;
        private string Curpath;

        public UpdateProduct(Product product)
        {
            InitializeComponent();
            Cbidloai.ItemsSource = _database.GetTypesName();
            Txtname.Text = product.Name;
            Txtcost.Text = product.Cost.ToString();
            Txtamout.Text = product.Amount.ToString();
            Cbidloai.Text = _database.GetTypesNameById((product.IdType).Value).ToString();
            Img.Source = product.Image;
            temp = product;
            _data = product;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "." && IsNumber(e.Text) == false) e.Handled = true;
            else if (e.Text == ".")
            {
                if (((TextBox)sender).Text.IndexOf(e.Text, StringComparison.Ordinal) > -1) e.Handled = true;
            }
        }
        private bool IsNumber(string text)
        {
            return int.TryParse(text, out _);
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (Txtname.Text != "" && Txtamout.Text != "" && Txtcost.Text != "")
            {
                var product = new Product
                {
                    Id = temp.Id,
                    Name = Txtname.Text,
                    Cost = double.Parse(Txtcost.Text),
                    Amount = int.Parse(Txtamout.Text),
                    IdType = _database.GetIdType(Cbidloai.SelectedItem.ToString())
            };
                if (check == 0)
                {
                    product.Path = temp.Path;
                }
                else
                {
                    product.Path = stringNewPath;
                    if (AppHelper.CheckExistFile(stringNewPath))
                    {
                        AppHelper.DeleteFile(stringNewPath);  
                    }
                    AppHelper.CopyFile(Curpath, stringNewPath);
                }

                _data = product;
                Close();
            }
            else
            {
                MessageBox.Show("Không được để trống ô!!", "Thông báo!!!");
            }
        }

        private void Picking(object sender, RoutedEventArgs e)
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
                var stringNameFile = temp.Id + Path.GetExtension(op.FileName);
                stringNewPath = stringNameFile;
                Curpath = op.FileName;
                Img.Source = new BitmapImage(new Uri(Path.GetFullPath(op.FileName)));
                check = 1;
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}