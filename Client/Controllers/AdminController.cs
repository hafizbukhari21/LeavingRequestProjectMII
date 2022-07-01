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
        [HttpGet("Employees/")]
        public IActionResult Employees()
        {
            return View();
        }
        [HttpGet("Divisions/")]
        public IActionResult Divisions()
        {
            return View();
        }
        [HttpGet("LeaveCategories/")]
        public IActionResult LeaveCategories()
        {
            return View();
        }
    }
}