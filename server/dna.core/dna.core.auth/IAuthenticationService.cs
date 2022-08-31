using dna.core.auth.Entity;
using dna.core.auth.Infrastructure;
using dna.core.auth.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dna.core.auth
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Get current user is authenticated
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticate();
        /// <summary>
        /// Get user IsSuperAdmin
        /// </summary>
        /// <returns></returns>
        bool IsSuperAdmin();
        /// <summary>
        /// Get user role
        /// </summary>
        /// <returns></returns>
        string GetUserRole();

		
        /// <summary>
        /// Register new user by email
        /// </summary>
        /// <param name="model"></param>
        /// <param name="emailConfirmed"></param>
        /// <returns></returns>
        Task<DnaIdentityResult> Register(RegisterModel model, bool emailConfirmed, bool smsConfirmed);

        Task<IdentityResult> RegisterWithPhoneNumber(string phoneNumber);

        /// <summary>
        /// Get user info of current logged user
        /// </summary>
        /// <returns></returns>
        //Task<JsonResult> GetLoginUser();
        /// <summary>
        /// Get UserID of current logged user
        /// </summary>
        /// <returns></returns>
        Nullable<int> GetUserId();
        /// <summary>
        /// User confirm email
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="code">Token</param>
        /// <returns></returns>
        Task<AuthResult> ConfirmEmail(string userId, string code);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<SignInResult> Login(LoginModel model, bool lockoutOnFailure = false);
        Task<AuthResult> Logout();
        AuthenticationProperties ConfigureExternalAuthentication(string provider, string returnUrl);
        Task<ExternalLoginInfoModel> GetExternalLogin(bool isPersistent = false);

        Task<AuthResult> InitRoleAndUser();

        Task<AuthResult> RequestPhoneToken(string phoneNumber);
        Task<AuthResult> VerifyPhoneCode(VerifyPhoneCodeModel model);
        Task<AuthResult> VerifyCode(VerifyCodeModel model);
        Task<AuthResult> AddEmail(string email);
        Task<AuthResult> GenerateActivationCode(string provider, string receiver);

        Task<AuthResult> AssignRoleToUserAsync(int userId, string[] roles);

        Task<IList<ApplicationRole>> GetAvailableRoleAsync();
        Task<IList<string>> GetUserRoleAsync(int userId);

        Task<IList<Claim>> GetClaimsAsync();

    }
}
