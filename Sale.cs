using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore
{
    public class Sale: Product
    {
        public DateTime SaleDate { get; set; }

        public static int ID;

        static Sale() // статический конструктор инициализирует статические данные класса
        {
            ID = 0;
        }

        // Конструктор при наследовании содержит : base(params), params - параметры конструктора базового класса
        public Sale(string name, string description, string category, decimal price, int amount, DateTime saleDate):
            base(name, description, category, price, amount)
        {
            Id = ++ID;
            SaleDate = saleDate;
        }
    }
}
