using Microsoft.EntityFrameworkCore;
using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class AppDBContent : DbContext
    {        
        public DbSet<Product> Product { get; set; } //регистрирует таблицу товаров

        public DbSet<Category> Category { get; set; } //регистрирует таблицу категорий

        public DbSet<User> Users { get; set; } //регистрирует таблицу пользователей 

        public DbSet<Role> Roles { get; set; } //регистрирует таблицу ролей для пользователей

        public DbSet<Cart> Carts { get; set; } //регистрирует таблицу корзины для пользователей

        public AppDBContent(DbContextOptions<AppDBContent> options) : base(options)//установление параметров подключения БД
        {
            Database.EnsureCreated();            
        }
    }
}
