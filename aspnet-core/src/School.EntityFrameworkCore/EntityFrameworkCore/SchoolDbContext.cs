using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using School.Authorization.Roles;
using School.Authorization.Users;
using School.Models;
using School.MultiTenancy;

namespace School.EntityFrameworkCore
{
    public class SchoolDbContext : AbpZeroDbContext<Tenant, Role, User, SchoolDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<Point> Points { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceGood> DeviceGoods { get; set; }
        public virtual DbSet<OperatorTree> OperatorTrees { get; set; }
        public virtual DbSet<OperatorDevice> OperatorDevices { get; set; }

        public  virtual  DbSet<Orders > Orders { get; set; }
        public  virtual  DbSet<Channel > Channels { get; set; }
        public  virtual  DbSet<Show > Shows { get; set; }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }
    }

   
}
