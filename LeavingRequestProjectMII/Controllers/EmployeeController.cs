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
    public class EmployeeController : ControllerBase
    {
        public EmployeeRepository employeeRepository; 

        public EmployeeController (EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(employeeRepository.Get());
        }
    }

    
}
