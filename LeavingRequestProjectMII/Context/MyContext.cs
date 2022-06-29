using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeavingRequest>()
                .HasOne(emp => emp.employees)
                .WithMany(lr => lr.leavingRequests)
                .HasForeignKey(lr => lr.employee_id);

            modelBuilder.Entity<LeavingRequest>()
                .HasOne(lcate => lcate.leaveCategory)
                .WithMany(lr => lr.leavingRequests)
                .HasForeignKey(lr => lr.category_id);

            modelBuilder.Entity<Employees>()
                .HasOne(emp => emp.role)
                .WithMany(emp => emp.employees)
                .HasForeignKey(emp => emp.role_Id);

            modelBuilder.Entity<Employees>()
                .HasOne(emp => emp.divisi)
                .WithMany(emp => emp.employees)
                .HasForeignKey(emp => emp.divisi_id);


             

               
        
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                     .UseLazyLoadingProxies();

            }

        }

        public DbSet<Employees> employees { set; get; }
        public DbSet<Role> roles { set; get; }
        public DbSet<LeavingRequest> leavingRequests { set; get; }
        public DbSet<Divisi> divisi { set; get; }
        public DbSet<LeaveCategory> leaveCategories { set; get; }
       



        
    }
}
