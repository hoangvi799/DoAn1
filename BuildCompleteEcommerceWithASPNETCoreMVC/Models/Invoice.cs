using DoAn1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Models
{
    [Table("Invoice")]
    public class Invoice
    {
        public Invoice()
        {
            InvoiceDetailses = new HashSet<InvoiceDetails>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public int Status { get; set; }

        public int AccountId { get; set; }


        public virtual Account Account { get; set; }

        public virtual ICollection<InvoiceDetails> InvoiceDetailses { get; set; }
    }
}
