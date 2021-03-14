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
        public int Login(string username, string password)
        {
            int ID = 0;
            using (var db = new DatabaseContext())
            {
                if (db.Users.Count() > 0)
                {
                    foreach (User user in db.Users)
                    {
                        if (username == user.Name && password == user.Password)
                        {
                            user.SessionTimer = DateTime.Now;
                            DateTime newTime = user.SessionTimer.AddMinutes(15);
                            user.SessionTimer = newTime;
                            ID = user.ID;
                        }
                    }
                }
                db.SaveChanges();
            }
            return ID;
        }

        public DateTime Logout(int ID)
        {
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.ID == ID);
                user.SessionTimer = DateTime.Now;
                return user.SessionTimer;
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

        public List<BookCategory> GetCategories(string keyword)
        {
            using (var db = new DatabaseContext())
            {
                List<BookCategory> booklist = new List<BookCategory>();
                foreach (BookCategory book in db.BookCategory)
                {
                    if (book.Name.Contains(keyword))
                    {
                        booklist.Add(book);
                    }
                }
                return booklist;
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

        public void GetAvailableBooks(int ID)
        {
            using (var db = new DatabaseContext())
            {
                var category = db.BookCategory.FirstOrDefault(c => c.ID == ID);
                if (category != null)
                {
                Console.WriteLine($"Listar upp alla böcker med kategorin {category.Name}.");
                    foreach (var book in db.Books)
                    {
                        if (book.Amount > 0 && book.CategoryID == ID)
                        {
                            Console.WriteLine($"{book.Title} - {book.Price}kr - Det finns {book.Amount} böcker kvar.");
                        }
                    }
                }
                else if (category == null)
                {
                    Console.WriteLine($"Det finns ingen kategori med kategorinummer {ID}.");
                }
            }
        }

        public void GetBook(int ID)
        {
            using (var db = new DatabaseContext())
            {
                var book = db.Books.FirstOrDefault(b => b.ID == ID);
                if (book != null)
                {
                    var category = db.BookCategory.FirstOrDefault(c => c.ID == book.CategoryID);
                    Console.WriteLine($"{book.Title} är en {category.Name} bok skriven av {book.Author}. Boken kostar {book.Price}kr och det finns {book.Amount} kvar i lagret.");
                }
                else if (book == null)
                {
                    Console.WriteLine($"Det finns ingen bok med ID-nummer {ID}.");
                }
            }
        }

        public void GetBooks(string keyword)
        {

        }
    }
}
