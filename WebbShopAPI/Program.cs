using System;
using WebbShopAPI.Database;
using System.Linq;
using WebbShopAPI.Models;

namespace WebbShopAPI
{
    class Program : WebbShopAPI
    {
        static void Main()
        {
            var API = new WebbShopAPI();
            Seeder.Seed();
            var test = API.Login("test", "secret");
            //API.InactivateUser(1, 3);
            Console.WriteLine(test);
        }
    }
}
