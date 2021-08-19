using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Models
{
    public class Product
    {
        public int Id { set; get; }

        public string name { set; get; }

        public string description { set; get; }

        public string img { set; get; }

        [NotMapped]
        public IFormFile imageFile { get; set; }

        public string imgName { get; set; }

        public ushort price { set; get; }

        public bool isFavourite { set; get; }

        public int categoryId { set; get; }

        public virtual Category Category { set; get; }


        public List<Cart> Carts { get; set; } = new List<Cart>();
    }
}
