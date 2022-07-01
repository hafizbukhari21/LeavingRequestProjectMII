using API.Models;
using API.ModelsInsert;
using API.ModelsResponse;
using API.Repositories.Data;
using API.Utils;
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

        [HttpGet("emp")]

        public ActionResult GetLeaveEmp(Employees employees)
        {
            return Ok(leavingRequestRepository.GetLeavingEmployee(employees));
        }
        [HttpGet("emp/detail")]
        public ActionResult GetLeaveEmp(LeavingRequest leavingRequest)
        {
            return Ok(leavingRequestRepository.GetLeavingEmployee(leavingRequest.request_id));
        }


        [HttpGet("man")]
        public ActionResult GetLeaveMananager(Employees employees)
        {
            return Ok(leavingRequestRepository.GetLeavingManager(employees));
        }

        [HttpPost]
        public ActionResult InsertLeavingRequest(LeavingRequestInserModel leavingRequestInser)
        {
            return Ok(leavingRequestRepository.InsertLeaving(leavingRequestInser));
        }

        [HttpPatch("approve")]
        public ActionResult ApproveRequest(LeavingRequest leaving)
        {
            string namaEmp = "";
            int ckhStatus = leavingRequestRepository.ApproveLeaving(leaving.request_id,leaving.approvalMessage, out namaEmp);
            if (ckhStatus > 0) return Ok(new GeneralResponse {ErrorType=Variables.SUCCESS, message="Berhasil Melakukan Approval Untuk "+ namaEmp  });
            else return BadRequest(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan Silahkan Coba Lagi" });
        }
        [HttpPatch("reject")]
        public ActionResult RejectRequest(LeavingRequest leaving)
        {
            string namaEmp = "";
            int ckhStatus = leavingRequestRepository.RejectLeaving(leaving.request_id, leaving.approvalMessage, out namaEmp);
            if (ckhStatus > 0) return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message = "Request Telah Direject Untuk " + namaEmp });
            else return BadRequest(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan Silahkan Coba Lagi" });
        }

        [HttpPatch("revisi")]
        public ActionResult RevisiRequest(LeavingRequest leaving)
        {
            string namaEmp = "";
            int ckhStatus = leavingRequestRepository.RevisiLeaving(leaving.request_id, leaving.approvalMessage, out namaEmp);
            if (ckhStatus > 0) return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message = "Request Telah Direvisi Untuk " + namaEmp });
            else return BadRequest(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan Silahkan Coba Lagi" });
        }

    }
}
