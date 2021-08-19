using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Models
{
    public class Category
    {
        public int id { set; get; }

        public string categiryName { set; get; }

        public List<Product> product { set; get; }
    }
}
