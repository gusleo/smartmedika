using dna.core.auth;
using dna.core.service.Infrastructure;
using MediCore.Data.Entities;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediCore.Service.Services
{
    public class MedicalStaffFavoriteService : ReadWriteService<MedicalStaffFavoriteModel, MedicalStaffFavorite>, IMedicalStaffFavoriteService
    {
        public MedicalStaffFavoriteService(IMediCoreUnitOfWork unitOfWork, IAuthenticationService authService) : base(authService, unitOfWork)
        {
         
        }

        public Response<MedicalStaffFavoriteModel> Create(MedicalStaffFavoriteModel modelToCreate)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToCreate) )
            {
                var en = GetEntityFromModel(modelToCreate);
                en.CreatedDate = en.UpdatedDate = DateTime.UtcNow;
                en.UserId = en.CreatedById = en.UpdatedById = GetUserId();
                _unitOfWork.MedicalStaffFavoriteRepository.Add(en);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Create);
                response.Item = GetModelFromEntity(en);

            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }
            return response;
        }

        public async Task<Response<MedicalStaffFavoriteModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.MedicalStaffFavoriteRepository.GetSingleAsync(id);
            if( en != null )
            {
                
                _unitOfWork.MedicalStaffFavoriteRepository.Delete(en);
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

        public async Task<Response<MedicalStaffFavoriteModel>> DeleteByStaffIdAsync(int medicalStaffId)
        {
            var response = InitErrorResponse();
            var userId = GetUserId();
            var en = await _unitOfWork.MedicalStaffFavoriteRepository.GetSingleAsync(x => x.UserId == userId && x.MedicalStaffId == medicalStaffId);
            if(en != null )
            {
                _unitOfWork.MedicalStaffFavoriteRepository.Delete(en);
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

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<MedicalStaffFavoriteModel> Edit(MedicalStaffFavoriteModel modelToEdit)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToEdit) )
            {
               
                var en = GetEntityFromModel(modelToEdit);
                en.UpdatedDate = DateTime.UtcNow;
                en.UpdatedById = GetUserId();
                _unitOfWork.MedicalStaffFavoriteRepository.Add(en);
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

        public async Task<Response<PaginationSet<MedicalStaffFavoriteModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                var results = await _unitOfWork.MedicalStaffFavoriteRepository.GetAllAsync(pageIndex, pageSize);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = GetModelFromEntity(results);
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<PaginationSet<MedicalStaffFavoriteModel>>> GetAllByUserAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            var results = await _unitOfWork.MedicalStaffFavoriteRepository.GetAllByUserAsync(GetUserId(), pageIndex, pageSize);
            response.Item = GetModelFromEntity(results);
            return response;
        }

        public async Task<Response<MedicalStaffFavoriteModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.MedicalStaffFavoriteRepository.GetSingleAsync(id);
            if(en != null )
            {
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(en);
            }
            else
            {
                response.Message = MessageConstant.NotFound;
            }
            return response;
        }

        public MedicalStaffFavorite RemoveChildEntity(MedicalStaffFavorite entity)
        {
            entity.User = null;
            entity.MedicalStaff = null;
            entity.CreatedByUser = null;
            entity.UpdatedByUser = null;
            return entity;
        }

        public bool Validate(MedicalStaffFavoriteModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }
    }
}
