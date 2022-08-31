using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.IdentityModel.Tokens;
using dna.core.data;
using dna.core.auth.Entity;
using IdentityServer4.Services;
using MediCore.Service;
using IdentityServer4.Validation;
using MediCore.Authentication.Libs;
using dna.core.auth.Infrastructure;
using MediCore.Service.Services;
using MediCore.Data;
using MediCore.Data.UnitOfWork;
using System.Security.Cryptography.X509Certificates;
using StackExchange.Redis;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using dna.core.libs.Sender.ConfigType;
using IdentityServer.External.TokenExchange.Interfaces;
using IdentityServer.External.TokenExchange.Stores;
using MediCore.Authentication.Services;

namespace MediCore.Authentication
{
    public class Startup
    {
        IHostingEnvironment env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if ( env.IsEnvironment("Development") )
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            string authority = Configuration.GetSection("Authentication").Value;

            
            services.AddDbContext<MediCoreContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(migrationsAssembly)));
            services.AddDbContext<DnaCoreContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), 
                b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<DnaCoreContext>()
                .AddDefaultTokenProviders();
            
            services.AddMvc();

            // Add application services.           
            services.AddTransient<IProfileService, IdentityProfileService>();
            services.AddTransient<IExtensionGrantValidator, OneTimePasswordValidator>();
           
            //custom DI for Dna Authentication
            services.AddDnaAuthentication();
            services.AddScoped<IMediCoreContext>(provider => provider.GetService<MediCoreContext>());
            services.AddScoped<IMediCoreUnitOfWork, MediCoreUnitOfWork>();
            services.AddScoped<IUserDetailService, UserDetailService>();

            services.AddScoped<ITokenExchangeProviderStore, DefaultTokenExchangeProviderStore>();
            services.AddScoped<IExternalUserStore, ExternalUserStore>();

            //Messaging         
            services.Configure<SenderConfiguration>(Configuration.GetSection("Sender"));
            services.AddScoped<IEmailSender, EmailSender>();

            var serverKey = Configuration.GetSection("SigninKeyCredentials");
            var keyFilePath = serverKey.GetValue<string>("KeyFilePath");
            var keyFilePassword = serverKey.GetValue<string>("KeyFilePassword");

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer(opt =>
            {
                opt.IssuerUri = authority;
                opt.PublicOrigin = authority;
            })
                .AddCorsPolicyService<InMemoryCorsPolicyService>()
                .AddSigningCredential(new X509Certificate2(keyFilePath, keyFilePassword))
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                .AddTokenExchangeForExternalProviders()  //registers an extension grant
                .AddDefaultTokenExchangeProviderStore()  //registers default in-memory store for providers info
                .AddDefaultExternalTokenProviders()      //registers providers auth implementations
                .AddDefaultTokenExchangeProfileService()
                .AddExtensionGrantValidator<OneTimePasswordValidator>();
              
                


            // Redis configuration
            var redisConfig = Configuration.GetSection("Redis");
            var appName = redisConfig.GetValue<string>("ApplicationName");
            var redisConnection = redisConfig.GetValue<string>("Connection");
            var redisPassword = redisConfig.GetValue<string>("Password");
            var redis = ConnectionMultiplexer.Connect(string.Format("{0},password={1},syncTimeout=5000,abortConnect=false", redisConnection, redisPassword));
            services.AddDataProtection()
                .SetApplicationName(appName)
                .PersistKeysToRedis(redis, "DataProtectionKeys")
                .ProtectKeysWithCertificate(new X509Certificate2(keyFilePath, keyFilePassword));


            var audience = Configuration.GetSection("Authentication").Value;
            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.ClientId = Configuration["Auth:Google:ClientId"];
                    options.ClientSecret = Configuration["Auth:Google:ClientSecret"];
                })
                .AddFacebook(options =>
                {
                    options.AppId = Configuration["Auth:Facebook:AppId"];
                    options.AppSecret = Configuration["Auth:Facebook:AppSecret"];
                })
                /*.AddOpenIdConnect("oidc", "OpenID Connect", options =>
                {
                    options.Authority = authority;
                    options.ClientId = "medicore";
                    options.SaveTokens = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                })*/
                //Remember to add JwtBearer, it's used by angular and react
                .AddJwtBearer(o =>
                {
                    o.Authority = authority;
                    o.Audience = audience;
                    o.RequireHttpsMetadata = false;
                    
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // this will do the initial DB population, but we only need to do it once
            // this is just in here as a easy, yet hacky, way to get our DB created/populated
            // InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            Libs.AutoMapperConfiguration.Configure();
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {                
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                
            }
        }
    }
}
