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
            var register = API.Register("test", "secret", "secret");
            Console.WriteLine(register);
        }
    }
}
