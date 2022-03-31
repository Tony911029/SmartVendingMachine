using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartVendingMachine
{
    class Product
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public Product(string name, int number)
        {
            this.Number = number;
            this.Name = name;
        }
    }
}
