using DoAn1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Models
{
    [Table("InvoiceDetails")]
    public class InvoiceDetails
    {
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Product Product { get; set; }
    }
}
