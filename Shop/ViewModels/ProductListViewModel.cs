using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> product { get; set; }
        public IEnumerable<Category> category { get; set; }
    }
}
