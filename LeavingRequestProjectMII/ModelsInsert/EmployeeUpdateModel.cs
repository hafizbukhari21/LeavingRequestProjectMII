using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ModelsInsert
{
    public class EmployeeUpdateModel
    {
        public string employee_id { set; get; }
        public string name { set; get; }
        public string gender { set; get; }
        public string email { set; get; }
        public string phoneNumber { set; get; }
        public int role_Id { set; get; }
        public string manager_id { set; get; }
        public int divisi_id { set; get; }
       
    }
}
