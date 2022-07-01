using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class DivisiRepository 
    {
        private readonly MyContext context;

        public DivisiRepository(MyContext context)
        {
            this.context = context;
        }
        public Object Get()
        {
            return context.divisi.ToList().Select(div=>new Divisi { 
                divisi_id =div.divisi_id,
                namaDivisi = div.namaDivisi

            });
        }

        public Object Get(Divisi divisi)
        {
            return context.divisi.Select(div => new Divisi
            {
                divisi_id = div.divisi_id,
                namaDivisi = div.namaDivisi,
                isDeleted = div.isDeleted
            }).ToList().FirstOrDefault(div=>div.divisi_id == divisi.divisi_id);
        }

        public Object Update(Divisi divisi)
        {
            Divisi div = context.divisi.Find(divisi.divisi_id);
            div.namaDivisi = divisi.namaDivisi;

            context.divisi.Update(div);
            return context.SaveChanges();
        }
    }
}
