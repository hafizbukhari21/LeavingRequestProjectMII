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
        public Object GetId(int category_id)
        {
            return context.leaveCategories.Select(cat => new LeaveCategory
            {
                category_id = cat.category_id,
                nameCategory = cat.nameCategory,
            }).ToList().FirstOrDefault(cat => cat.category_id == category_id);
        }

        public Object Update(LeaveCategory category)
        {
            LeaveCategory cat = context.leaveCategories.Find(category.category_id);
            cat.nameCategory = category.nameCategory;

            context.leaveCategories.Update(cat);
            return context.SaveChanges();
        }

        public int Insert(LeaveCategory category)
        {
            LeaveCategory cat = new LeaveCategory
            {
                nameCategory = category.nameCategory
            };
            context.Add(cat);
            return context.SaveChanges();
        }
    }
}
