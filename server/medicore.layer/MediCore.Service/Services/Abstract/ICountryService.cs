using dna.core.data.Infrastructure;
using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Service.Model;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public interface ICountryService : IReadWriteService<CountryModel, Country>
    {
        

        /// <summary>
        /// Get country by status and pagination
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="includeInActive"></param>
        /// <returns></returns>
        Task<Response<PaginationSet<CountryModel>>> GetCountryByStatusAsync(int pageIndex, int pageSize = Constant.PageSize, bool includeInActive = false);

        /// <summary>
        /// Change country status
        /// </summary>
        /// <param name="id">Current country id</param>
        /// <param name="status">enum value <see cref="Status"/></param>
        /// <returns></returns>
        Task<Response<CountryModel>> ChangeCountryStatusAsync(int id, Status status);

        /// <summary>
        /// Get all UTC Time
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Response<PaginationSet<UTCTimeBaseModel>>> GetAllUtcByCountryAsync(int countryId, Status[] status, int page, int pageSize = Constant.PageSize);
    }
}
