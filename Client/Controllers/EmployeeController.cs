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
        [Authorize(Roles = "Employee, Manager")]
        [HttpGet("Employee/")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Employee, Manager")]
        [HttpGet("LeaveRequest/")]
        public IActionResult LeaveRequest()
        {
            return View();
        }

        [Authorize(Roles = "Employee, Manager")]
        [HttpGet("LeaveHistory/")]
        public IActionResult LeaveHistory()
        {
            return View();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("LeaveRequestConfrimation/")]
        public IActionResult LeaveRequestConfirmation()
        {
            return View();
        }

    }
}