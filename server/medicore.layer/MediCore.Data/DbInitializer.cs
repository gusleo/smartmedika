using dna.core.auth.Entity;
using dna.core.auth.Infrastructure;
using dna.core.data.Entities;
using dna.core.data.Infrastructure;
using MediCore.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data
{
    public static class DbInitializer
    {
        private static MediCoreContext _context;

        public static async void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope() )
            {
                _context = serviceScope.ServiceProvider.GetRequiredService<MediCoreContext>();

                try
                {
                   _context.Database.Migrate();
                }catch(Exception ex )
                {
                    Console.Write("Database already migrate. Message: " + ex.Message);
                }

                await Task.Run(() => InitData());
            }

        }

        public static async Task InitData()
        {
            await SeedUser();
            InitCountry();
            InitUtcTimeBase();
            InitRegion();
            InitRegency();
            InitMenu();
        }

       
        public static void InitCountry()
        {
            if ( !_context.Country.Any() )
            {
                IList<Country> countries = new List<Country>()
                {
                    new Country() { Name = "Indonesia", Code = "IDN"}
                };
                _context.Country.AddRange(countries);
                _context.SaveChanges();
            }

        }
        public static void InitUtcTimeBase()
        {
            if ( !_context.UTCTimeBase.Any() )
            {
                IList<UTCTimeBase> utcTimes = new List<UTCTimeBase>()
                {
                    new UTCTimeBase() { Name = "Waktu Indonesia Barat", Code = "WIB", UTC = 7, CountryId = 1},
                     new UTCTimeBase() { Name = "Waktu Indonesia Tengah", Code = "WITA", UTC = 8, CountryId = 1},
                      new UTCTimeBase() { Name = "Waktu Indonesia Timur", Code = "WIT", UTC = 9, CountryId = 1}
                };
                _context.UTCTimeBase.AddRange(utcTimes);
                _context.SaveChanges();
            }

        }
        
        public static void InitRegion()
        {
            if ( !_context.Region.Any() )
            {
                IList<Region> regions = new List<Region>()
                {
                    new Region() {Name = "Bali", CountryId = 1, UTCId = 2 }
                };
                _context.Region.AddRange(regions);
                _context.SaveChanges();
            }
        }
        public static void InitRegency()
        {
            if ( !_context.Regency.Any() )
            {
                IList<Regency> regencies = new List<Regency>()
                {
                    new Regency() { Name = "Kota Denpasar", RegionId = 1},
                     new Regency() { Name = "Badung", RegionId = 1},
                      new Regency() { Name = "Karangasem", RegionId = 1},
                       new Regency() { Name = "Klungkung", RegionId = 1},
                        new Regency() { Name = "Jembrana", RegionId = 1},
                         new Regency() { Name = "Gianyar", RegionId = 1},
                          new Regency() { Name = "Tabanan", RegionId = 1},
                           new Regency() { Name = "Singaraja", RegionId = 1}
                };
                _context.Regency.AddRange(regencies);
                _context.SaveChanges();
            }
        }

        public static void InitMenu()
        {
            if ( !_context.TreeMenu.Any() )
            {
                IList<TreeMenu> menus = new List<TreeMenu>()
                {
                    new TreeMenu(){ DisplayIcon = "explore", DisplayName = "HOME", Link = "/", ParentId = 0,
                        Roles = "SuperAdmin,Operator", Type = MenuType.Admin, Order = 1},

                     new TreeMenu(){ DisplayIcon = "today", DisplayName = "Appointment", Link = "appointment", ParentId = 0,
                        Roles = "SuperAdmin,Admin,Operator", Type = MenuType.Admin, Order = 2},

                      new TreeMenu(){ DisplayIcon = null, DisplayName = "Daftar Appointment", Link = "list", ParentId = 2,
                        Roles = "SuperAdmin,Admin,Operator", Type = MenuType.Admin, Order = 1},

                       new TreeMenu(){ DisplayIcon = null, DisplayName = "Register", Link = "register", ParentId = 2,
                        Roles = "SuperAdmin,Admin,Operator", Type = MenuType.Admin, Order = 2},

                       new TreeMenu(){ DisplayIcon = "local_hospital", DisplayName = "Klinik", Link = "clinic", ParentId = 0,
                        Roles = "SuperAdmin", Type = MenuType.Admin, Order = 3}

                        

                };
                _context.TreeMenu.AddRange(menus);
                _context.SaveChanges();
            }
        }

        public static async Task SeedUser()
        {

            var user = new ApplicationUser
            {
                UserName = "admin@smartmedika.com",
                NormalizedUserName = "admin@smartmedika.com",
                Email = "admin@smartmedika.com",
                NormalizedEmail = "admin@smartmedika.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new CustomRoleStore(_context);

            IList<ApplicationRole> roles = new List<ApplicationRole>()
            {
                new ApplicationRole() {Name = MembershipConstant.SuperAdmin, NormalizedName =  MembershipConstant.SuperAdmin.ToUpper()},
                new ApplicationRole() {Name = MembershipConstant.Admin, NormalizedName =  MembershipConstant.Admin.ToUpper() },
                new ApplicationRole() {Name = MembershipConstant.Operator, NormalizedName =  MembershipConstant.Operator.ToUpper() },
                new ApplicationRole() {Name = MembershipConstant.Staff, NormalizedName =  MembershipConstant.Staff.ToUpper() },
                new ApplicationRole() {Name = MembershipConstant.Owner, NormalizedName =  MembershipConstant.Owner.ToUpper() },
                new ApplicationRole() {Name = MembershipConstant.Member, NormalizedName =  MembershipConstant.Member.ToUpper() }
            };
            foreach ( var role in roles )
            {
                //var existRole = await roleStore.FindByNameAsync(role.NormalizedName);
                if ( !_context.Roles.Any(x => x.Name == role.Name) )
                {
                    await roleStore.CreateAsync(new ApplicationRole { Name = role.Name, NormalizedName = role.NormalizedName });
                }
               

            }

            if ( !_context.Users.Any(u => u.UserName == user.UserName) )
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "test123!");
                user.PasswordHash = hashed;
                var userStore = new CustomUserStore(_context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, MembershipConstant.SuperAdmin);
            }
        }

    }
}
