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

        [HttpGet("{divisi_id}")]
        [EnableCors("AllowOrigin")]
        public ActionResult GetDivisi(int divisi_id)
        {
            return Ok(divisiRepository.GetId(divisi_id));
        }

        [HttpPatch]
        [EnableCors("AllowOrigin")]
        public ActionResult Update(Divisi divisi)
        {
             return Ok(divisiRepository.Update(divisi));

        }

        [HttpDelete]
        [EnableCors("AllowOrigin")]
        public ActionResult DeleteEmployee(Divisi divisi)
        {
            return Ok(divisiRepository.softDelete(divisi.divisi_id));
        }

        [HttpPost]
        [EnableCors("AllowOrigin")]
        public ActionResult Insert(Divisi divisi)
        {
            return Ok(divisiRepository.Insert(divisi));
        }
    }
}
