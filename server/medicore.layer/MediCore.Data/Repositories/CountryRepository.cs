using dna.core.data.Infrastructure;
using dna.core.data.Repositories;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using MediCore.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories
{
    public class CountryRepository : EntityReadWriteBaseRepository<Country>, ICountryRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public CountryRepository(IMediCoreContext context) : base(context)
        {
        }

        public async Task<PaginationEntity<Country>> GetCountryByStatusAsync(Status[] status, int pageIndex, int pageSize)
        {
            var result = (from c in MediCoreContext.Country
                          where status.Contains(c.Status)
                          orderby c.Name
                          select c);
            return new PaginationEntity<Country>
            {
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(),
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync()
            };
        }
    }

    
}
