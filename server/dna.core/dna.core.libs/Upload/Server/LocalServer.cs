using dna.core.libs.Upload;
using dna.core.libs.Upload.Config;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace dna.core.libs.Upload.Server
{
    public class LocalServer : IServerType
    {
        private ServerConfig _config;
        public LocalServer(ServerConfig config)
        {
            _config = config;
        }
        public UploadMessage Delete(string containerName, string fileName)
        {
            throw new NotImplementedException();
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
                string containerDefault = _config.DestinationFolder.Replace("/", "\\");

                string path = rootFolder + $@"\{containerDefault}" +  $@"\{filename}";
                using ( FileStream fs = File.Create(path) )
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                message.FileName = file.FileName;
                message.GuidFileName = filename;
                message.FullPath = String.Format("{0}/{1}", _config.DestinationFolder, filename);
                message.Message = UploadMessage.SUCCESS;

            }
            catch ( Exception ex )
            {
                message.Message = ex.Message;
            }

            return message;
        }

        public UploadMessage Delete(string path)
        {
            var message = new UploadMessage();
            string[] splits = path.Split('/');
            string filename = splits[splits.Length - 1];
            path = path.Replace(filename, "");
            path = path.Replace("/", "\\");
            string[] files = Directory.GetFiles(path, filename);
            if(files.Length > 0 )
            {
                foreach(var loc in files )
                {
                    File.Delete(loc);
                }
                message.Message = "Success";
            }else
            {
                message.Message = "Not Found";
            }

            return message;
        }

        public async Task<UploadMessage> DeleteAsync(string path)
        {
            return Delete(path);
        }
    }
}
