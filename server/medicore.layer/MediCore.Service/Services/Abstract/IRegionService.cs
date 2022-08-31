using dna.core.data.Infrastructure;
using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public interface IRegionService : IReadWriteService<RegionModel, Region>
    {
        Task<Response<RegionModel>> ChangeRegionStatusAsync(int id, Status status);
        Task<Response<PaginationSet<RegionModel>>> GetAllRegionByClueAsync(Status[] status, int page, int pageSize = 20, string clue = "");

        Task<Response<PaginationSet<RegionModel>>> GetRegionByCountryAndClueAsync(int countryId, Status[] status, int page, int pageSize = 20, string clue = "");

        /// <summary>
        /// Return list of region by country with/without child regency
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="includeRegency"></param>
        /// <returns></returns>
        Task<Response<IList<RegionModel>>> GetRegionByCountryAsync(int countryId, bool includeRegency = false);


    }
}
