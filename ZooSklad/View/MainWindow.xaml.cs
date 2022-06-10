using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZooSklad.Model;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using ZooSklad.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZooSklad.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVewModel();
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem == null)
                MessageBox.Show("Выберите одну строку!", "Внимание!");
            else
                return;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem == null)
                MessageBox.Show("Выберите одну строку!", "Внимание!");
            else
                return;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (checkNone.IsChecked == true)
                checkMin.IsChecked = false;
        }

        private void checkMin_Checked(object sender, RoutedEventArgs e)
        {
            if (checkMin.IsChecked == true)
                checkNone.IsChecked = false;
        }
    }
}
