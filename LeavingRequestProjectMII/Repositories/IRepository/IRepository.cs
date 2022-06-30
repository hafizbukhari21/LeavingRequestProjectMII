using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.IRepository
{
    public interface IRepository<Entity,Key> where Entity:class
    {
        Object Update(Entity entity);
        Object Delete(Key entity);
    }
}
