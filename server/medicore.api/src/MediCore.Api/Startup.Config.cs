using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.Memory;
using dna.core.libs.Cache;
using Swashbuckle.AspNetCore.Swagger;
using MediCore.Api.Swagger;
using System.IO;

namespace MediCore.Api
{
    /// <summary>
    /// Partial class of Startup.cs
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Swagger configuration
        /// </summary>
        /// <param name="services">Service collection <see cref="IServiceCollection"/></param>
        public void SwaggerConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SmartMedika API", Version = "v1",  });
                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "MediCore.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });           
            services.ConfigureSwaggerGen(options =>
            {                
                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();               

            });

        }

        /// <summary>
        /// Configuration for session and caching
        /// </summary>
        /// <param name="services">Service collection <see cref="IServiceCollection"/></param>
        public void SessionCacheConfig(IServiceCollection services)
        {
            //session
           
            services.AddSession();
            var useRedis = Convert.ToBoolean(Configuration["Cache:UseRedis"]);
            if ( useRedis )
            {

            }else
            {
                services.AddSingleton<IMemoryCache>(factory =>
                {
                    var cache = new MemoryCache(new MemoryCacheOptions());
                    return cache;
                });
                services.AddSingleton<ICacheService, MemoryCacheService>();
            }
            services.AddTransient<CacheAttribute>();
        }

        /// <summary>
        /// Config for culture
        /// </summary>
        /// <param name="app">Application builder <see cref="IApplicationBuilder"/> </param>
        public void CultureConfig(IApplicationBuilder app)
        {
            
            var ci = new CultureInfo("en-GB");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-GB"),
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("en-GB"),
                }
            });            
            
        }
        /// <summary>
        /// Using swagger always after app.UseMvc()
        /// </summary>
        /// <param name="app">Application builder <see cref="IApplicationBuilder"/> </param>
        public void UseSwagger(IApplicationBuilder app)
        {
            //use swagger (always after app.UseMvc();)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // keeping swagger UI URL consistent with my previous settings
                c.RoutePrefix = "swagger/ui";
                // adding endpoint to JSON file containing API endpoints for UI
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                // disabling the Swagger validator -- passing null as validator URL.
                // Alternatively, you can specify your own internal validator
                c.EnableValidator(null);
            });


        }
    }
}
