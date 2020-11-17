using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn1.Models
{
    [Table("Photo")]
    public partial class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool Featured { get; set; }
        public int ProductId { get; set; }


        public virtual Product Product { get; set; }

    }
}
