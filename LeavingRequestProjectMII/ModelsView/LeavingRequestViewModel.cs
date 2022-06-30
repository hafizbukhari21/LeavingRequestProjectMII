using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ModelsView
{
    public class LeavingRequestViewModel
    {
        public string request_id { set; get; }
        public string employee_id { set; get; }
        public string employeeName { set; get; }
        public string categoryName { set; get; }
        public string approvalStatus { set; get; }
        public DateTime requestTime { set; get; }
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }
        public string approvalMessage { set; get; }

    }
}
