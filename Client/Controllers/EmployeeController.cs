using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        [HttpGet("Employee/")]
        //[Authorize(Roles = "Employee, Manager")]
        public IActionResult Index()
        {
            return View();
        }
        //[Authorize]
        [HttpGet("LeaveRequest/")]
        public IActionResult LeaveRequest()
        {
            return View();
        }
        
        //[Authorize]
        [HttpGet("LeaveHistory/")]
        public IActionResult LeaveHistory()
        {
            return View();
        }

        //[Authorize(Roles = "Manager")]
        [HttpGet("LeaveRequestConfrimation/")]
        public IActionResult LeaveRequestConfirmation()
        {
            return View();
        }

    }
}