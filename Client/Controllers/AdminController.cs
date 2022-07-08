using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/")]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("Employees/")]
        public IActionResult Employees()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Divisions/")]
        public IActionResult Divisions()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("LeaveCategories/")]
        public IActionResult LeaveCategories()
        {
            return View();
        }
    }
}