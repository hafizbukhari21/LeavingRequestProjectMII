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
        //[Authorize(Roles = "Employee")]
        [HttpGet("Employee/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("LeaveRequest/")]
        public IActionResult LeaveRequest()
        {
            return View();
        }

        [HttpGet("LeaveHistory/")]
        public IActionResult LeaveHistory()
        {
            return View();
        }

    }
}