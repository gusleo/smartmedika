using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using dna.core.libs.Upload.Config;
using Microsoft.AspNetCore.Http;

namespace dna.core.libs.Upload.Server
{
    public class AWSBucketServer : IServerType
    {
        private ServerConfig _config;
        private IAmazonS3 _s3Client;
        public AWSBucketServer(ServerConfig config)
        {
            _config = config;
        }
        public void Register(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        public async Task<UploadMessage> DeleteAsync(string path)
        {
            var message = new UploadMessage();

            var response = await _s3Client.DeleteObjectAsync(_config.Container, path);
            message.Message = response.HttpStatusCode == HttpStatusCode.OK ? UploadMessage.SUCCESS : UploadMessage.ERROR;
            return message;
        }

        public MemoryStream Read(string containerName, string fileName, bool crop = false, int w = 0, int h = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<UploadMessage> UploadAsync(IFormFile file, string rootFolder)
        {
            UploadMessage message = new UploadMessage
            {
                Message = UploadMessage.ERROR
            };

            try
            {

                // Create a unique name for the images we are about to upload
                string filename = String.Format("{0}{1}",
                    Guid.NewGuid().ToString().Substring(0, 8),
                    Path.GetExtension(file.FileName));

                HttpStatusCode code;
                using ( MemoryStream fs = new MemoryStream() )
                {
                    file.CopyTo(fs);
                    var request = new PutObjectRequest
                    {
                        BucketName = _config.Container,
                        Key = String.Format("{0}/{1}", _config.DestinationFolder, filename),
                        InputStream = fs,
                        ContentType = file.ContentType,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var cancellationToken = new CancellationToken();
                    var response = await _s3Client.PutObjectAsync(request, cancellationToken);
                    code = response.HttpStatusCode;
                    fs.Flush();
                }

                message.FileName = file.FileName;
                message.GuidFileName = filename;
                message.FullPath = String.Format("{0}/{1}", _config.DestinationFolder, filename);
                message.Message = code == HttpStatusCode.OK ? UploadMessage.SUCCESS : UploadMessage.ERROR;

            }
            catch ( Exception ex )
            {
                Console.Write(ex.Message);
                message.Message = ex.Message;
            }

            return message;
        }

        public UploadMessage Delete(string path)
        {
            Task<UploadMessage> results = Task.Run(async () =>
            {
                var response = await _s3Client.DeleteObjectAsync(_config.Container, path);
                return new UploadMessage
                {
                    Message = response.HttpStatusCode == HttpStatusCode.OK ? UploadMessage.SUCCESS : UploadMessage.ERROR
                };
            });

            return results.Result;
        }
    }
}
