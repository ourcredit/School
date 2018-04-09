using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace School.EntityFrameworkCore
{
    public static class SchoolDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SchoolDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SchoolDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}
