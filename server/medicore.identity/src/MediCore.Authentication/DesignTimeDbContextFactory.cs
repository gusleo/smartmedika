using dna.core.data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MediCore.Authentication
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DnaCoreContext>
    {
        public DnaCoreContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<DnaCoreContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName));

            return new DnaCoreContext(builder.Options);
        }
    }
    public class DesignConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName));

            return new ConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        }
    }

    public class DesignPersistedGrantDbContextFactory : 
        IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {

        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .Build();

            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName));

            return new PersistedGrantDbContext(builder.Options, new OperationalStoreOptions());
        }
    }

}
