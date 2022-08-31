using dna.core.libs.Upload.Abstract;
using Microsoft.AspNetCore.Http;
using dna.core.libs.Upload.Config;
using Microsoft.Extensions.Options;
using System;
using Amazon.S3;
using dna.core.libs.Upload.Server;
using System.Threading.Tasks;

namespace dna.core.libs.Upload
{
    public class UploadService : IUploadService
    {
       
        private readonly ServerConfig _config;
        private IServerType _serverType;
        
        public UploadService(IOptions<ServerConfig> config, IAmazonS3 s3Client)
        {
            _config = config.Value;
            _serverType = new ServerTypeFactory().Create(_config);   

            // Can't adding IAmazonS3 by factory pattern,
            // created special function and refrences the Dependency Injection
            if(_config.ServerType == ServerConfig.AWS )
            {
                (_serverType as AWSBucketServer).Register(s3Client);
            }
        }

        public UploadMessage Delete(string path)
        {
            return _serverType.Delete(path);
        }

        public async Task<UploadMessage> DeleteAsync(string path)
        {
            return await _serverType.DeleteAsync(path);
        }

        public async Task<UploadMessage> UploadAsync(IFormFile file, string rootFolder = "")
        {
            return await _serverType.UploadAsync(file, rootFolder);
        }
    }
}
