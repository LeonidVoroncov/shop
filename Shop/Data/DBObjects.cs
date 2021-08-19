using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class DBObjects
    {
        public static void Initial(AppDBContent content)
        {
            if (!content.Roles.Any())
                content.Roles.AddRange(Role.Select(c => c.Value));

            if (!content.Users.Any())//Если база данных пустая значит заполняем ее
            {
                content.AddRange(
                    new User
                    {
                        Email = "admin",
                        Password = "admin",
                        RoleId = 2
                    }
                );
            }

            content.SaveChanges(); //Сохраняем данные 
        }

        private static Dictionary<string, Role> role;

        public static Dictionary<string, Role> Role
        {
            get
            {
                if (role == null) //Если таблица категорий пустая, то заполняем ее
                {
                    var list = new Role[]
                    {
                        new Role { Name = "user"},
                        new Role { Name = "admin"}
                    };

                    role = new Dictionary<string, Role>();
                    foreach (Role el in list)
                        role.Add(el.Name, el);  // Заполняем таблицу категорий, но это не точно

                }

                return role;
            }
        }
    }
}
