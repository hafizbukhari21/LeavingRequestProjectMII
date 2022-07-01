using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Divisi
    {
        
        public Divisi()
        {
            employees = new HashSet<Employees>();
        }
        [Key]
        public int divisi_id { set; get; }
        public string namaDivisi { set; get; }
        public bool isDeleted { set; get; }
        public virtual ICollection<Employees> employees { set; get; }

    }
}
