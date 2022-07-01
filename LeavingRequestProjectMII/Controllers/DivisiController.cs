using API.Models;
using API.Repositories.Data;
using Microsoft.AspNetCore.Cors;
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
    public class DivisiController : ControllerBase
    {
        private readonly DivisiRepository divisiRepository;

        public DivisiController(DivisiRepository divisiRepository)
        {
            this.divisiRepository = divisiRepository;
        }

        [HttpGet]
        [EnableCors("AllowOrigin")]
        public ActionResult GetDivisi()
        {
            return Ok(divisiRepository.Get());
        }
    }
}
