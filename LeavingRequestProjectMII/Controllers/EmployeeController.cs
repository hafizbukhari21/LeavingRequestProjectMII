using API.Controllers.Base;
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
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employees, EmployeeRepository, string>
    {
        public EmployeeRepository employeeRepository; 

        public EmployeeController (EmployeeRepository employeeRepository) :base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(employeeRepository.Get());
        }
        [HttpPost]
        public ActionResult Insert(EmployeeInsertModel employeeInsert)
        {
            if (employeeRepository.EmailIsUsed(employeeInsert.email)) 
                return BadRequest(new GeneralResponse { ErrorType=Variables.EMAIL_DUPLICATE, message="Email Telah Digunakan" }) ;

            if (employeeRepository.PhoneIsUsed(employeeInsert.phoneNumber))
                return BadRequest(new GeneralResponse { ErrorType =Variables.NO_TELP_DUPLICATE, message = "Nomor Telp Telah Digunakan" });

            if(employeeRepository.Insert(employeeInsert)>0)
                return Ok(new GeneralResponse { ErrorType = Variables.SUCCESS, message = "Berhasil Menambahkan Data" });
            else
                return BadRequest(new GeneralResponse { ErrorType = Variables.FAIL, message = "Terjadi Kesalahan mohon coba beberapa saat lagi" });
        }   
    }

    
}
