using BM.Auth.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Infrastructure
{
    public class AuthDbContext : DbContext
    {
        public DbSet<AuthUser> Users { get; set; }
        public DbSet<AuthEmp> Emps { get; set; }
        public DbSet<AuthPosition> Positions { get; set; }
        public DbSet<AuthSpecialty> Specialties { get; set; }
        public DbSet<AuthCustomer> Customers { get; set; }
        public DbSet<AuthSchedule> Schedules { get; set; }
        public DbSet<AuthScheEmp> ScheEmps { get; set; }
        public DbSet<AuthRole> Roles { get; set; }
        public DbSet<AuthPermission> Permissions { get; set; }
        public DbSet<AuthRolePermission> RolePermissions { get; set; }
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthUser>()
                .HasOne(u => u.AuthEmp)
                .WithOne(e => e.AuthUser)
                .HasForeignKey<AuthEmp>(e => e.userID); //(1-1)

            modelBuilder.Entity<AuthUser>()
              .HasOne(u => u.AuthCustomer)
              .WithOne(e => e.AuthUser)
              .HasForeignKey<AuthCustomer>(e => e.userID); //(1-1)


            modelBuilder.Entity<AuthEmp>()
                .HasOne(e => e.AuthPosition)
                .WithMany(p => p.AuthEmps)
                .HasForeignKey(e => e.positionID); //(1-n)

            modelBuilder.Entity<AuthEmp>()
                .HasOne(e => e.AuthSpecialty)
                .WithMany(s => s.AuthEmps)
                .HasForeignKey(e => e.specialtyID); //(1-n)

            modelBuilder.Entity<AuthScheEmp>()
                .HasOne(se => se.AuthEmp)
                .WithMany (e => e.AuthScheEmps)
                .HasForeignKey(se => se.empID); //(1-n)

            modelBuilder.Entity<AuthScheEmp>()
                .HasOne(se => se.AuthSchedule)
                .WithMany(s => s.AuthScheEmps)
                .HasForeignKey(se => se.scheduleID); //(1-n)

            modelBuilder.Entity<AuthUser>()
                .HasOne(u => u.AuthRoles)
                .WithMany(r => r.AuthUsers)
                .HasForeignKey(r => r.roleID); //(1-n)

            modelBuilder.Entity<AuthRole>()
                .HasMany(r => r.AuthRolePermissions)
                .WithOne(u => u.AuthRole)
                .HasForeignKey(u => u.roleID); //(1-n)

            modelBuilder.Entity<AuthPermission>()
                .HasMany(p => p.AuthRolePermissions)
                .WithOne(r => r.AuthPermission)
                .HasForeignKey(r => r.permissionID); //(1-n)

            base.OnModelCreating(modelBuilder);
        }
    }
}
