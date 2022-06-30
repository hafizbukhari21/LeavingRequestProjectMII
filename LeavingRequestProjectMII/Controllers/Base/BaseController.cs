using API.Repositories.IRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Base
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity :class
        where Repository :IRepository<Entity,Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        //[HttpPatch]
        //[EnableCors("AllowOrigin")]
        //public ActionResult<Entity> Update(Entity entity)
        //{
        //    return Ok(repository.Update(entity));
        //}

        //[HttpDelete("{key}")]
        //[EnableCors("AllowOrigin")]
        //public ActionResult<Entity> Delete(Key key)
        //{
        //    return Ok(repository.Delete(key));
        //}
    }
}
