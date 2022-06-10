using Haley.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooSklad.ViewModel
{
    public class ProductViewModel : ChangeNotifier
    {
        private int id;
        private string article;
        private string title;
        private int amount;
        private string unit;
        private int wholesale;
        private int retail;
        private bool inStock;

        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged();
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                OnPropertyChanged();
            }
        }
        public int Wholesale
        {
            get { return wholesale; }
            set
            {
                wholesale = value;
                OnPropertyChanged();
            }
        }
        public int Retail
        {
            get { return retail; }
            set
            {
                retail = value;
                OnPropertyChanged();
            }
        }
        public bool InStock
        {
            get { return inStock; }
            set
            {
                inStock = value;
                OnPropertyChanged();
            }
        }



    }
}
