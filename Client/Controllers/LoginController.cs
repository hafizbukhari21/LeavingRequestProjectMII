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
            HttpContext.Session.SetString("employee_id",token.employee_id);
<<<<<<< HEAD
            HttpContext.Session.SetString("name", token.name);
=======
            HttpContext.Session.SetString("role_name",token.name_role);
>>>>>>> f86179efc2f7dea4454d5302a0ad20516b5a7ee6
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
                if (HttpContext.User.IsInRole("Employee") || HttpContext.User.IsInRole("Manager"))
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

        [HttpGet("ForgotPassword/")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}