using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class LeaveCategory
    {

        public LeaveCategory()
        {
            leavingRequests = new HashSet<LeavingRequest>();
        }
        [Key]
        public int category_id { set; get; }
        public string nameCategory { set; get; }
        public virtual ICollection<LeavingRequest> leavingRequests { set; get; }
    }
}
