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

        [HttpGet("emp/{employee_id}/notif")]
        [EnableCors("AllowOrigin")]
        public ActionResult GetEmployeeNotif(string employee_id)
        {
            return Ok(leavingRequestRepository.GetNotifikasiEmployee(employee_id));
        }

        [HttpGet("emp/{request_id}/isRead")]
        [EnableCors("AllowOrigin")]
        public ActionResult SetIsReadRequest(string request_id)
        {
            Tuple<int,LeavingRequest> chk = leavingRequestRepository.IsReadNotif(request_id);
            if (chk.Item1 == Variables.SUCCESS) return Ok(new { ErrorType = Variables.SUCCESS, request_id = chk.Item2.request_id });
            else return Ok(new { ErrorType = Variables.FAIL});
        }


        [HttpGet("man/{manager_id}/statistic")]
        [EnableCors("AllowOrigin")]
        public ActionResult Statistic(string manager_id)
        {
            return Ok(leavingRequestRepository.CountStatisticManager(manager_id));
        }

        [HttpGet("emp/{employee_id}/statistic")]
        [EnableCors("AllowOrigin")]
        public ActionResult StatisticEmp(string employee_id)
        {
            return Ok(leavingRequestRepository.CountStatisticEmployee(employee_id));
        }

        [HttpGet("man/{manager_id}/calendar")]
        [EnableCors("AllowOrigin")]
        public ActionResult GetCutiDateEmp(string manager_id)
        {
            return Ok(leavingRequestRepository.GetCutiEmployeeForManager(manager_id));
        }

        [HttpGet("countNotRead/{employee_id}")]
        [EnableCors("AllowOrigin")]

        public int CountIsNotReadRequest(string employee_id)
        {
            return leavingRequestRepository.CountNotReadRequest(employee_id);
        }

        [HttpGet("emp/{employee_id}")]
        [EnableCors("AllowOrigin")]


        public ActionResult GetLeaveEmp(string employee_id)
        {
            return Ok(leavingRequestRepository.GetLeavingEmployee(employee_id));
        }
        [HttpPost("emp/detail")]
        [EnableCors("AllowOrigin")]

        public ActionResult GetLeaveEmp(LeavingRequest leavingRequest)
        {
            return Ok(leavingRequestRepository.GetLeavingEmployeeDetail(leavingRequest.request_id));
        }

        [HttpPost("emp/detail/download")]
        [EnableCors("AllowOrigin")]

        public ActionResult GetLeaveEmpDownload(LeavingRequest leavingRequest)
        {
            return Ok(leavingRequestRepository.DownloadFileBukti(leavingRequest.request_id));
        }


        [HttpGet("man/{manager_id}")]
        [EnableCors("AllowOrigin")]

        public ActionResult GetLeaveMananager(string manager_id)
        {
            return Ok(leavingRequestRepository.GetLeavingManager(manager_id));
        }

        [HttpPost]
        [EnableCors("AllowOrigin")]

        public async Task<ActionResult> InsertLeavingRequest(LeavingRequestInserModel leavingRequestInser)
        {
            int chk = await leavingRequestRepository.InsertLeaving(leavingRequestInser);

            if (chk == Variables.CUTI_SUDAH_HABIS)
                return Ok(new GeneralResponse { ErrorType = Variables.CUTI_SUDAH_HABIS, message = "Jatah Cuti tidak mencukupi" });
            else if(chk == Variables.SYARAT_MIN_TANGGAL_REQUEST)
                return Ok(new GeneralResponse { ErrorType = Variables.CUTI_SUDAH_HABIS, message = "Cuti Harus diajukan H-7 Sebelumnya" });
            else
                return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message = "Berhasil Mengajukan Cuti" });

        }

       

        [HttpPatch]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UpdateLeavingRequest(LeavingRequestInserModel leaving)
        {
            int chk = await leavingRequestRepository.UpdateLeaving(leaving);
            switch (chk)
            {
                case Variables.SUCCESS:
                    return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message="Berhasil Mengupdate data" });
                case Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI:
                    return Ok(new GeneralResponse { ErrorType = Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI, message = "Jumlah Cuti anda tidak mencukupi" });
                default:
                    return Ok(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi kesalahan silakhkan coba lagi nanti" });
            }

        }



        [HttpPatch("man/approve")]
        [EnableCors("AllowOrigin")]

        public async Task<ActionResult> ApproveRequest(LeavingRequest leaving)
        {
            
            Tuple<int,string> ckhStatus = await leavingRequestRepository.ApproveLeaving(leaving.request_id,leaving.approvalMessage);
            if (ckhStatus.Item1 == Variables.SUCCESS) return Ok(new GeneralResponse {ErrorType=Variables.SUCCESS, message="Berhasil Melakukan Approval Untuk "+ ckhStatus.Item2  });
            if (ckhStatus.Item1 == Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI) return Ok(new GeneralResponse {ErrorType=Variables.JUMLAH_CUTI_TIDAK_MENCUKUPI, message="Sisa hari cuti tidak memenuhi "+ ckhStatus.Item2 });
            if (ckhStatus.Item1 == Variables.MANAGER_DILARANG_APPROVE_CUTI) return Ok(new GeneralResponse {ErrorType=Variables.MANAGER_DILARANG_APPROVE_CUTI, message="Approval Cuti Tidak bisa dilakukan tanggal 1 Januari " });
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
