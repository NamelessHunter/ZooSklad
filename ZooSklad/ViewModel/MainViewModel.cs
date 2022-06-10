using Haley.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZooSklad.Model;
using ZooSklad.View;

namespace ZooSklad.ViewModel
{
    internal class MainVewModel : ChangeNotifier
    {
        private List<ProductViewModel> AllList;
        private List<ProductViewModel> FullList;

        private ObservableCollection<ProductViewModel> demo_list;
        public ObservableCollection<ProductViewModel> DemoList
        {
            get { return demo_list; }
            set
            {
                demo_list = value;
                OnPropertyChanged();
            }
        }
        private string textFilter;
        public string TextFilter
        {
            get { return textFilter; }
            set
            {
                textFilter = value;
                OnPropertyChanged();
            }
        }
        private bool noneAmount { get; set; }
        public bool NoneAmount
        {
            get { return noneAmount; }
            set
            {
                noneAmount = value;
                OnPropertyChanged();
            }
        }
        private bool minAmount { get; set; }
        public bool MinAmount
        {
            get { return minAmount; }
            set
            {
                minAmount = value;
                OnPropertyChanged();
            }
        }

        public MainVewModel()
        {
            AccessData();
        }
        private void AccessData()
        {
            FullList = new List<ProductViewModel>();
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "select * from [Warehouse] ";
            cmd.Connection = con;
            OleDbDataReader rd = cmd.ExecuteReader();
            List<Product> products = new List<Product>();
            while (rd.Read())
            {
                Product product = new Product();
                product.ID = (int)rd["ID"];
                product.Article = (string)rd["Article"];
                product.Title = (string)rd["Title"];
                product.Amount = (int)rd["Amount"];
                product.Unit = (string)rd["Unit"];
                product.Wholesale = (int)rd["Wholesale"];
                product.Retail = (int)rd["Retail"];
                if (product.Amount > 0)
                    product.InStock = true;
                else
                    product.InStock = false;
                products.Add(product);
            }
            foreach (var item in products)
            {
                ProductViewModel ourProduct = new ProductViewModel();
                ourProduct.ID = item.ID;
                ourProduct.Article = item.Article;
                ourProduct.Title = item.Title;
                ourProduct.Amount = item.Amount;
                ourProduct.Unit = item.Unit;
                ourProduct.Wholesale = item.Wholesale;
                ourProduct.Retail = item.Retail;
                ourProduct.InStock = item.InStock;
                FullList.Add(ourProduct);
            }
            DemoList = new ObservableCollection<ProductViewModel>(FullList.OrderBy(p => p.Title).ToList());
            AllList = new List<ProductViewModel>(FullList);
            con.Close();
        }
        #region Command

