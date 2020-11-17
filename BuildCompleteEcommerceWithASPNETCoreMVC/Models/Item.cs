using DoAn1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Photo { get; set; }

        public int Quantity { get; set; }
    }
}
