using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dna.core.auth.Entity;
using dna.core.auth.Infrastructure;
using dna.core.data;
using IdentityModel;
using IdentityServer.External.TokenExchange.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MediCore.Authentication.Libs
{
    public class ExternalUserStore : IExternalUserStore
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly DnaCoreContext _context;

        public ExternalUserStore(DnaCoreContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public Task<bool> FindUserByEmailAsync(string email)
        {
            var user = _userManager.FindByEmailAsync(email);
            return Task.FromResult(user == null);

        }

        public Task<bool> FindUserByExternalIdAsync(string externalId)
        {
            var user =_context.Users.FirstOrDefault(x => x.ProviderSubjectId == externalId);
            return Task.FromResult(user == null);
        }

        public async Task<string> CreateExternalUserAsync(string externalId, string email, string provider)
        {
            // check email already exist
            // if not create new user
            // else associated exsiting with social media
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null ){
                user = new ApplicationUser
                {
                    ProviderSubjectId = externalId,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    ProviderName = provider
                };
                // Generate password
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, Guid.NewGuid().ToString().Substring(0, 8));
                user.PasswordHash = hashed;

                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, MembershipConstant.Member);
            }else{
                user.ProviderSubjectId = externalId;
                user.EmailConfirmed = true;
                user.ProviderName = provider;
                await _userManager.UpdateAsync(user);
            }


            return user.Id.ToString();
        }

        public async Task<List<Claim>> GetUserClaimsByExternalIdAsync(string externalId)
        {
            
            var user = _context.Users.FirstOrDefault(x => x.ProviderSubjectId == externalId);
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                return claims.ToList();
            }

            return new List<Claim>();
        }

        public Task<string> FindByProviderAsync(string provider, string externalId)
        {
            var user = _context.Users.FirstOrDefault(x => x.ProviderName.ToLower() == provider.ToLower()
                                                  && x.ProviderSubjectId == externalId);
            return Task.FromResult(user?.Id.ToString());
        }

        public Task<string> FindByIdAsync(string subjectId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == int.Parse(subjectId));
            return Task.FromResult(user?.Id.ToString());
        }

        public async Task<List<Claim>> GetUserClaimsByIdAsync(string subjectid)
        {
            var user = await _userManager.FindByIdAsync(subjectid);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim("AspNet.Identity.SecurityStamp", user.SecurityStamp)

                };
                if( roles.Any()){
                    claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));
                }
                return claims.ToList();
            }

            return new List<Claim>();
        }
    }
}
