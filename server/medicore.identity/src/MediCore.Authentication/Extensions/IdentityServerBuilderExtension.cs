using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace MediCore.Authentication.Extensions
{
    public static class IdentityServerBuilderExtension
    {
        public static void AddCertificateFromFile( this IIdentityServerBuilder builder, IConfigurationSection options, ILogger logger){
            var keyFilePath = options.GetValue<string>("KeyFilePath");
            var keyFilePassword = options.GetValue<string>("KeyFilePassword");

            if ( File.Exists(keyFilePath) )
            {
                logger.LogDebug($"SigninCredentialExtension adding key from file {keyFilePath}");

                // You can simply add this line in the Startup.cs if you don't want an extension. 
                // This is neater though ;)
                builder.AddSigningCredential(new X509Certificate2(keyFilePath, keyFilePassword));
            }
            else
            {
                logger.LogError($"SigninCredentialExtension cannot find key file {keyFilePath}");
            }
        }
    }
}
