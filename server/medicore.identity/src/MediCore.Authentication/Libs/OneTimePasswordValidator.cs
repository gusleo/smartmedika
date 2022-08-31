using dna.core.auth.Entity;
using dna.core.data;
using dna.core.libs.Validation;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MediCore.Authentication.Libs
{
    public class OneTimePasswordValidator : IExtensionGrantValidator
    {
        const string PROVIDER = "otp";
        readonly ITokenValidator _validator;
        readonly DnaCoreContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public OneTimePasswordValidator(ITokenValidator validator, UserManager<ApplicationUser> userManager, DnaCoreContext context)
        {
            _validator = validator;
            _userManager = userManager;
            _context = context;
        }
        public string GrantType => PROVIDER;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            
            var username = context.Request.Raw.Get("username");
            var code = context.Request.Raw.Get("code");

            if ( string.IsNullOrEmpty(username) || string.IsNullOrEmpty(code) )
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }



            ApplicationUser user;
            if ( Validator.EmailIsValid(username) )
            {
                user = await _userManager.FindByEmailAsync(username);
            }
            else
            {
                user = _context.Users.Where(xx => xx.PhoneNumber == username).FirstOrDefault();
            }


            if ( user != null )
            {
                var request = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);
                if(!request.Succeeded){
                    var errors = string.Join(", ", request.Errors.Select(xx => xx.Description));
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, errors);
                    return;
                }

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim("AspNet.Identity.SecurityStamp", user.SecurityStamp)

                };
                if (roles.Any())
                {
                    claims.AddRange(roles.Select(role => new Claim( JwtClaimTypes.Role, role)));
                }
                context.Result = new GrantValidationResult(user.Id.ToString(), PROVIDER, claims, PROVIDER, null);
                return;

            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "User Not Found");
            return;
        }
    }
}
