using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ModelsInsert
{
    public class LeavingRequestInserModel
    {
        
        public string employee_id { set; get; }
        public int category_id { set; get; }
        public DateTime requestTime { set; get; }
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }

        public string leavingMessage { set; get; }
    }
}
