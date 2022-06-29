using API.Context;
using API.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class GeneralRepository<Contex, Entity, Key> : IRepository<Entity, Key>
        where Entity:class
        where Contex:MyContext
    {
        private readonly MyContext myContext;
        private readonly DbSet<Entity> entities;

        public GeneralRepository(MyContext myContext)
        {
            this.myContext = myContext;
            entities = myContext.Set<Entity>();
        }

        public object Delete(Key key)
        {
            var deleteEntity = myContext.employees.Find(key);
            myContext.Remove(deleteEntity);
            return myContext.SaveChanges();
        }

        public object Update(Entity entity)
        {
            myContext.Entry(entity).State = EntityState.Modified;
            return myContext.SaveChanges();
        }
    }
}
