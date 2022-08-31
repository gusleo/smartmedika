using dna.core.libs.Upload.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dna.core.libs.Upload.Config
{
    public class ServerConfig : IServerConfig
    {
        public const string AZURE = "Azure";
        public const string LOCAL = "Local";
        public const string AWS = "AWS";
        //As BucketName on AWS
        public string Container { get; set; }
        public string DestinationFolder { get; set; }
        public string ServerType { get; set; }
    }
}
