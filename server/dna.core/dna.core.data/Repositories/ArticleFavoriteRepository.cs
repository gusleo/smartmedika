using dna.core.data.Entities;
using dna.core.data.Infrastructure;
using dna.core.data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace dna.core.data.Repositories
{
    public class ArticleFavoriteRepository: EntityReadWriteBaseRepository<ArticleFavorite>, IArticleFavoriteRepository
    {
        public IDnaCoreContext DnaCoreContext
        {
            get { return _context as IDnaCoreContext; }
        }

        public ArticleFavoriteRepository(IDnaCoreContext context) : base(context)
        {

        }

        public async Task<IList<ArticleFavorite>> GetAllUserFavoriteArticleIds(int userId)
        {
            var results = (from a in DnaCoreContext.ArticleFavorite where a.UserId == userId select new ArticleFavorite { ArticleId = a.ArticleId });
            return await results.ToListAsync();
        }

        public async Task<PaginationEntity<ArticleFavorite>> GetUserFavoriteArticleAsync(int pageIndex, int pageSize, int userId)
        {
            ArticleStatus[] status = new ArticleStatus[]{ ArticleStatus.Confirmed};

            var result = (from f in DnaCoreContext.ArticleFavorite
                          join a in DnaCoreContext.Article on f.ArticleId equals a.Id
                          join c in DnaCoreContext.ArticleCategory on a.CategoryId equals c.Id
                          join user in DnaCoreContext.UserDetail on a.CreatedById equals user.UserId
                          where status.Contains(a.Status) && c.IsVisible == true
                          orderby f.CreatedDate descending
                          select new ArticleFavorite
                          {
                              Id = f.Id,
                              Article = new Article
                              {
                                  Id = a.Id,
                                  ShortDescription = a.ShortDescription,
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
                              }
                          });
        
            return new PaginationEntity<ArticleFavorite>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
    }
    public interface IArticleFavoriteRepository : IWriteBaseRepository<ArticleFavorite>, IReadBaseRepository<ArticleFavorite>
    {
        Task<PaginationEntity<ArticleFavorite>> GetUserFavoriteArticleAsync(int pageIndex, int pageSize, int userId);
        Task<IList<ArticleFavorite>> GetAllUserFavoriteArticleIds(int userId);
    }
}
