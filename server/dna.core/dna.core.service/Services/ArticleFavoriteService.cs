using AutoMapper;
using dna.core.auth;
using dna.core.data.Entities;
using dna.core.data.Infrastructure;
using dna.core.data.UnitOfWork;
using dna.core.service.Infrastructure;
using dna.core.service.Models;
using dna.core.service.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dna.core.service.Services
{
    public class ArticleFavoriteService : ReadWriteServiceBase<ArticleFavoriteModel, ArticleFavorite>, IArticleFavoriteService
    {
        private readonly IDNAUnitOfWork _unitOfWork;
      

        public ArticleFavoriteService(IAuthenticationService authService, IDNAUnitOfWork unitOfWork) : base(authService)
        {
            _unitOfWork = unitOfWork;
        }

        public Response<ArticleFavoriteModel> Create(ArticleFavoriteModel modelToCreate)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToCreate) )
            {
                try
                {
                    var en = GetEntityFromModel(modelToCreate);
                    en.UserId = en.CreatedById = en.UpdatedById = GetUserId();
                    en.CreatedDate = en.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.ArticleFavoriteRepository.Add(en);
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Create);
                    response.Item = GetModelFromEntity(en);
                }
                catch ( Exception ex )
                {
                    response.Message = ex.Message;
                }

            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }
            return response;
        }

        public async Task<Response<ArticleFavoriteModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.ArticleFavoriteRepository.GetSingleAsync(id);
            if( en != null )
            {
                _unitOfWork.ArticleFavoriteRepository.Delete(en);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Delete);
                response.Item = GetModelFromEntity(en);
            }
            else
            {
                response.Message = MessageConstant.NotFound;
            }

            return response;
           
        }

        public async Task<Response<ArticleFavoriteModel>> DeleteByArticle(int articleId)
        {
            var reseponse = InitErrorResponse();
            var userId = GetUserId();
            var en = await _unitOfWork.ArticleFavoriteRepository.GetSingleAsync(x => x.ArticleId == articleId && x.UserId == userId);
            if(en != null){
                _unitOfWork.ArticleFavoriteRepository.Delete(en);
                _unitOfWork.Commit();
                reseponse = InitSuccessResponse(MessageConstant.Delete);
                reseponse.Item = GetModelFromEntity(en);

            }else{
                reseponse.Message = MessageConstant.NotFound;
            }
            return reseponse;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<ArticleFavoriteModel> Edit(ArticleFavoriteModel modelToEdit)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToEdit) )
            {

                var en = GetEntityFromModel(modelToEdit);
                _unitOfWork.ArticleFavoriteRepository.Edit(en);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Update);
                response.Item = GetModelFromEntity(en);


            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }
            return response;
        }

        public async Task<Response<PaginationSet<ArticleFavoriteModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                var articles = await _unitOfWork.ArticleFavoriteRepository.GetAllAsync(pageIndex, pageSize);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = Mapper.Map<PaginationSet<ArticleFavoriteModel>>(articles);
            }
            catch ( Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<IList<ArticleFavoriteModel>>> GetAllUserFavoriteArticleIds()
        {
            var response = InitErrorListResponse();
            try
            {
                var userId = GetUserId();
                var categories = await _unitOfWork.ArticleFavoriteRepository.GetAllUserFavoriteArticleIds(userId);
                response = InitSuccessListResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(categories);
            }
            catch ( Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<ArticleFavoriteModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            try
            {
                var en = await _unitOfWork.ArticleFavoriteRepository.GetSingleAsync(id);
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(en);
            }
            catch ( Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<PaginationSet<ArticleFavoriteModel>>> GetUserFavoriteArticleAsync(int pageIndex, int pageSize)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                var userId = GetUserId();
                var categories = await _unitOfWork.ArticleFavoriteRepository.GetUserFavoriteArticleAsync(pageIndex, pageSize, userId);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = GetModelFromEntity(categories);
            }
            catch ( Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public ArticleFavorite RemoveChildEntity(ArticleFavorite entity)
        {
            throw new NotImplementedException();
        }

        public bool Validate(ArticleFavoriteModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }
    }
}
