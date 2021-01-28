using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }

        private static int ID;

        public Product() 
        {
            Id = ++ID;
        }

        public Product(string name, string description, string category, decimal price, int amount)
        {
            Name = name;
            Description = description;
            Category = category;
            Price = price;
            Amount = amount;
            Id = ++ID;
        }

        static Product() // статический конструктор, инициализирующий статические данные класса
        {
            ID = 0;
        }
    }
}
