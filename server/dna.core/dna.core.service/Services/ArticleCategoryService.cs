using dna.core.data.Entities;
using dna.core.service.Models;
using dna.core.service.Services;
using dna.core.service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.service.Infrastructure;
using dna.core.data.UnitOfWork;
using AutoMapper;
using dna.core.auth;
using Microsoft.AspNetCore.Http;

namespace dna.core.service.Services
{
    public class ArticleCategoryService : ReadWriteServiceBase<ArticleCategoryModel, ArticleCategory>, IArticleCategoryService
    {
        private readonly IDNAUnitOfWork _unitOfWork;
        private readonly IFileServices _fileServices;

        public ArticleCategoryService(IAuthenticationService authService, IDNAUnitOfWork unitOfWork, IFileServices fileServices) : base (authService)
        {
            _unitOfWork = unitOfWork;
            _fileServices = fileServices;
        }
        public Response<ArticleCategoryModel> Create(ArticleCategoryModel modelToCreate)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToCreate) )
            {
                try
                {
                    var en = GetEntityFromModel(modelToCreate);
                    _unitOfWork.ArticleCategoryRepository.Add(en);
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Create);
                    response.Item = GetModelFromEntity(en);
                }
                catch(Exception ex)
                {
                    response.Message = ex.Message;
                }
                
            }else
            {
                response.Message = MessageConstant.ValidationError;
            }
            return response;
        }

        public async Task<Response<ArticleCategoryModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            try
            {
                var en = await _unitOfWork.ArticleCategoryRepository.GetSingleAsync(id);
                _unitOfWork.ArticleCategoryRepository.Delete(en);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Delete);
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<ArticleCategoryModel> Edit(ArticleCategoryModel modelToEdit)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToEdit) )
            {
                                  
                var en = GetEntityFromModel(modelToEdit);
                _unitOfWork.ArticleCategoryRepository.Edit(en);
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

        public async Task<Response<PaginationSet<ArticleCategoryModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                var categories = await _unitOfWork.ArticleCategoryRepository.GetAllAsync(pageIndex, pageSize, true);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = Mapper.Map<PaginationSet<ArticleCategoryModel>>(categories);
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<PaginationSet<ArticleCategoryModel>>> GetCategoryWithCountAsync(int pageIndex, int pageSize, bool onlyVisibleCategory)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                var categories = await _unitOfWork.ArticleCategoryRepository.GetCategoryWithCountAsync(pageIndex, pageSize, onlyVisibleCategory);
                response = InitSuccessResponse( pageIndex, pageSize, MessageConstant.Load );
                response.Item = GetModelFromEntity(categories);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<ArticleCategoryModel>> GetArticleCountAsync(int id)
        {
            var response = InitErrorResponse();
            try
            {
                var en = await _unitOfWork.ArticleCategoryRepository.GetSingleAsync(id);
                int count = _unitOfWork.ArticleCategoryRepository.GetArticleCount(id);
                var model = GetModelFromEntity(en);
                model.ArticleCount = count;
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = model;
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
            

        }

        public async Task<Response<ArticleCategoryModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            try
            {
                var en = await _unitOfWork.ArticleCategoryRepository.GetSingleAsync(id);
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(en); 
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public ArticleCategory RemoveChildEntity(ArticleCategory entity)
        {
            throw new NotImplementedException();
        }

        public bool Validate(ArticleCategoryModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public async Task<Response<ArticleCategoryModel>> UploadCoverImageAsync(int Id, IFormFile file)
        {
            var response = InitErrorResponse();
            var category = await _unitOfWork.ArticleCategoryRepository.GetSingleAsync(Id);
            if(category != null )
            {
                if( !String.IsNullOrEmpty(category.Image) )
                {
                    // delete existing image
                    await _fileServices.DeleteAsync(category.Image);
                }

                var coverImage = await _fileServices.UploadAsync(file);
                if ( coverImage.Success )
                {
                    category.Image = coverImage.Item.Path;
                    _unitOfWork.ArticleCategoryRepository.Edit(category);
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(category);
                }
            }

            return response;
        }
    }
}
