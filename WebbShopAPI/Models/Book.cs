using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebbShopAPI.Models
{
    public class Book
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int CategoryID { get; set; }
    }

    public class BookCategory
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class SoldBooks
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public DateTime PurchasedDate { get; set; }
        public int CategoryId { get; set; }
        public int UserID { get; set; }
    }
}
