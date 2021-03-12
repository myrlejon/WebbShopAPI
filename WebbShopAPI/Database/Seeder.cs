using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebbShopAPI.Models;

namespace WebbShopAPI.Database
{
    public class Seeder
    {
        public static void Seed()
        {
            using (var db = new DatabaseContext())
            {
                if (db.Users.Count() == 0)
                {
                    db.Users.AddRange(new List<User>
                    {
                        new User { Name = "Administrator", Password = "CodicRulez", IsAdmin = true },
                        new User { Name = "TestCustomer", Password = "Codic2021", IsAdmin = false }
                    });
                    db.SaveChanges();
                }

                if (db.BookCategory.Count() == 0)
                {
                    db.BookCategory.AddRange(new List<BookCategory>
                    {
                        new BookCategory { Name = "Horror"},
                        new BookCategory { Name = "Science Fiction"}
                    });
                    db.SaveChanges();
                }

                if (db.Books.Count() == 0)
                {
                    db.Books.AddRange(new List<Book>
                    {
                        new Book { Title = "Cabal (Nightbreed)", Author = "Clive Barker", Price = 250, Amount = 3, CategoryID = 1},
                        new Book { Title = "The Shining", Author = "Stephen King", Price = 200, Amount = 2, CategoryID = 1},
                        new Book { Title = "Doctor Sleep", Author = "Stephen King", Price = 200, Amount = 1, CategoryID = 1},
                        new Book { Title = "I Robot", Author = "Isaac Asimov", Price = 150, Amount = 4, CategoryID = 2},
                    });
                    db.SaveChanges();
                }


                
            }
        }
    }
}
