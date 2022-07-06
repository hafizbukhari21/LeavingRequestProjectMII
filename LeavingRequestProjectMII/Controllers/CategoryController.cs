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
    public class CategoryController : ControllerBase
    {
        public readonly CategoryRepository categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        [EnableCors("AllowOrigin")]

        public ActionResult GetCategory()
        {
            return Ok(categoryRepository.GetCategory());
        }

        [HttpGet("{category_id}")]
        [EnableCors("AllowOrigin")]
        public ActionResult GetCategory(int category_id)
        {
            return Ok(categoryRepository.GetId(category_id));
        }

        [HttpPatch]
        [EnableCors("AllowOrigin")]
        public ActionResult Update(LeaveCategory category)
        {
            return Ok(categoryRepository.Update(category));

        }

        [HttpPost]
        [EnableCors("AllowOrigin")]
        public ActionResult Insert(LeaveCategory category)
        {
            return Ok(categoryRepository.Insert(category));
        }
    }
}
