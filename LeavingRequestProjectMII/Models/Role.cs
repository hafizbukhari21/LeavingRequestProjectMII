using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Role
    {
        public Role()
        {
            employees = new HashSet<Employees>();
        }
        [Key]
        public int role_id { set; get; }
        public string roleName { set; get; }

        public virtual ICollection<Employees> employees { set; get; }
    }
}
