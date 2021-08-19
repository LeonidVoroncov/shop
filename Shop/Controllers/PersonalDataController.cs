using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{

    [Authorize]
    public class PersonalDataController : Controller
    {
        private AppDBContent db;

        public PersonalDataController(AppDBContent content)
        {
            db = content;
        }

        public IActionResult Cart() // вывод продуктов
        {            
            var user = db.Users.FirstOrDefault(p => p.Email == User.Identity.Name); //получаем объект пользователя

            var carts = db.Carts.Include(c => c.Products).ToList(); //получаем все корзины пользователей с товароми (склеиваем 2 таблицы)        

            var productList = db.Carts.FirstOrDefault(p => p.UserId == user.Id).Products;

            var SumPrice = 0;

            foreach (var prod in productList)
            {
                SumPrice = SumPrice + prod.price;
            }

            ViewBag.SumPrice = SumPrice;

            var homeProduct = new HomeViewModel
            {
                product = db.Carts.FirstOrDefault(p => p.UserId == user.Id).Products // получаем корзину пользователя
            };

            return View(homeProduct);
        }

        public IActionResult CartProdDel(int id)
        {
            var Colapse = db.Carts.Include(c => c.Products).ToList(); //получаем все корзины пользователей с товароми (склеиваем 2 таблицы)  

            var userCarts = db.Carts.FirstOrDefault(p => p.UserId == db.Users.FirstOrDefault(p => p.Email == User.Identity.Name).Id); //получаем корзину пользователя

            var product = db.Product.Find(id);

            product.Carts.Remove(userCarts); // Удаляем товар из корзины

            db.SaveChanges();

            return RedirectToAction("Cart");
        }
        
    }
}
