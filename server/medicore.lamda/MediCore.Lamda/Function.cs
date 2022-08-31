using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Amazon.Lambda.Core;
using MediCore.Lamda.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MediCore.Lamda.Data.Repositories;
using MediCore.Lamda.Data.Entitties;
using System.Data.Common;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MediCore.Lamda
{
    public class Function
    {
        public static Lazy<ILifetimeScope> LifetimeScope { get; set; } = new Lazy<ILifetimeScope>(CreateContainer);
        private static ILifetimeScope CreateContainer()
        {
            var configuration = Configuration.Instance();
            var builder = new ContainerBuilder();
            builder.Register(c =>
            {
                var opt = new DbContextOptionsBuilder<MediCoreContext>();
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

                return new MediCoreContext(opt.Options);
            }).AsImplementedInterfaces().InstancePerLifetimeScope(); // AsSelf
            builder.RegisterType<MedicalStaffRepository>().As<IMedicalStaffRepository>().InstancePerLifetimeScope();
            builder.RegisterType<HospitalAppointmentRatingRepository>().As<IHospitalAppointmentRatingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<HospitalRepository>().As<IHospitalRepository>().InstancePerLifetimeScope();
            return builder.Build();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> FunctionHandler(ILambdaContext context)
        {
            var response = new List<object>();
            using ( var innerScope = LifetimeScope.Value.BeginLifetimeScope() )
            {
                var ratingRepo = innerScope.Resolve<IHospitalAppointmentRatingRepository>();

                // Update Medical Staff Rating
                var staffRepo = innerScope.Resolve<IMedicalStaffRepository>();
                var medicalStaffs = await staffRepo.GetAllAsync();
                var staffRated = new List<MedicalStaff>();
                
                foreach(var item in medicalStaffs )
                {
                    var ratings = await ratingRepo.GetRatingByMedicalStaffAsync(item.Id);
                    var newValue = GenerateRatingValue(ratings);
                    if(item.Rating != null && item.Rating != newValue )
                    {
                        item.Rating = newValue;
                        staffRated.Add(item);
                    }
                    
                }
              
                if(staffRated.Count() > 0 )
                {
                    staffRepo.EditRange(medicalStaffs);
                    staffRepo.SaveChanges();
                    response.Concat(staffRated.Cast<object>());
                }
                
               

                // Update Hospital Staff Repo
                var hospitalRepo = innerScope.Resolve<IHospitalRepository>();
                var hospitals = await hospitalRepo.GetAllAsync();
                var hospitalRated = new List<Hospital>();
                foreach ( var item in hospitals )
                {
                    var ratings = await ratingRepo.GetRatingByHospitalAsync(item.Id);
                    var newValue = GenerateRatingValue(ratings);
                    if ( item.Rating != null && item.Rating != newValue )
                    {
                        item.Rating = newValue;
                        hospitalRated.Add(item);
                    }
                }
                if(hospitalRated.Count() > 0 )
                {
                    hospitalRepo.EditRange(hospitals);
                    hospitalRepo.SaveChanges();
                    response.Concat(hospitalRated.Cast<object>());
                }
              
                

            }

            return response;

        }

        private double GenerateRatingValue(IList<int> ratings)
        {
            var rating1 = ratings.Where(x => x == 1).Count();
            var rating2 = ratings.Where(x => x == 2).Count();
            var rating3 = ratings.Where(x => x == 3).Count();
            var rating4 = ratings.Where(x => x == 4).Count();
            var rating5 = ratings.Where(x => x == 5).Count();
            var total = rating5 + rating4 + rating3 + rating2 + rating1;
            if ( total > 0 )
                return ((5 * rating5) + (4 * rating4) + (3 * rating3) + (2 * rating2) + (1 * rating1))
                            / (total);
            else
                return 0;

        }

        
    }
}
