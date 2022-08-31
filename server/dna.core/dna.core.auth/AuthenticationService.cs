using dna.core.auth.Entity;
using dna.core.auth.Infrastructure;
using dna.core.auth.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Linq;
using dna.core.libs.Sender;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace dna.core.auth
{

    public partial class AuthenticationService : IAuthenticationService
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        private readonly HttpContext _context;
       

        private readonly ISenderFactory _sender;
       
        public AuthenticationService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, 
            ISenderFactory sender)
        {
            _context = contextAccessor.HttpContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _sender = sender;

           
          
        }

        public async Task<AuthResult> InitRoleAndUser()
        {
            await InitializeRolesAndUser();
            return new AuthResult() { Suceess = true, Message = AuthResult.SUCCESS};
        }

        protected async Task<ApplicationUser> GetCurrentUser()
        {
            var claim = _context.User.Claims.Where(c => c.Type.Equals("sub")).FirstOrDefault();
            return await _userManager.FindByIdAsync(claim.Value);
        }

        public bool IsAuthenticate()
        {
            return _context.User.Identity.IsAuthenticated;
        }
        
        public string GetUserRole()
        {
            var res =_context.User.Claims.Where(x => x.Type.Equals("role")).FirstOrDefault();
            if ( res != null )
                return res.Value;
            else
                return "";
        }
        public bool IsSuperAdmin()
        {
            return _context.User.IsInRole(MembershipConstant.SuperAdmin);
        }

        public async Task<DnaIdentityResult> Register(RegisterModel model, bool emailConfirmed, bool smsConfirmed)
        {
			
            
            var user = new ApplicationUser()
            {
                UserName = String.IsNullOrWhiteSpace(model.Username) ? model.Email : model.Username,
                PhoneNumber = model.CellPhone,
                Email = model.Email
            };

            var response = await _userManager.CreateAsync(user, model.Password);
            var result = new DnaIdentityResult(response);

            if ( result.Result.Succeeded )
            {
                result.UserId = user.Id;
                if ( !String.IsNullOrWhiteSpace(model.Email) && emailConfirmed == true )
                {
                    result.GeneratedToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                }
                if(!String.IsNullOrEmpty(model.CellPhone) && smsConfirmed == true){
                    result.GenerateSMSToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.CellPhone);
                }

                
                await _userManager.AddToRoleAsync(user, MembershipConstant.Member);

                if(emailConfirmed == false)
                    await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;


        }

        public async Task<ClaimsIdentity> Login(LoginModel model)
        {
            
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
            if ( result.Succeeded )
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var claims = await _userManager.GetClaimsAsync(user);

                return await Task.FromResult(new ClaimsIdentity(new GenericIdentity(model.Username, "Token"), claims));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
        
        public async Task<AuthResult> ConfirmEmail(string userId, string code)
        {
            
            var message = new AuthResult();
            if ( userId == null || code == null )
            {
                message.Message = AuthResult.ERROR;
            }else
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                if ( user == null )
                {
                    message.Message = AuthResult.USER_NOTFOUND;
                }
                else
                {
                    var result = await _userManager.ConfirmEmailAsync(user, code);
                    if ( result.Succeeded )
                    {
                        user.Status = UserStatus.Active;
                        await _userManager.UpdateAsync(user);
                        message.Suceess = true;
                        message.Message = AuthResult.SUCCESS;
                    }
                }
            }
            return message;
        }
       
        /*public async Task<JsonResult> GetLoginUser()
        {           
            return new JsonResult(await GetCurrentUser());
        }*/
        public int? GetUserId()
        {
            try
            {
                string Id = _context.User.Claims
                        .Where(c => c.Type.Equals("sub")).FirstOrDefault().Value;
                if ( !String.IsNullOrWhiteSpace(Id) )
                {
                    return int.Parse(Id);
                }
            }catch(Exception ex )
            {
                Console.Write(ex.Message);
            }
            
            return null;
           
            
        }

        public async Task<SignInResult> Login(LoginModel model, bool lockoutOnFailure = false)
        {
            return  await _signInManager.PasswordSignInAsync(model.Username, model.Password, 
                        model.RememberLogin, lockoutOnFailure: lockoutOnFailure);
        }
        public async Task<AuthResult> Logout()
        {
           
            var message = new AuthResult();
            try
            {
                await _signInManager.SignOutAsync();
                message.Suceess = true;
                message.Message = AuthResult.SUCCESS;
            }catch(Exception ex )
            {
                message.Message = ex.Message;
            }
            return message;
            
        }
        //TODO: Fixing external login
        public AuthenticationProperties ConfigureExternalAuthentication(string provider, string returnUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);

        }
        public async Task<ExternalLoginInfoModel> GetExternalLogin(bool isPersistent = false)
        {
            try
            {
               
                var info = await _signInManager.GetExternalLoginInfoAsync();
                // Sign in the user with this external login provider if the user already has a login.
                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: isPersistent);
                return new ExternalLoginInfoModel
                {
                    Info = info,
                    Result = result
                };
            }
            catch ( Exception ex )
            {
                Console.Write("External Login: " + ex.Message);
                return null;
            }
        }

        public async Task<IdentityResult> RegisterWithPhoneNumber(string phoneNumber)
        {
            
            
            var username = Guid.NewGuid();
            var password = Guid.NewGuid().ToString().Substring(0, 8);
            var user = new ApplicationUser()
            {
                UserName = username.ToString(),
                PhoneNumber = phoneNumber
            };
            var result = await _userManager.CreateAsync(user, password);
            

            if ( result.Succeeded )
            {
                
                await _userManager.AddToRoleAsync(user, MembershipConstant.Member);
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
                await _signInManager.SignInAsync(user, isPersistent: false);
                ISender sender = _sender.Create("sms");
                var response = await sender.SendAsync(phoneNumber, "", String.Format("Your code verification: {0}", code));
               
                            
            }

            return result;
        }

        public async Task<AuthResult> RequestPhoneToken(string phoneNumber)
        {
            var user = await GetCurrentUser();
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            return new AuthResult
            {
                Suceess = true,
                Message = code
            };
        }

        

        public Task<AuthResult> AddEmail(string email)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthResult> GenerateActivationCode(string provider, string receiver)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            var message = new AuthResult();

            if ( user == null )
                message.Message = AuthResult.USER_NOTFOUND;

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, provider);
            if ( string.IsNullOrWhiteSpace(code) )
            {
                message.Message = AuthResult.FAILED_GENERATE_TOKEN;
            }

            
            if (provider == ProviderConstant.SMS )
            {
                var tokenMessage = "Your security code is: " + code;
                ISender sender = _sender.Create("sms");
                await sender.SendAsync(user.PhoneNumber, "", tokenMessage);

                message.Suceess = true;
                message.Message = AuthResult.SUCCESS;
            }else
            {
                //activation code for email
                message.Suceess = true;
                message.Message = AuthResult.SUCCESS;
            }

            return message;
        }

        public async Task<AuthResult> VerifyCode(VerifyCodeModel model)
        {
            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            var message = new AuthResult();
            if ( result.Succeeded )
            {
                message.Suceess = true;
                message.Message = AuthResult.SUCCESS;
            }
            if ( result.IsLockedOut )
            {
                message.Message = "User account locked out.";
            }
            else
            {
                message.Message = "Invalid code.";
            }

            return message;
        }

        public async Task<AuthResult> AssignRoleToUserAsync(int userId, string[] roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var message = new AuthResult();
            var existRoles = await _userManager.GetRolesAsync(user);

            //adding new roles
            var newRoles = roles.Where(p => !existRoles.Any(p2 => p2 == p)).ToList();
            await _userManager.AddToRolesAsync(user, newRoles);

            //remove new roles
            var deletedRoles = existRoles.Where(ex => !roles.Any(p2 => p2 == ex)).ToList();
            await _userManager.RemoveFromRolesAsync(user, deletedRoles);

            message = new AuthResult() { Suceess = true, Message = "Successfully add roles" };
            return message;
        }

        public async Task<IList<ApplicationRole>> GetAvailableRoleAsync()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return result;
        }
        public async Task<IList<string>> GetUserRoleAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.IsEmailConfirmedAsync( user );
        }

        public async Task<AuthResult> VerifyPhoneCode(VerifyPhoneCodeModel model)
        {
            var user = await GetCurrentUser();
            var results = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.PhoneCode);
            return new AuthResult
            {
                Suceess = results.Succeeded,
                Message = String.Format("{0} to verify phone number", results.Succeeded ? "Success" : "Failed")
            };
        }

        public async Task<IList<Claim>> GetClaimsAsync()
        {
            var user = await GetCurrentUser();
            var claims = await _userManager.GetClaimsAsync(user);
            return claims;
        }
    }
}
