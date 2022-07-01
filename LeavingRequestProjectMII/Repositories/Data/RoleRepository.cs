using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class RoleRepository
    {
        private readonly MyContext context;

        public RoleRepository(MyContext context)
        {
            this.context = context;
        }

        public Object GetRole()
        {
            return context.roles.Select(role => new Role { 
                role_id  = role.role_id,
                roleName = role.roleName
                
            });
        }
    }
}
