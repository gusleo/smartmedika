using dna.core.data.Entities;
using dna.core.data.Infrastructure;
using dna.core.data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace dna.core.data.Repositories
{
    public class ArticleRepository : EntityReadWriteBaseRepository<Article>, IArticleRepository
    {
        public IDnaCoreContext DnaCoreContext
        {
            get { return _context as IDnaCoreContext; }
        }
        public ArticleRepository(IDnaCoreContext context) : base(context)
        {
        }

        public async Task<PaginationEntity<Article>> GetNewestArticleAsync(int pageIndex, int pageSize, ArticleStatus[] status, int categoryId = 0, int staffId = 0)
        {
            var result = (from a in DnaCoreContext.Article
                          join c in DnaCoreContext.ArticleCategory on a.CategoryId equals c.Id
                          join user in DnaCoreContext.UserDetail on a.CreatedById equals user.UserId
                          where status.Contains(a.Status) && c.IsVisible == true 
                          orderby a.CreatedDate descending
                          select new Article
                          {
                              Id = a.Id,
                              ShortDescription = a.ShortDescription,
                              Slug = a.Slug,
                              Title = a.Title,
                              CategoryId = a.CategoryId,
                              CreatedById = a.CreatedById,
                              CreatedDate = a.CreatedDate,
                              Author = new UserDetail{
                                  FirstName = user.FirstName,
                                  LastName = user.LastName
                              },
                              Category = new ArticleCategory
                              {
                                  Id = c.Id,
                                  Name = c.Name
                              },
                              ImageMaps = (from i in DnaCoreContext.Image
                                           join map in DnaCoreContext.ArticleImage on i.Id equals map.ImageId
                                           where map.ArticleId == a.Id && i.IsPrimary == true
                                           select new ArticleImage
                                           {
                                               Id = map.Id,
                                               Image = new Image
                                               {
                                                   Id = i.Id,
                                                   FileName = i.FileName,
                                                   FileExtension = i.FileExtension,
                                                   ImageUrl = i.ImageUrl,
                                                   IsPrimary = true
                                               }
                                           }).ToList()
                          });

            if( categoryId > 0){
                result = result.Where(x => x.CategoryId == categoryId);
            }

            if(staffId > 0 )
            {
                result = result.Where(x => x.CreatedById == staffId);
            }
            return new PaginationEntity<Article>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public async Task<Article> GeArticleDetailAsync(int id)
        {
            var result = (from a in DnaCoreContext.Article
                          join c in DnaCoreContext.ArticleCategory on a.CategoryId equals c.Id
                          join user in DnaCoreContext.UserDetail on a.CreatedById equals user.UserId
                          where a.Id == id
                          orderby a.CreatedDate descending
                          select new Article
                          {
                              Id = a.Id,
                              ShortDescription = a.ShortDescription,
                              Description = a.Description,
                              Slug = a.Slug,
                              Title = a.Title,
                              CategoryId = a.CategoryId,
                              CreatedById = a.CreatedById,
                              CreatedDate = a.CreatedDate,
                              Author = new UserDetail
                              {
                                  FirstName = user.FirstName,
                                  LastName = user.LastName
                              },
                              Category = new ArticleCategory
                              {
                                  Id = c.Id,
                                  Name = c.Name
                              },
                              ImageMaps = (from i in DnaCoreContext.Image
                                           join map in DnaCoreContext.ArticleImage on i.Id equals map.ImageId
                                           where map.ArticleId == a.Id orderby i.IsPrimary == true
                                           select new ArticleImage
                                           {
                                               Id = map.Id,
                                               Image = new Image
                                               {
                                                   Id = i.Id,
                                                   ImageUrl = i.ImageUrl
                                                  
                                               }
                                           }).ToList()
                          });

            return await result.FirstOrDefaultAsync();

        }
    }






    #region IArticleRepository
    public interface IArticleRepository: IWriteBaseRepository<Article>, IReadBaseRepository<Article>{
        Task<PaginationEntity<Article>> GetNewestArticleAsync(int pageIndex, int pageSize, ArticleStatus[] status, int categoryId = 0, int staffId = 0);
        Task<Article> GeArticleDetailAsync(int id);
    }
    #endregion
}
