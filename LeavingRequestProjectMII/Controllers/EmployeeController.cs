using API.Controllers.Base;
using API.Models;
using API.ModelsInsert;
using API.ModelsResponse;
using API.Repositories.Data;
using API.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employees, EmployeeRepository, string>
    {
        public EmployeeRepository employeeRepository;
        public IConfiguration configuration;

        public EmployeeController(EmployeeRepository employeeRepository, IConfiguration configuration) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            this.configuration = configuration;
        }

        [HttpGet("allManager")]
        [EnableCors("AllowOrigin")]
        public ActionResult GetAllManager()
        {
            return Ok(employeeRepository.GetAllManager());
        }

        [HttpGet]
        [EnableCors("AllowOrigin")]
        public ActionResult Get()
        {
            return Ok(employeeRepository.Get());
        }

        [HttpGet("{employee_id}")]
        [EnableCors("AllowOrigin")]
        public ActionResult Get(string employee_id)
        {
            return Ok(employeeRepository.Get(employee_id));
        }

        [HttpPost]
        [EnableCors("AllowOrigin")]
        public ActionResult Insert(EmployeeInsertModel employeeInsert)
        {
            if (employeeRepository.EmailIsUsed(employeeInsert.email)) 
                return Ok(new GeneralResponse { ErrorType=Variables.EMAIL_DUPLICATE, message="Email Telah Digunakan" }) ;

            if (employeeRepository.PhoneIsUsed(employeeInsert.phoneNumber))
                return Ok(new GeneralResponse { ErrorType =Variables.NO_TELP_DUPLICATE, message = "Nomor Telp Telah Digunakan" });

            if(employeeRepository.Insert(employeeInsert)>0)
                return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message = "Berhasil Menambahkan Data" });
            else
                return Ok(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan mohon coba beberapa saat lagi" });
        }

        [HttpPost("login")]
        [EnableCors("AllowOrigin")]

        public ActionResult Login(EmployeeLoginModel employeeLogin)
        {
            Employees empReturn = new Employees();
            int checkStatus = employeeRepository.Login(employeeLogin, out empReturn);


            if (checkStatus==Variables.SUCCESS)
            {
                string token = ApplyJwt.GetJwt(empReturn, configuration);
                return Ok(new LoginResponse { ErrorType = Variables.SUCCESS, message = "Berhasil Login", token = token, name = empReturn.name, employee_id = empReturn.employee_id });
            }
            else if(checkStatus == Variables.WRONG_EMAIL)
            {
                return BadRequest(new LoginResponse { ErrorType=Variables.WRONG_EMAIL, message="Email atau akun tidak ditemukan", token="", name="" });
            }
            else if (checkStatus == Variables.WRONG_PASSWORD)
            {
                return BadRequest(new LoginResponse { ErrorType = Variables.WRONG_PASSWORD, message = "Salah Memasukan Password", token = "", name = "" });
            }
            else
            {
                return BadRequest(new LoginResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan Silahkan Coba Lagi", token = "", name = "" });
            }
        }

        [HttpPatch]
        [EnableCors("AllowOrigin")]
        public ActionResult Update(EmployeeUpdateModel employeeUpdate)
        {
            int checkStatus = employeeRepository.Update(employeeUpdate);

            if (checkStatus == Variables.SUCCESS) 
                return Ok(new GeneralResponse { ErrorType=Variables.SUCCESS,  message="Berhasil Update"});
            
            else if(checkStatus == Variables.EMAIL_DUPLICATE)
                return BadRequest(new GeneralResponse { ErrorType = Variables.EMAIL_DUPLICATE, message = "Email Telah Digunakan" });

            else if (checkStatus == Variables.NO_TELP_DUPLICATE)
                return BadRequest(new GeneralResponse { ErrorType = Variables.NO_TELP_DUPLICATE, message = "Nomor Telp Telah Digunakan" });

            else return BadRequest(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan Dalam sistem" });

        }

        [HttpDelete]
        [EnableCors("AllowOrigin")]
        public ActionResult DeleteEmployee(Employees employees)
        {
            return Ok(employeeRepository.softDelete(employees.employee_id));
        }





    }

    
}
