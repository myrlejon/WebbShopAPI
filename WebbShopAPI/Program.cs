using System;
using WebbShopAPI.Database;
using System.Linq;

namespace WebbShopAPI
{
    class Program : WebbShopAPI
    {
        static void Main()
        {
            var API = new WebbShopAPI();
            Seeder.Seed();
            API.GetCategory(2);
        }
    }
}
