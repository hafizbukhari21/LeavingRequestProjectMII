using API.Models;
using API.ModelsInsert;
using API.ModelsResponse;
using API.Repositories.Data;
using API.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavingRequestController : ControllerBase
    {
        public LeavingRequestRepository leavingRequestRepository;

        public LeavingRequestController(LeavingRequestRepository leavingRequestRepository)
        {
            this.leavingRequestRepository = leavingRequestRepository;
        }

        [HttpGet("countNotRead/{employee_id}")]
        [EnableCors("AllowOrigin")]

        public int CountIsNotReadRequest(string employee_id)
        {
            return leavingRequestRepository.CountNotReadRequest(employee_id);
        }

        [HttpGet("emp")]
        [EnableCors("AllowOrigin")]


        public ActionResult GetLeaveEmp(Employees employees)
        {
            return Ok(leavingRequestRepository.GetLeavingEmployee(employees));
        }
        [HttpGet("emp/detail")]
        [EnableCors("AllowOrigin")]

        public ActionResult GetLeaveEmp(LeavingRequest leavingRequest)
        {
            return Ok(leavingRequestRepository.GetLeavingEmployee(leavingRequest.request_id));
        }

        [HttpGet("emp/detail/download")]
        [EnableCors("AllowOrigin")]

        public ActionResult GetLeaveEmpDownload(LeavingRequest leavingRequest)
        {
            return Ok(leavingRequestRepository.DownloadFileBukti(leavingRequest.request_id));
        }


        [HttpGet("man")]
        [EnableCors("AllowOrigin")]

        public ActionResult GetLeaveMananager(Employees employees)
        {
            return Ok(leavingRequestRepository.GetLeavingManager(employees));
        }

        [HttpPost]
        [EnableCors("AllowOrigin")]

        public ActionResult InsertLeavingRequest(LeavingRequestInserModel leavingRequestInser)
        {
            int chk = leavingRequestRepository.InsertLeaving(leavingRequestInser);

            if (chk == Variables.CUTI_SUDAH_HABIS)
                return Ok(new GeneralResponse { ErrorType = Variables.CUTI_SUDAH_HABIS, message = "Jatah Cuti tidak mencukupi" });
            else if(chk == Variables.SYARAT_MIN_TANGGAL_REQUEST)
                return Ok(new GeneralResponse { ErrorType = Variables.CUTI_SUDAH_HABIS, message = "Cuti Harus diajukan H-7 Sebelumnya" });
            else
                return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message = "Berhasil Mengajukan Cuti" });

        }

       

        [HttpPatch]
        [EnableCors("AllowOrigin")]
        public ActionResult UpdateLeavingRequest(LeavingRequestInserModel leaving)
        {
            return Ok(leavingRequestRepository.UpdateLeaving(leaving));
        }



        [HttpPatch("man/approve")]
        [EnableCors("AllowOrigin")]

        public ActionResult ApproveRequest(LeavingRequest leaving)
        {
            string namaEmp = "";
            int ckhStatus = leavingRequestRepository.ApproveLeaving(leaving.request_id,leaving.approvalMessage, out namaEmp);
            if (ckhStatus == Variables.SUCCESS) return Ok(new GeneralResponse {ErrorType=Variables.SUCCESS, message="Berhasil Melakukan Approval Untuk "+ namaEmp  });
            if (ckhStatus == Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI) return Ok(new GeneralResponse {ErrorType=Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI, message="Sisa hari cuti tidak memenuhi "+ namaEmp  });
            else return BadRequest(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan Silahkan Coba Lagi" });
        }


        [HttpPatch("man/reject")]
        [EnableCors("AllowOrigin")]

        public ActionResult RejectRequest(LeavingRequest leaving)
        {
            string namaEmp = "";
            int ckhStatus = leavingRequestRepository.RejectLeaving(leaving.request_id, leaving.approvalMessage, out namaEmp);
            if (ckhStatus > 0) return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message = "Request Telah Direject Untuk " + namaEmp });
            else return BadRequest(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan Silahkan Coba Lagi" });
        }


        [HttpPatch("man/revisi")]
        [EnableCors("AllowOrigin")]

        public ActionResult RevisiRequest(LeavingRequest leaving)
        {
            string namaEmp = "";
            int ckhStatus = leavingRequestRepository.RevisiLeaving(leaving.request_id, leaving.approvalMessage, out namaEmp);
            if (ckhStatus > 0) return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message = "Request Telah Direvisi Untuk " + namaEmp });
            else return BadRequest(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan Silahkan Coba Lagi" });
        }

    }
}
