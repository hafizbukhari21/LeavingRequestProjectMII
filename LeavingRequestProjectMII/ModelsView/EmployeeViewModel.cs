using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ModelsView
{
    public class EmployeeViewModel
    {
        public string employee_id { set; get; }
        public string name { set; get; }
        public string Email { set; get; }
        public string phoneNumber { set; get; }
        public int sisaCuti { set; get; }
        public string roleName { set; get; }
        public string namaDivisi { set; get; }

        public ICollection<Cuti> cuti { set; get; }
      
        
    }

    public class Cuti
    {
        public string approvalMessage { get; set; }
    }
}
