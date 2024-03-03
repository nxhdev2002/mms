using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace prod.EntityFrameworkCore
{
    public static class prodDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<prodDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<prodDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}