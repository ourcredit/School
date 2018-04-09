using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using School.Authorization.Roles;
using School.Authorization.Users;
using School.MultiTenancy;

namespace School.EntityFrameworkCore
{
    public class SchoolDbContext : AbpZeroDbContext<Tenant, Role, User, SchoolDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }
    }
}
