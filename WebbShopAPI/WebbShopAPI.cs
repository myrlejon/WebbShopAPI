using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebbShopAPI.Database;
using WebbShopAPI.Models;
using Microsoft.EntityFrameworkCore;

// Påminnelse, kom ihåg att allt ska vara public i denna klassen.
// Påminnelse, programmet behöver inte ha en Meny. Alla metoder körs i main.

namespace WebbShopAPI
{
    public class WebbShopAPI
    {
        public void Login()
        {
            using (var db = new DatabaseContext())
            {
                //Console.WriteLine("Username: ");
                //string userInput = Console.ReadLine();
                //if (db.Users.FirstOrDefault(u=>u.Name == userInput) != null)
                //{
                //    Console.WriteLine("Password: ");
                //    string passInput = Console.ReadLine();
                //    if (db.Users.FirstOrDefault(p=>p.Password == passInput= != null)
                //    {

                //    }

                //}
                //else if (db.Users.FirstOrDefault(u=>u.Name==userInput) == null)
                //{
                //    Console.WriteLine("Den användaren finns inte i registret.");
                //}
            }
        }

        public void GetCategories()
        {
            using (var db = new DatabaseContext())
            {
                foreach (var book in db.BookCategory)
                {
                    Console.WriteLine(book.Name);
                }
            }
        }

        public void GetCategories(string keyword)
        {
            using (var db = new DatabaseContext())
            {
                List<string> booklist = new List<string>();
                foreach (var book in db.BookCategory)
                {
                    booklist.Add(book.Name);
                }
                var contains = booklist.Contains(keyword);

                foreach (var book in booklist)
                {
                    contains = booklist.Contains(keyword);
                }

                if (contains)
                {
                    Console.WriteLine($"Kategorin {keyword} finns i databasen.");
                }

                else if (contains == false)
                {
                    Console.WriteLine($"Kategorin {keyword} finns inte i databasen.");
                }
            }
        }

        public void GetCategory(int ID)
        {
            using (var db = new DatabaseContext())
            {
                var category = db.BookCategory.FirstOrDefault(c => c.ID == ID);
                Console.WriteLine(category.Name);
            }

        }


    }
}
