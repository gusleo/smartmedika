using dna.core.auth;
using MediCore.Data.Entities;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.service.Infrastructure;

namespace MediCore.Service.Services
{
    public class HospitalAppointmentRatingService : ReadWriteService<HospitalAppointmentRatingModel, HospitalAppointmentRating>, IHospitalAppointmentRatingService
    {


        public HospitalAppointmentRatingService(IAuthenticationService auth, IMediCoreUnitOfWork unitOfWork) : base(auth, unitOfWork)
        {

        }

        public Response<HospitalAppointmentRatingModel> Create(HospitalAppointmentRatingModel modelToCreate)
        {
            int userId = this.GetUserId();
            var response = InitErrorResponse();
            if ( Validate(modelToCreate) )
            {
                // only user who has created the appointment
                // can give a rating
                if ( ValidateUser(modelToCreate, userId) )
                {
                    var en = GetEntityFromModel(modelToCreate);
                    en.CreatedDate = en.UpdatedDate = DateTime.UtcNow;
                    en.CreatedById = en.UpdatedById = userId;
                    _unitOfWork.HospitalAppointmentRatingRepository.Add(en);
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Create);
                    response.Item = GetModelFromEntity(en);
                }
                else
                {
                    response.Message = MessageConstant.UserNotAllowed;
                }
            }else
            {
                response.Message = MessageConstant.ValidationError;
            }
            
            return response;
        }

        private bool ValidateUser(HospitalAppointmentRatingModel model, int userId)
        {
            
            int appointmentUser = _unitOfWork.HospitalAppointmentRepository.GetHospitalAppointmentUserId(model.HospitalAppointmentId);
            return appointmentUser == userId;
        }

        public async Task<Response<HospitalAppointmentRatingModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.HospitalAppointmentRatingRepository.GetSingleAsync(id);
            if(en != null )
            {
                _unitOfWork.HospitalAppointmentRatingRepository.Delete(en);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Delete);
                response.Item = GetModelFromEntity(en);
            }else
            {
                response.Message = MessageConstant.NotFound;
            }

            return response;

        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<HospitalAppointmentRatingModel> Edit(HospitalAppointmentRatingModel modelToEdit)
        {
            int userId = this.GetUserId();
            var response = InitErrorResponse();
            if ( Validate(modelToEdit) )
            {
                if ( ValidateUser(modelToEdit, userId) )
                {
                    var en = GetEntityFromModel(modelToEdit);
                    en.UpdatedById = userId;
                    en.UpdatedDate = DateTime.Now;
                    _unitOfWork.HospitalAppointmentRatingRepository.Edit(en);
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(en);
                }
                else
                {
                    response.Message = MessageConstant.UserNotAllowed;
                }
            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }

            return response;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentRatingModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var result = await _unitOfWork.HospitalAppointmentRatingRepository.GetAllAsync(pageIndex, pageSize);
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);

            return response;
        }

        public async Task<Response<HospitalAppointmentRatingModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.HospitalAppointmentRatingRepository
                        .GetSingleAsync(x => x.Id == id, incl => incl.HospitalAppointment);

            if(en != null )
            {
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(en);
            }else
            {
                response.Message = MessageConstant.NotFound;
            }
            return response;
        }

        public HospitalAppointmentRating RemoveChildEntity(HospitalAppointmentRating entity)
        {
            entity.HospitalAppointment = null;
            return entity;
        }

        public bool Validate(HospitalAppointmentRatingModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentRatingModel>>> GetHospitalRatingAsync(int hospitalId, int pageIndex, int pageSize)
        {
            var result = await _unitOfWork.HospitalAppointmentRatingRepository.GetHospitalRatingAsync(hospitalId, pageIndex, pageSize);
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            return response;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentRatingModel>>> GetStaffRatingAsync(int staffId, int pageIndex, int pageSize)
        {
            var result = await _unitOfWork.HospitalAppointmentRatingRepository.GetStaffRatingAsync(staffId, pageIndex, pageSize);
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            return response;
        }

        public async Task<Response<IList<HospitalAppointmentRatingModel>>> GetTotalHospitalRatingAsync(int hospitalId)
        {
            var response = InitSuccessListResponse(MessageConstant.Load);
            var result = await _unitOfWork.HospitalAppointmentRatingRepository.GetTotalHospitalRatingAsync(hospitalId);
            response.Item = GetModelFromEntity(result);
            return response;
            
        }

        public async Task<Response<IList<HospitalAppointmentRatingModel>>> GetTotalStaffRatingAsync(int staffId)
        {
            var response = InitSuccessListResponse(MessageConstant.Load);
            var result = await _unitOfWork.HospitalAppointmentRatingRepository.GetTotalStaffRatingAsync(staffId);
            response.Item = GetModelFromEntity(result);
            return response;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentRatingModel>>> GetUserRatingAsync(int pageIndex, int pageSize)
        {
            var userId = GetUserId();
            var result = await _unitOfWork.HospitalAppointmentRatingRepository.GetUserRatingAsync(userId, pageIndex, pageSize);
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            return response;
        }
    }

 
}
