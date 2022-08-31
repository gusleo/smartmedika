﻿using dna.core.data;
using dna.core.data.UnitOfWork;
using dna.core.service.Services;
using dna.core.service.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dna.core.service
{
    public static class DnaCoreDependency
    {
        public static void AddDnaCoreDependency(this IServiceCollection services)
        {
            services.AddScoped<IDnaCoreContext>(provider => provider.GetService<DnaCoreContext>());
            services.AddScoped<IDNAUnitOfWork, DNAUnitOfWork>();
            services.AddTransient<IFileServices, FileServices>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IArticleFavoriteService, ArticleFavoriteService>();
            services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IAdvertisingService, AdvertisingService>();
            services.AddScoped<IFirebaseUserMapUserService, FirebaseUserMapService>();
            services.AddScoped<IUserService, UserService>();
            
        }
    }
}
