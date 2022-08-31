using dna.core.data.Repositories;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.data.Infrastructure;
using MediCore.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Data.Repositories
{
    public class RegencyRepository : EntityReadWriteBaseRepository<Regency>, IRegencyRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public RegencyRepository(IMediCoreContext context) : base(context)
        {
        }

        public IList<Regency> GetRegencyRegionByClue(string clue)
        {
            //NOTE: we can use simple FindBy then Include, 
            //but it make showing lazy loading on region -> regencys

            var query = (from c in MediCoreContext.Regency
                         join r in MediCoreContext.Region on c.RegionId equals r.Id
                         where c.Name.Contains(clue) || r.Name.Contains(clue)
                         select new Regency
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Status = c.Status,
                             RegionId = c.RegionId,
                             Region = new Region
                             {
                                 Id = r.Id,
                                 Name = r.Name,
                                 Status = r.Status,
                                 CountryId = r.CountryId
                             }
                         });
            return query.ToList();
        }

        public async Task<PaginationEntity<Regency>> GetRegencyDetailByCountry(int countryId, Status[] Status, int page, int pageSize = 20, string clue = "")
        {
            var query = (from c in MediCoreContext.Regency
                         join r in MediCoreContext.Region on c.RegionId equals r.Id
                         where r.CountryId == countryId && Status.Contains(c.Status)
                            && (c.Name.Contains(clue) || r.Name.Contains(clue))
                         select new Regency
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Status = c.Status,
                             RegionId = c.RegionId,
                             Longitude = c.Longitude,
                             Latitude = c.Latitude,
                             Region = new Region
                             {
                                 Id = r.Id,
                                 Name = r.Name,
                                 Status = r.Status,
                                 CountryId = r.CountryId
                             }
                         });


            return new PaginationEntity<Regency>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = query.Count(),
                Items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
    }

    
}
