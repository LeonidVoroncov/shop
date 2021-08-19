using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Data.Models;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        AppDBContent _db;
        IWebHostEnvironment _appEnvironment; // Для добавления изображения в папку wwwroot

        public AdminController(AppDBContent db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
        }

        //--------------------добавление товара-------------------------------

        [HttpGet]
        public ViewResult AddProductForm()
        {
            var allCategory = new AllCategoriesViewModel
            {
                category = _db.Category.ToList()
            };

            ViewBag.Categories = new SelectList(allCategory.category, "id", "categiryName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductForm(Product product, IFormFile uploadedFile)
        {            
            if (uploadedFile != null)
            {                
                product.imgName = await SaveImage(uploadedFile);

                product.img = "/img/" + product.imgName;
            }
            else
                product.img = "/img/no-photo.png";

            _db.Product.Add(product);
            _db.SaveChanges();

            return RedirectToAction("AddProductForm");
        }

        //--------------------изменение товара-------------------------------

        [HttpGet]
        public IActionResult ChangeProductForm(int? id)
        {
            if (id != null)
            {
                var product = _db.Product.AsNoTracking().FirstOrDefault(p => p.Id == id);

                var allCategory = new AllCategoriesViewModel
                {
                    category = _db.Category.ToList()
                };
                ViewBag.Categories = new SelectList(allCategory.category, "id", "categiryName");

                if (product != null)
                    return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProductForm(Product product, IFormFile uploadedFile)
        {
            var productEntity = await _db.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == product.Id);

            if (uploadedFile != null)
            {
                if(uploadedFile.Name != "/img/no-photo.png")
                {
                    DeleteImage(productEntity.imgName);
                }                

                product.imgName = await SaveImage(uploadedFile);

                product.img = "/img/" + product.imgName;
            }
            else
            {
                product.imgName = productEntity.imgName;
                product.img = productEntity.img;
            }

            //_db.Entry(product).State = EntityState.Modified;

            _db.Product.Update(product);

            await _db.SaveChangesAsync();

            return LocalRedirect("~/Admin");
        }

        //--------------------удаление товара-------------------------------

        public IActionResult DelProduct(int id)
        {
            var prodDel = _db.Product.Find(id);

            if (prodDel.img != "/img/no-photo.png")
            {
                DeleteImage(prodDel.imgName);
            }

            _db.Product.Remove(prodDel);
            _db.SaveChanges();

            return LocalRedirect("~/Admin");
        }

        //--------------------Добавление изображения----------------------
        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_appEnvironment.WebRootPath,"img", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        //--------------------Удаление изображения----------------------

        [NonAction]
        public void DeleteImage(string imageName)
        {
            if(imageName != null)
            {
                string imagePath = Path.Combine(_appEnvironment.WebRootPath, "img", imageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }
        }

        //--------------------добавление категории-------------------------

        [HttpGet]
        public ViewResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category name)
        {
            _db.Category.Add(name);
            _db.SaveChanges();

            return LocalRedirect("~/Admin/AddCategory");
        }


        //--------------------Вывод всей продукции-------------------------------

        [HttpGet]
        public ViewResult Index()
        {
            var allProduct = new ProductListViewModel
            {
                product = _db.Product.ToList(),
                category = _db.Category.ToList()
            };

            return View(allProduct);
        }
    }
}
