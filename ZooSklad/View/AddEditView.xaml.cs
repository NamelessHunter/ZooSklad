using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Text.RegularExpressions;
using ZooSklad.Model;
using ZooSklad.ViewModel;

namespace ZooSklad.View
{
    /// <summary>
    /// Логика взаимодействия для AddEditView.xaml
    /// </summary>
    public partial class AddEditView : Window, INotifyPropertyChanging
    {
        public ProductViewModel Product { get; set; }
        private List<string> unitsList;
        public List<string> UnitsList
        {
            get { return unitsList; }
            set
            {
                unitsList = value;
                OnPropertyChanged("UnitsList");
            }
        }

        public AddEditView(int id, string article, string title, int amount, string unit, int wholesale, int retail)
        {
            InitializeComponent();

            Product = new ProductViewModel();
            Product.ID = id;
            Product.Article = article;
            Product.Title = title;
            Product.Amount = amount;
            Product.Unit = unit;
            Product.Wholesale = wholesale;
            Product.Retail = retail;
            DataContext = Product;

            UnitsList = new List<string>() { "шт","г","кг","упак"};
            ComboUnit.ItemsSource = unitsList;
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangingEventHandler PropertyChanging;

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnRand_Click(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < 10; i++)
                s = String.Concat(s, random.Next(10).ToString());
            TextBoxArticle.Text = s;
        }
    }
}
