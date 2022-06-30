using API.Models;
using API.Repositories.Data;
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

        [HttpGet("man")]
        public ActionResult GetLeaveMananager(Employees employees)
        {
            return Ok(leavingRequestRepository.GetLeavingManager(employees));
        }
    }
}
