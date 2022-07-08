using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class LeavingRequest
    {
        [Key]
        public string request_id { set; get; }
        public string employee_id { set; get; }
        public int category_id { set; get; }
        public Approval_status approvalStatus { set; get; }
        public DateTime requestTime { set; get; }
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }

        public string leavingMessage { set; get; }

        public string approvalMessage { set; get; }

        public Byte[] fileBukti { get; set; }

        public string namaFileBukti { get; set; }

        public bool isRead { get; set; }

        public bool isDelete { get; set; }

        public TipeFileBukti tipeFileBukti { get; set; }

        public virtual Employees employees { set; get; }
        public virtual LeaveCategory leaveCategory { set; get; }


    }

    public enum TipeFileBukti
    {
        none,
        jpg,
        pdf,
        png,
        docx,
        doc,
        tiff,
        ppt,
        pptx,
        xlsx,
        xls,
        htm,
        mp3,
        mp4,
        txt,
        
    }

    public enum Approval_status
    {
      Menunggu, Diterima, Ditolak, Revisi   
    }
}
