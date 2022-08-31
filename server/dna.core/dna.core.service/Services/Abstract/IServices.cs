using dna.core.auth.Entity;
using dna.core.data.Entities;
using dna.core.data.Infrastructure;
using dna.core.libs.TreeMenu;
using dna.core.service.Infrastructure;
using dna.core.service.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dna.core.service.Services.Abstract
{
    public interface IArticleService : IReadWriteService<ArticleModel, Article>
    {
        Task<Response<ArticleModel>> GetSingleDetailAsync(int id);
        Task<Response<ArticleModel>> ChangeStatusAsync(int id, ArticleStatus status);
        Task<Response<PaginationSet<ArticleModel>>> GetNewestArticleAsync(List<ArticleStatus> status, int pageIndex, int pageSize, int categotyId = 0);
        Task<Response<IList<ArticleImageModel>>> UploadArticleImageAsync(int id, IList<IFormFile> files, bool isPrimary);
        Task<Response<ArticleImageModel>> UploadArticleImageCoverAsync(int id, IFormFile file);
        Task<Response<PaginationSet<ArticleModel>>> FindByStatus(int pageIndex, int pageSize, ArticleStatus[] status);
        Task<Response<PaginationSet<ArticleModel>>> GetArticleByStaff(int staffId, int pageIndex, int pageSize, ArticleStatus[] status);
    }

    public interface ITreeMenuService : IReadWriteService<TreeMenuModel, TreeMenu>
    {
        Task<Response<IList<MenuItem>>> GetMenuByTypeAsync(MenuType type);
        Task<Response<IList<TreeMenuModel>>> GetAllParentMenuAsync(MenuType type);
        Task<Response<PaginationSet<TreeMenuModel>>> GetMenuByTypeAsync(MenuType type, int pageIndex, int pageSize);

    }
    public interface IArticleCategoryService : IReadWriteService<ArticleCategoryModel, ArticleCategory>
    {
        Task<Response<ArticleCategoryModel>> GetArticleCountAsync(int id);
        Task<Response<PaginationSet<ArticleCategoryModel>>> GetCategoryWithCountAsync(int pageIndex, int pageSize, bool onlyVisibleCategory);
        Task<Response<ArticleCategoryModel>> UploadCoverImageAsync(int Id, IFormFile file);
    }
    public interface IArticleFavoriteService: IReadWriteService<ArticleFavoriteModel, ArticleFavorite>
    {
        Task<Response<PaginationSet<ArticleFavoriteModel>>> GetUserFavoriteArticleAsync(int pageIndex, int pageSize);
        Task<Response<IList<ArticleFavoriteModel>>> GetAllUserFavoriteArticleIds();
        Task<Response<ArticleFavoriteModel>> DeleteByArticle(int articleId);
    }
    public interface IImageService : IReadWriteService<ImageModel, Image>
    {
        Task<Response<ImageModel>> UploadImageAsync(IFormFile file, bool isPrimary);
        Task<Response<IList<ImageModel>>> UploadImageAsync(IList<IFormFile> files, bool isPrimary);
    }

    public interface IErrorLogService : IReadWriteService<ErrorLogModel, ErrorLog> { }

    public interface IAdvertisingService : IReadWriteService<AdvertisingModel, Advertising>
    {
        Task<Response<IList<AdvertisingModel>>> GetAdvertisingByTypeAndStatusAsync(AdvertisingType[] types, Status[] status);
    }
    public interface IFirebaseUserMapUserService : IReadWriteService<FirebaseUserMapModel, FirebaseUserMap>
    {
      
    }

    public interface IUserService: IDisposable
    {
        bool IsUserPhoneNumberExist(string phoneNumber);
    }
}
