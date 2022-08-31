using dna.core.data.Infrastructure;
using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public interface IRegencyService : IReadWriteService<RegencyModel, Regency>
    {
        Task<Response<IList<RegencyModel>>> GetRegencyByRegionAsync(int regionId);
        Task<Response<PaginationSet<RegencyModel>>> GetRegencyDetailByCountryAsync(int countryId, Status[] Status, int page, int pageSize = Constant.PageSize, string clue = "");
        Task<Response<RegencyModel>> ChangeRegencyStatusAsync(int id, Status status);
    }
}
