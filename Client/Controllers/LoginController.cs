using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers.Base;
using API.Models;
using API.ModelsInsert;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class LoginController : Controller
    {

        private readonly LoginRepository repository;
        public LoginController(LoginRepository repository) 
        {
            this.repository = repository;
        }

        [HttpPost]
        public async Task<JsonResult> Auth(EmployeeLoginModel employeeLoginModel)
        {
            var jwtToken = await repository.Auth(employeeLoginModel);
            var token = jwtToken;

            if (token == null)
            {
                return Json(jwtToken);
            }

            HttpContext.Session.SetString("JWToken", token.token);
            return Json(jwtToken);
        }
        [HttpGet("login/")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("JWToken") != null)
            {
                if (HttpContext.User.IsInRole("Admin"))
                {
                    return Redirect("Admin/");
                }
                if (HttpContext.User.IsInRole("Employee"))
                {
                    return Redirect("Employee/");
                }
                else
                {
                    return Redirect("/");
                }
            }
            return View();
        }

        [HttpGet("Logout/")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}