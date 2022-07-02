using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class CategoryRepository
    {
        private readonly MyContext context;

        public CategoryRepository(MyContext context)
        {
            this.context = context;
        }

        public Object GetCategory()
        {
            return context.leaveCategories.Select(cate=>new LeaveCategory { 
                category_id = cate.category_id,
                nameCategory = cate.nameCategory
            });
        }
    }
}
