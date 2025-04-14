using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.ViewModels.Product
{
    public class ProductsListViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int  ProductPrice { get; set; }
        public int Count { get; set; }
        public int TotalPrice { get; set; }
    }
}
