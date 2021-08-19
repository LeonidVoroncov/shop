using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class AllCategoriesViewModel
    {
        public IEnumerable<Category> category { get; set; }
    }
}
