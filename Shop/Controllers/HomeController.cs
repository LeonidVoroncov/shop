using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Data.Models;
using Shop.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        AppDBContent db;

        public HomeController(AppDBContent context)
        {
            db = context;
        }

        public ViewResult Index()
        {
            var homeProduct = new HomeViewModel
            {
                product = db.Product.ToList()
            };

            return View(homeProduct);
        }
        
        [Route("ProductPage")]
        public IActionResult ProductPage(int id) //Вывод продукта на его личную страницу
        {
            var prod = db.Product.Find(id);//получаем объект продукта по id

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("admin"))
                {
                    ViewBag.button = "Добавить в корзину";
                }
                else
                {
                    var carts = db.Carts.Include(c => c.Products).ToList(); //получаем все корзины пользователей с товароми (склеиваем 2 таблицы)        

                    var product = db.Carts.FirstOrDefault(p => p.UserId == db.Users.FirstOrDefault(p => p.Email == User.Identity.Name).Id).Products; // получаем корзину пользователя

                    if (product.FirstOrDefault(p => p.Id == prod.Id) == null)
                    {
                        ViewBag.button = "Добавить в корзину";
                    }
                    else
                    {
                        ViewBag.button = "Удалить из корзины";
                    }
                }
                
            }  
            else
                ViewBag.button = "Добавить в корзину";

            return View(prod);
        }

        [Authorize]
        public IActionResult CartAddProduct(int id) //Добавление продукта в корзину
        {
            if (!User.IsInRole("admin"))
            {
                var userCarts = db.Carts.FirstOrDefault(p => p.UserId == db.Users.FirstOrDefault(p => p.Email == User.Identity.Name).Id); //получаем корзину пользователя

                var product = db.Product.Find(id); // берем продукт

                var Colapse = db.Carts.Include(c => c.Products).ToList(); //получаем все корзины пользователей с товароми (склеиваем 2 таблицы)        

                var cart = db.Carts.FirstOrDefault(p => p.UserId == db.Users.FirstOrDefault(p => p.Email == User.Identity.Name).Id).Products; // получаем продукты из корзины пользователя

                if (cart.FirstOrDefault(p => p.Id == product.Id) == null)
                    product.Carts.Add(userCarts); // добавляем товар в корзину
                else
                    product.Carts.Remove(userCarts); // Удаляем товар из корзины

                db.SaveChanges();
            }
            
            return RedirectToAction("ProductPage", new { id });

        }
    }
}