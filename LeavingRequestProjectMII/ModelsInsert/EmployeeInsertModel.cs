using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ModelsInsert
{
    public class EmployeeInsertModel
    {
        public string name { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phoneNumber { get; set; }
        public int role_id { get; set; }
        public int divisi_id { get; set; }
        public string manager { get; set; }

    }
}
