using System;
using WebbShopAPI.Database;

namespace WebbShopAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Seeder.Seed();
        }
    }
}
