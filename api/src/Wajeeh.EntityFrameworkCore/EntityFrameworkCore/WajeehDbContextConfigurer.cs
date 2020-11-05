using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Wajeeh.EntityFrameworkCore
{
    public static class WajeehDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<WajeehDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<WajeehDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
