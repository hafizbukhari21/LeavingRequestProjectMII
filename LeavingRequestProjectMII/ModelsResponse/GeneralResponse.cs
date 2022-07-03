using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ModelsResponse
{
    public class GeneralResponse
    {
        public int ErrorType { set; get; }
        public string message { set; get; }

    }

    public class LoginResponse
    {
        public int ErrorType { set; get; }
        public string message { set; get; }
        public string token { set; get; }
        public string name { set; get; }
        public string employee_id { set; get; }
        public string name_role { set; get; }
    }
}
