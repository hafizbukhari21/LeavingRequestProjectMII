using API.Utils;
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
    public class LiburController : ControllerBase
    {
        private readonly NationalDayServices nationalDay;

        public LiburController(NationalDayServices nationalDay)
        {
            this.nationalDay = nationalDay;
        }

        [HttpGet]
        public async Task<ActionResult> GetLibur()
        {
             return Ok(await nationalDay.GetNationalDay());
        }

        [HttpGet("tot")]
        public async Task<ActionResult> GetTotalhari()
        {
            return Ok(await nationalDay.CountPotonganLibur());
        }
    }
}
