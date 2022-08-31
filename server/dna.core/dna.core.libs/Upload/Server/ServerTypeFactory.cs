using dna.core.libs.Upload.Abstract;
using dna.core.libs.Upload.Config;
using dna.core.libs.Upload.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dna.core.libs.Upload
{
    public class ServerTypeFactory
    {
        
        public IServerType Create(ServerConfig config)
        {
            IServerType server;
            switch ( config.ServerType )
            {
                case ServerConfig.AZURE:
                    server = new AzureServer(config);
                    break;
                case ServerConfig.AWS:
                    server = new AWSBucketServer(config);
                    break;
                default:
                    server = new LocalServer(config);
                    break;
            }
            return server;
        }
    }
}
