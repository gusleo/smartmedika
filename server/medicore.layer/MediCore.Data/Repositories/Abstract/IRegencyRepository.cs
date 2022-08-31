using dna.core.data.Infrastructure;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories.Abstract
{
    public interface IRegencyRepository : IReadBaseRepository<Regency>, IWriteBaseRepository<Regency>
    {
        IList<Regency> GetRegencyRegionByClue(string clue);
        Task<PaginationEntity<Regency>> GetRegencyDetailByCountry(int countryId, Status[] Status, int page, int pageSize = 20, string clue = "");
    }
}
