using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Employees
    {
        public Employees()
        {
            leavingRequests = new HashSet<LeavingRequest>();
        }

        [Key]
        public string employee_id { set; get; }
        public string name { set; get; }
        public Gender gender { set; get; }
        public string email { set; get; }
        public string password { set; get; }
        public string phoneNumber { set; get; }
        public int sisaCuti { set; get; }
        public int role_Id { set; get; }
        public int manager_id { set; get; }
        public int divisi_id { set; get; }
        public bool isDeleted { set; get; }

        public virtual Role role { set; get; }
        public virtual Divisi divisi { set; get; }
        public virtual  ICollection<LeavingRequest> leavingRequests { set; get; }




       




    }

    public enum Gender
    {
        Male,
        Female
    }


}