        private RelayCommand filterText;
        public RelayCommand FilterText
        {
            get
            {
                return filterText ??
                  (filterText = new RelayCommand(obj =>
                  {
                      if (TextFilter.Length != 0)
                      {
                          FullList = AllList.Where(p => p.Title.StartsWith(TextFilter)).ToList();
                          DemoList = new ObservableCollection<ProductViewModel>(FullList.OrderBy(p => p.Title).ToList());
                      }
                      else
                      {
                          AccessData();
                      }
                  }));
            }
        }
        private RelayCommand noneAmountCommand;
        public RelayCommand NoneAmountCommand
        {
            get
            {
                return noneAmountCommand ??
                  (noneAmountCommand = new RelayCommand(obj =>
                  {
                      if (noneAmount == true)
                      {
                          FullList = AllList.Where(p => p.Amount.Equals(0)).ToList();
                          DemoList = new ObservableCollection<ProductViewModel>(FullList.OrderBy(p => p.Title).ToList());
                      }
                      else if (noneAmount == false)
                      {
                          AccessData();
                      }
                  }));
            }
        }
        private RelayCommand minAmountCommand;
        public RelayCommand MinAmountCommand
        {
            get
            {
                return minAmountCommand ??
                  (minAmountCommand = new RelayCommand(obj =>
                  {
                      if (minAmount == true)
                      {
                          AccessData();
                          FullList = AllList.Where(p => p.Amount <= 3 && p.Unit.Equals("шт") || p.Amount <= 1 && p.Unit.Equals("кг") || p.Amount <= 300 && p.Unit.Equals("г") || p.Amount <= 1 && p.Unit.Equals("упак")).ToList();
                          DemoList = new ObservableCollection<ProductViewModel>(FullList.OrderBy(p => p.Title).ToList());
                      }
                      else if (minAmount == false)
                      {
                          AccessData();
                      }
                  }));
            }
        }
        private RelayCommand add;
        public RelayCommand Add
        {
            get
            {
                return add ??
                    (add = new RelayCommand(obj =>
                    {
                        AddEditView addEditView = new AddEditView(0,null,null,0,null,0,0);
                        if (addEditView.ShowDialog() == true)
                        {
                            ProductViewModel item = addEditView.Product;
                            ProductViewModel ourProduct = new ProductViewModel();
                            OleDbConnection con = new OleDbConnection();
                            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
                            con.Open();
                            ourProduct.Article = item.Article;
                            ourProduct.Title = item.Title;
                            ourProduct.Amount = item.Amount;
                            ourProduct.Unit = item.Unit;
                            ourProduct.Wholesale = item.Wholesale;
                            ourProduct.Retail = item.Retail;
                            ourProduct.InStock = item.InStock;
                            if (ourProduct.Amount > 0)
                                ourProduct.InStock = true;
                            else
                                ourProduct.InStock = false;
                            OleDbCommand comand = new OleDbCommand();
                            comand.Connection = con;
                            comand.CommandType = CommandType.Text;
                            comand.CommandText = "INSERT INTO Warehouse (Article,Title,Amount,Unit,Wholesale,Retail) VALUES (?,?,?,?,?,?)";
                            comand.Parameters.AddWithValue("@Article", ourProduct.Article);
                            comand.Parameters.AddWithValue("@Title", ourProduct.Title);
                            comand.Parameters.AddWithValue("@Amount", ourProduct.Amount);
                            comand.Parameters.AddWithValue("@Unit", ourProduct.Unit);
                            comand.Parameters.AddWithValue("@Wholesale", ourProduct.Wholesale);
                            comand.Parameters.AddWithValue("@Retail", ourProduct.Retail);

                            if (comand.ExecuteNonQuery() != 1)
                                MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                            else
                                MessageBox.Show("Данные добавлены!", "Внимание!");
                            OleDbCommand cmd = new OleDbCommand();
                            cmd.CommandText = "SELECT TOP 1 * FROM [Warehouse] ORDER BY ID DESC";
                            cmd.Connection = con;
                            OleDbDataReader rd = cmd.ExecuteReader();
                            while (rd.Read())
                            {
                                ourProduct.ID = (int)rd["ID" ];
                            }
                            con.Close();
                            AccessData();
                        }
                    }));
            }
        }
        private ProductViewModel selectedItem;
        public ProductViewModel SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }
        private RelayCommand edit;
        public RelayCommand Edit
        {
            get
            {
                return edit ??
                    (edit = new RelayCommand(obj =>
                    {
                        ProductViewModel selectedProduct = new ProductViewModel();
                        selectedProduct = selectedItem;
                        OleDbConnection con = new OleDbConnection();
                        con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
                        con.Open();
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandText = "select * from [Warehouse] where [ID] = " + (selectedProduct.ID);
                        cmd.Connection = con;
                        OleDbDataReader rd = cmd.ExecuteReader();
                        ProductViewModel product = new ProductViewModel();
                        while (rd.Read())
                        {
                            product.ID = (int)rd["ID"];
                            product.Article = (string)rd["Article"];
                            product.Title = (string)rd["Title"];
                            product.Amount = (int)rd["Amount"];
                            product.Unit = (string)rd["Unit"];
                            product.Wholesale = (int)rd["Wholesale"];
                            product.Retail = (int)rd["Retail"];
                        }
                        AddEditView addEditView = new AddEditView(product.ID, product.Article, product.Title, product.Amount, product.Unit, product.Wholesale, product.Retail);
                    if (addEditView.ShowDialog() == true)
                    {
                            ProductViewModel item = addEditView.Product;
                            ProductViewModel ourProduct = new ProductViewModel();
                            ourProduct.ID = product.ID;
                            ourProduct.Article = item.Article;
                            ourProduct.Title = item.Title;
                            ourProduct.Amount = item.Amount;
                            ourProduct.Unit = item.Unit;
                            ourProduct.Wholesale = item.Wholesale;
                            ourProduct.Retail = item.Retail;
                            ourProduct.InStock = item.InStock;
                            if (ourProduct.Amount > 0)
                                ourProduct.InStock = true;
                            else
                                ourProduct.InStock = false;
                            OleDbCommand comand = new OleDbCommand();
                            comand.Connection = con;
                            comand.CommandType = CommandType.Text;
                            comand.CommandText = "UPDATE Warehouse SET Article = ?,Title = ?,Amount = ?,Unit = ?,Wholesale = ?,Retail = ? WHERE ID = ?";
                            comand.Parameters.AddWithValue("@Article", ourProduct.Article);
                            comand.Parameters.AddWithValue("@Title", ourProduct.Title);
                            comand.Parameters.AddWithValue("@Amount", ourProduct.Amount);
                            comand.Parameters.AddWithValue("@Unit", ourProduct.Unit);
                            comand.Parameters.AddWithValue("@Wholesale", ourProduct.Wholesale);
                            comand.Parameters.AddWithValue("@Retail", ourProduct.Retail);
                            comand.Parameters.AddWithValue("@ID", product.ID);
                            try
                            {
                                if (comand.ExecuteNonQuery() >= 1)
                                    MessageBox.Show("Данные изменены!", "Внимание!");
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                            }
                            con.Close();
                            AccessData();
                        }
                    }));

            }
        }
        private RelayCommand delete;
        public RelayCommand Delete
        {
            get
            {
                return delete ??
                    (delete = new RelayCommand(obj =>
                    {
                        string messageBoxText = "Вы уверены что хотите удалить этот продукт?";
                        string caption = "Удаление";
                        MessageBoxButton button = MessageBoxButton.YesNo;
                        MessageBoxImage icon = MessageBoxImage.Question;
                        
                        MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:

                                ProductViewModel selectedProduct = new ProductViewModel();
                                selectedProduct = selectedItem;
                                OleDbConnection con = new OleDbConnection();
                                con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
                                con.Open();
                                OleDbCommand comand = new OleDbCommand();
                                comand.Connection = con;
                                comand.CommandType = CommandType.Text;
                                comand.CommandText = "DELETE FROM Warehouse WHERE ID = " + selectedProduct.ID;
                                try
                                {
                                    if (comand.ExecuteNonQuery() >= 1)
                                        MessageBox.Show("Данные удалены!", "Внимание!");
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                                }
                                AccessData();

                                break;

                            case MessageBoxResult.No:
                                break;
                        }
                    }));
            }
        }

        private RelayCommand suppOrder;
        public RelayCommand SuppOrder
        {
            get
            {
                return suppOrder ??
                    (suppOrder = new RelayCommand(obj =>
                    {
                        
                        
                    }));
            }
        }




        #endregion
    }
}
