using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooSklad.Model
{
    public class Product
    {
        public int ID { get; set; }
        public string Article { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public string Unit { get; set; }
        public int Wholesale { get; set; }
        public int Retail { get; set; }
        public bool InStock { get; set; }
    }
}
