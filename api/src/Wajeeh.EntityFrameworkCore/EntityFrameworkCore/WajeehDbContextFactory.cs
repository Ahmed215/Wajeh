using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Wajeeh.Configuration;
using Wajeeh.Web;

namespace Wajeeh.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class WajeehDbContextFactory : IDesignTimeDbContextFactory<WajeehDbContext>
    {
        public WajeehDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WajeehDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            WajeehDbContextConfigurer.Configure(builder, configuration.GetConnectionString(WajeehConsts.ConnectionStringName));

            return new WajeehDbContext(builder.Options);
        }
    }
}
