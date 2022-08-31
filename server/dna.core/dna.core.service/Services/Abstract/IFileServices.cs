using dna.core.libs.Stream;
using dna.core.libs.Stream.Option;
using dna.core.service.Infrastructure;
using dna.core.service.JsonModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dna.core.service.Services
{
    public interface IFileServices
    {
        List<T> ImportData<T>(IFormFile file, StreamAdvanceOption option = null) where T : class, IStreamEntity, new();
        Task<Response<FileModel>> UploadAsync(IFormFile file);
        Task<Response<IList<FileModel>>> UploadAsync(IList<IFormFile> files);
        Response<FileModel> Delete(string path);
        Task<Response<FileModel>> DeleteAsync(string path);
    }
}
