using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn1.Models
{
    [Table("Role")]
    public class Role
    {
        public Role()
        {
           RoleAccounts = new HashSet<RoleAccount>();
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}
