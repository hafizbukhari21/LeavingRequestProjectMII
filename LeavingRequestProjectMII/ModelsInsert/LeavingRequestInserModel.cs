using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ModelsInsert
{
    public class LeavingRequestInserModel
    {
        public string request_id { get; set; }
        public string employee_id { set; get; }
        public int category_id { set; get; }
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }

        public Byte[] fileBukti { get; set; }

        public string namaFileBukti { get; set; }

        public string tipeFileBukti { get; set; }


        public string leavingMessage { set; get; }
    }
}
