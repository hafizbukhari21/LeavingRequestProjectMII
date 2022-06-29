using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class EmployeeRepository
    {
        private readonly MyContext context;

        public EmployeeRepository(MyContext context)
        {
            this.context = context;
        }

        public Object Get()
        {
            return context.employees.Select(emp => new {
                name = emp.name
            }).ToList();
        }
    }
}
