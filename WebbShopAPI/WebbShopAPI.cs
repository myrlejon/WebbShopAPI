using System;
using System.Collections.Generic;
using System.Linq;
using WebbShopAPI.Database;
using WebbShopAPI.Models;

// Påminnelse, kom ihåg att allt ska vara public i denna klassen.
// Påminnelse, programmet behöver inte ha en Meny. Alla metoder körs i main.
// TODO: Gör om så att alla API metoder returnerar ett värde.

namespace WebbShopAPI
{
    public class WebbShopAPI
    {
        public int Login(string username, string password)
        {
            int ID = 0;
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Name == username);

                if (username == user.Name && password == user.Password && user.IsActive)
                {
                    user.SessionTimer = DateTime.Now;
                    DateTime newTime = user.SessionTimer.AddMinutes(15);
                    user.SessionTimer = newTime;
                    ID = user.ID;
                }
                db.SaveChanges();
            }
            return ID;
        }

        public bool UpdateSession(int ID)
        {
            using (var db = new DatabaseContext())
            {
                bool update = false;
                var user = db.Users.FirstOrDefault(u => u.ID == ID);

                if (DateTime.Now < user.SessionTimer)
                {
                    user.SessionTimer = DateTime.Now;
                    DateTime newTime = user.SessionTimer.AddMinutes(15);
                    user.SessionTimer = newTime;
                    update = true;
                }
                return update;
            }
        }

        public DateTime Logout(int ID)
        {
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.ID == ID);
                user.SessionTimer = DateTime.Now;
                db.SaveChanges();
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
                List<BookCategory> bookList = new List<BookCategory>();
                foreach (BookCategory book in db.BookCategory)
                {
                    if (book.Name.Contains(keyword))
                    {
                        bookList.Add(book);
                    }
                }
                return bookList;
            }
        }

        public List<Book> GetCategory(int ID)
        {
            using (var db = new DatabaseContext())
            {
                List<Book> bookList = new List<Book>();
                var category = db.BookCategory.FirstOrDefault(c => c.ID == ID);
                if (category != null)
                {
                    foreach (var book in db.Books)
                    {
                        if (book.CategoryID == category.ID)
                        {
                            bookList.Add(book);
                        }
                    }
                }
                return bookList;
            }
        }

        public List<Book> GetAvailableBooks(int ID)
        {
            using (var db = new DatabaseContext())
            {
                List<Book> bookList = new List<Book>();
                var category = db.BookCategory.FirstOrDefault(c => c.ID == ID);
                if (category != null)
                {
                    foreach (var book in db.Books)
                    {
                        if (book.Amount > 0 && book.CategoryID == ID)
                        {
                            bookList.Add(book);
                        }
                    }
                }
                return bookList;
            }
        }

        public Book GetBook(int ID)
        {
            using (var db = new DatabaseContext())
            {
                var book = db.Books.FirstOrDefault(b => b.ID == ID);
                return book;
            }
        }

        public List<Book> GetBooks(string keyword)
        {
            using (var db = new DatabaseContext())
            {
                List<Book> booklist = new List<Book>();
                foreach (Book book in db.Books)
                {
                    if (book.Title.Contains(keyword))
                    {
                        booklist.Add(book);
                    }
                }
                return booklist;
            }
        }

        public List<Book> GetAuthors(string keyword)
        {
            using (var db = new DatabaseContext())
            {
                List<Book> booklist = new List<Book>();
                foreach (Book book in db.Books)
                {
                    if (book.Author.Contains(keyword))
                    {
                        booklist.Add(book);
                    }
                }
                return booklist;
            }
        }

        public bool BuyBook(int userID, int bookID)
        {
            using (var db = new DatabaseContext())
            {
                {
                    bool sold = false;
                    var user = db.Users.FirstOrDefault(u => u.ID == userID);
                    var book = db.Books.FirstOrDefault(u => u.ID == bookID);

                    if (user != null && book != null && book.Amount > 0 && user.SessionTimer > DateTime.Now)
                    {
                        db.SoldBooks.Add(new SoldBooks
                        {
                            Title = book.Title,
                            Author = book.Author,
                            Price = book.Price,
                            PurchasedDate = user.SessionTimer,
                            CategoryId = book.CategoryID,
                            UserID = user.ID
                        });
                        book.Amount--;
                        sold = true;
                        db.SaveChanges();
                    }
                    return sold;
                }
            }
        }

        public string Ping(int ID)
        {
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.ID == ID);
                string pong = "";
                if (user != null)
                {
                    if (user.SessionTimer > DateTime.Now)
                    {
                        pong = "Pong";
                        user.SessionTimer = DateTime.Now;
                        DateTime newTime = user.SessionTimer.AddMinutes(15);
                        user.SessionTimer = newTime;
                    }
                }
                db.SaveChanges();
                return pong;
            }
        }

        public bool Register(string username, string password, string passwordVerify)
        {
            using (var db = new DatabaseContext())
            {
                bool create = false;
                var user = db.Users.FirstOrDefault(u => u.Name == username);
                if (user == null && password == passwordVerify)
                {
                    db.Users.Add(new User
                    {
                        Name = username,
                        Password = password,
                    });
                    db.SaveChanges();
                    create = true;
                }
                return create;
            }
        }

        public bool AddBook(int adminID, int bookID, string title, string author, int price, int amount)
        {
            using (var db = new DatabaseContext())
            {
                bool addBook = false;
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);
                var book = db.Books.FirstOrDefault(b => b.ID == bookID);

                if (book != null && user != null && user.IsAdmin == true && user.SessionTimer > DateTime.Now)
                {
                    book.Amount += amount;
                    addBook = true;
                    db.SaveChanges();
                }
                else if (book == null && user != null && user.IsAdmin == true && user.SessionTimer > DateTime.Now)
                {
                    db.Books.Add(new Book
                    {
                        Title = title,
                        Author = author,
                        Price = price,
                        Amount = amount
                    });
                    db.SaveChanges();
                    addBook = true;
                }
                return addBook;
            }
        }

        public bool SetAmount(int adminID, int bookID, int amount)
        {
            using (var db = new DatabaseContext())
            {
                bool newAmount = false;
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);
                var book = db.Books.FirstOrDefault(b => b.ID == bookID);

                if (book != null && user != null && user.IsAdmin == true && user.SessionTimer > DateTime.Now)
                {
                    book.Amount = amount;
                    newAmount = true;
                    db.SaveChanges();
                }
                return newAmount;
            }
        }

        public List<User> ListUsers(int adminID)
        {
            using (var db = new DatabaseContext())
            {
                List<User> userList = new List<User>();
                var admin = db.Users.FirstOrDefault(u => u.ID == adminID);
                if (admin != null && admin.IsAdmin == true && admin.SessionTimer > DateTime.Now)
                {
                    foreach (User user in db.Users)
                    {
                        userList.Add(user);
                    }
                }
                return userList;
            }
        }

        public List<User> FindUser(int adminID, string keyword)
        {
            using (var db = new DatabaseContext())
            {
                List<User> userList = new List<User>();
                var admin = db.Users.FirstOrDefault(u => u.ID == adminID);
                if (admin != null && admin.IsAdmin == true && admin.SessionTimer > DateTime.Now)
                {
                    foreach (User user in db.Users)
                    {
                        if (user.Name.Contains(keyword))
                        {
                            userList.Add(user);
                        }
                    }
                }
                return userList;
            }
        }

        public bool UpdateBook(int adminID, int bookID, string title, string author, int price)
        {
            using (var db = new DatabaseContext())
            {
                bool bookUpdate = false;
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);
                var book = db.Books.FirstOrDefault(b => b.ID == bookID);

                if (book != null && user != null && user.IsAdmin == true && user.SessionTimer > DateTime.Now)
                {
                    book.Title = title;
                    book.Author = author;
                    book.Price = price;
                    bookUpdate = true;
                    db.SaveChanges();
                }
                return bookUpdate;
            }
        }

        public bool DeleteBook(int adminID, int bookID)
        {
            using (var db = new DatabaseContext())
            {
                bool bookDelete = false;
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);
                var book = db.Books.FirstOrDefault(b => b.ID == bookID);

                if (book != null && user != null && user.IsAdmin == true && user.SessionTimer > DateTime.Now)
                {
                    bookDelete = true;
                    db.Books.Remove(book);
                    db.SaveChanges();
                }
                return bookDelete;
            }
        }

        public bool AddCategory(int adminID, string category)
        {
            using (var db = new DatabaseContext())
            {
                bool categoryAdd = false;
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);
                var book = db.BookCategory.FirstOrDefault(b => b.Name == category);

                if (book == null && user != null && user.IsAdmin == true && user.SessionTimer > DateTime.Now)
                {
                    db.BookCategory.Add(new BookCategory
                    {
                        Name = category
                    });
                    db.SaveChanges();
                    categoryAdd = true;
                }
                return categoryAdd;
            }
        }

        public bool AddBookToCategory(int adminID, int bookID, string category)
        {
            using (var db = new DatabaseContext())
            {
                bool bookAdd = false;
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);
                var book = db.Books.FirstOrDefault(b => b.ID == bookID);
                var bookCategory = db.BookCategory.FirstOrDefault(b => b.Name == category);

                if (book != null && user != null && bookCategory != null && user.IsAdmin == true && user.SessionTimer > DateTime.Now)
                {
                    book.CategoryID = bookCategory.ID;
                    bookAdd = true;
                    db.SaveChanges();
                }
                return bookAdd;
            }
        }

        public bool DeleteCategory(int adminID, int categoryID)
        {
            using (var db = new DatabaseContext())
            {
                bool categoryDelete = false;
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);
                var category = db.BookCategory.FirstOrDefault(b => b.ID == categoryID);

                if (category != null && user != null && user.IsAdmin && user.SessionTimer > DateTime.Now)
                {
                    db.BookCategory.Remove(category);
                    categoryDelete = true;
                    db.SaveChanges();
                }
                return categoryDelete;
            }
        }

        public bool AddUser(int adminID, string username, string password)
        {
            using (var db = new DatabaseContext())
            {
                bool addUser = false;
                var admin = db.Users.FirstOrDefault(u => u.ID == adminID);
                var user = db.Users.FirstOrDefault(u => u.Name == username);

                if (user == null && admin != null && String.IsNullOrEmpty(password) == false && admin.IsAdmin && admin.SessionTimer > DateTime.Now)
                {
                    db.Users.Add(new User
                    {
                        Name = username,
                        Password = password
                    });
                    db.SaveChanges();
                    addUser = true;
                }

                // Om lösenordet är tomt så sätts ett defaultlösenordet som är "CodicRulez", notera String.IsNullOrEmpty(password) i if satsen.
                else if (user == null && admin != null && String.IsNullOrEmpty(password) == true && admin.IsAdmin && admin.SessionTimer > DateTime.Now)
                {
                    db.Users.Add(new User
                    {
                        Name = username
                    });
                    db.SaveChanges();
                    addUser = true;
                }
                return addUser;
            }
        }

        public List<SoldBooks> SoldItems(int adminID)
        {
            using (var db = new DatabaseContext())
            {
                List<SoldBooks> soldBooks = new List<SoldBooks>();
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);

                if (user != null && user.IsAdmin && user.SessionTimer > DateTime.Now)
                {
                    foreach (var book in db.SoldBooks)
                    {
                        soldBooks.Add(book);
                    }
                }
                return soldBooks;
            }
        }

        public int MoneyEarned(int adminID)
        {
            using (var db = new DatabaseContext())
            {
                int sum = 0;
                List<SoldBooks> soldBooks = new List<SoldBooks>();
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);

                if (user != null && user.IsAdmin && user.SessionTimer > DateTime.Now)
                {
                    foreach (var book in db.SoldBooks)
                    {
                        sum += book.Price;
                    }
                }
                return sum;
            }
        }

        public void BestCustomer(int adminID)
        {
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.ID == adminID);
                List<SoldBooks> customerList = new List<SoldBooks>();

                if (user != null && user.IsAdmin && user.SessionTimer > DateTime.Now)
                {
                    var bestCustomer = db.SoldBooks.GroupBy(q => q.UserID).ToList(); //.OrderByDescending(gp => gp.Count()).Take(5).Select(g => g.Key).ToList();
                    Console.WriteLine(bestCustomer[1]);
                    //for (int i = 0; i < customerList.Count; i++)
                    //{
                    //    if (i == customerList[i].UserID)
                    //    {

                    //    }
                    //}
                }
            }
        }

        public bool Promote(int adminID, int userID)
        {
            using (var db = new DatabaseContext())
            {
                bool promote = false;
                var admin = db.Users.FirstOrDefault(u => u.ID == adminID);
                var user = db.Users.FirstOrDefault(u => u.ID == userID);

                if (user != null && admin != null && admin.IsAdmin && admin.SessionTimer > DateTime.Now)
                {
                    user.IsAdmin = true;
                    promote = true;
                    db.SaveChanges();
                }
                return promote;
            }
        }

        public bool Demote(int adminID, int userID)
        {
            using (var db = new DatabaseContext())
            {
                bool demote = false;
                var admin = db.Users.FirstOrDefault(u => u.ID == adminID);
                var user = db.Users.FirstOrDefault(u => u.ID == userID);

                if (user != null && admin != null && admin.IsAdmin && admin.SessionTimer > DateTime.Now)
                {
                    user.IsAdmin = false;
                    demote = true;
                    db.SaveChanges();
                }
                return demote;
            }
        }

        public bool ActivateUser(int adminID, int userID)
        {
            using (var db = new DatabaseContext())
            {
                bool activate = false;
                var admin = db.Users.FirstOrDefault(u => u.ID == adminID);
                var user = db.Users.FirstOrDefault(u => u.ID == userID);

                if (user != null && admin != null && admin.IsAdmin && admin.SessionTimer > DateTime.Now)
                {
                    user.IsActive = true;
                    activate = true;
                    db.SaveChanges();
                }
                return activate;
            }
        }

        public bool InactivateUser(int adminID, int userID)
        {
            using (var db = new DatabaseContext())
            {
                bool inactivate = false;
                var admin = db.Users.FirstOrDefault(u => u.ID == adminID);
                var user = db.Users.FirstOrDefault(u => u.ID == userID);

                if (user != null && admin != null && admin.IsAdmin && admin.SessionTimer > DateTime.Now)
                {
                    user.IsActive = false;
                    inactivate = true;
                    db.SaveChanges();
                }
                return inactivate;
            }
        }
    }
}