using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MediCore.Authentication
{
    //ref: https://identityserver4.readthedocs.io/en/dev/configuration/resources.html
    public class Config
    {
        private const int ONE_MONTH = 2592000;
        private const int HALF_MONTH = 1296000;
        private const int FIVE_DAY = (3600 * 24 * 5);


        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {

            return new List<ApiResource>
            {

                new ApiResource
                {
                    Name = "medicore",
                    DisplayName = "MediCore API",
                    UserClaims =
                    {
                        //must be JwtClaimTypes to get Identity
                        //also "AspNet.Identity.SecurityStamp"
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Role,
                        "AspNet.Identity.SecurityStamp"
                    },
                    Scopes =
                    {
                       
                        new Scope
                        {
                            Name = "medicore.basic",
                            DisplayName = "MediCore user have basic access",
                           
                        },
                        new Scope
                        {
                            Name = "medicore.full_access",
                            DisplayName = "MediCore user have full access"
                            
                        }
                    }
                }
               
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                //NOTE: make sure RedirectUri is same both Client and Javascript config
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { "http://localhost:9999/auth.html", "http://localhost:9999/callback", "http://52.87.109.107/callback", "http://admin.smartmedika.com/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:9999/index.html", "http://localhost:9999","http://52.87.109.107", "http://admin.smartmedika.com/index.html" },
                    AllowedCorsOrigins = { "http://localhost:9999", "http://admin.smartmedika.com", "http://52.87.109.107" },
                    RequireConsent = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "medicore.full_access"
                    },
                    AllowOfflineAccess = true,
                    IdentityTokenLifetime = (3600 * 24 * 4),
                    AccessTokenLifetime = (3600 * 24 * 5),
                    AbsoluteRefreshTokenLifetime = (3600 * 24 * 4),
                    SlidingRefreshTokenLifetime = (3600 * 24 * 2),
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
                // Ionic Client
                new Client
                {
                    ClientId = "medicore",
                    ClientName = "Hybrid Application",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("FuckGoverment".Sha256())
                    },
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword, "external", "otp"},
                    //AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "medicore.basic"
                    },
                    AllowOfflineAccess = true,
                    IdentityTokenLifetime = (3600 * 24 * 4),
                    AccessTokenLifetime = FIVE_DAY,
                    AbsoluteRefreshTokenLifetime = (3600 * 24 * 4),
                    SlidingRefreshTokenLifetime = (3600 * 24 * 2),
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
                //Android - AppAuth
                //https://github.com/IdentityServer/IdentityServer4/issues/479
                new Client
                {
                    ClientId = "client.android",
                    RequireClientSecret = false,
                    ClientName = "Android app client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    //AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris = { "com.yourcompany.app://oidccallback" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "api1"
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}
