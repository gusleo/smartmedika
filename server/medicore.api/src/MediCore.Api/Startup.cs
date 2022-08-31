using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MediCore.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using dna.core.auth.Entity;
using dna.core.auth.Infrastructure;
using Microsoft.AspNetCore.Http;
using MediCore.Service;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using dna.core.service.ErrorHandling;
using dna.core.data;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using IdentityServer4.AccessTokenValidation;

namespace MediCore.Api
{
    public partial class Startup
    {
        private IHostingEnvironment CurrentEnvironment;   
        /// <summary>
        /// Interface configuration
        /// </summary>
        public IConfigurationRoot Configuration { get; }

      /// <summary>
      /// Initialize
      /// </summary>
      /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            CurrentEnvironment = env;
        }




        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container
        /// </summary>
        /// <param name="services">Service collection <see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string authority = Configuration.GetSection("Authentication").Value;

           
            // binder & json seriliaze
            services.AddMvc()
            .AddJsonOptions(options =>
            {
                // handle loops correctly
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                // use standard name conversion of properties
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
                // donot include $id property in the output
                options.SerializerSettings.PreserveReferencesHandling =
                    PreserveReferencesHandling.None;
            });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            //use https services
            services.Configure<MvcOptions>(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add(new CorsAuthorizationFilterFactory("default"));
            });

            services.AddAuthorization();

            //HACK: when using multiple DbContext and inheret ont to another
            //make sure the order is important, sinces it can make issue when setting DbContextOptions
            services.AddDbContext<MediCoreContext>(options =>
                options.UseSqlServer(connection,
                b => b.MigrationsAssembly(assemblyName)));
            services.AddDbContext<DnaCoreContext>(options =>
               options.UseSqlServer(connection,
                 b => b.MigrationsAssembly(assemblyName)));

            //TODO: fixing Cookies
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {              
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<MediCoreContext>()
            .AddDefaultTokenProviders();


            services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             .AddIdentityServerAuthentication(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.Authority = authority;
                 options.ApiName = "medicore";
                 options.RoleClaimType = JwtClaimTypes.Role;


             });
                
            if(CurrentEnvironment.IsProduction()){
                // Adding data protection
                var serverKey = Configuration.GetSection("SigninKeyCredentials");
                var keyFilePath = serverKey.GetValue<string>("KeyFilePath");
                var keyFilePassword = serverKey.GetValue<string>("KeyFilePassword");
                var cert = new X509Certificate2(keyFilePath, keyFilePassword);

                var redisConfig = Configuration.GetSection("Redis");
                var appName = redisConfig.GetValue<string>("ApplicationName");
                var redisConnection = redisConfig.GetValue<string>("Connection");
                var redisPassword = redisConfig.GetValue<string>("Password");
                var redis = ConnectionMultiplexer.Connect(string.Format("{0},password={1},syncTimeout=7000,abortConnect=false", redisConnection, redisPassword));
                services.AddDataProtection()
                    .SetApplicationName(appName)
                    .PersistKeysToRedis(redis, "DataProtectionKeys")
                    .ProtectKeysWithCertificate(cert);
            }

            SessionCacheConfig(services);
            SwaggerConfig(services);
            DepedencyInjection(services);

            // AWS
            var opt = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();




        }

      /// <summary>
      /// Configure
      /// </summary>
      /// <param name="app"></param>
      /// <param name="env"></param>
      /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();


            if ( env.IsDevelopment() )
            {               
                app.UseDeveloperExceptionPage();
            }
            else
            {               
                app.UseExceptionHandler("/Home/Error");               
            }


            // DbInitializer.Initialize(app);

            // CultureConfig(app);

            

            AutoMapperConfiguration.Configure();

            //configure HttpContext so it can use by AuthenticationService
            HTTPHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            //change location from wwwroot to "Content" directory
           
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Content")),
                RequestPath = new PathString("/Content")
            });

            app.UseCors("default");
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            
            app.UseSession();

            app.UseAuthentication();         


            //app.UseMiddleware<CacheMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseMvc();            
            UseSwagger(app);


        }
    }
}
