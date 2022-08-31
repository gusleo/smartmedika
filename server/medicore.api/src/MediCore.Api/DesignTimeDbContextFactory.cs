using MediCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MediCore.Api
{
    /// <summary>
    /// ContextFactory to migrate
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MediCoreContext>
    {
        /// <summary>
        /// Create database context from appsetting.json
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public MediCoreContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MediCoreContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName));

            return new MediCoreContext(builder.Options);
        }
    }
}
