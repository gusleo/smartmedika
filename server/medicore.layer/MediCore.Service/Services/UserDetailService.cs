using dna.core.auth;
using MediCore.Data.Entities;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using dna.core.service.Infrastructure;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using dna.core.service.Services;

namespace MediCore.Service.Services
{
    public class UserDetailService : ReadWriteService<UserDetailMediCoreModel, UserDetailMediCore>, IUserDetailService
    {


        public UserDetailService(IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork) : base(authService, unitOfWork)
        {

        }

        
        public async Task<Response<UserDetailMediCoreModel>> GetDetailByUserId(int userId)
        {
            var response = InitErrorResponse();
            var user = await _unitOfWork.UserDetailRepository.GetSingleAsync(xx => xx.UserId == userId);
            response = InitSuccessResponse(MessageConstant.Create);
            response.Item = GetModelFromEntity(user);
            return response;
        }
        public Response<UserDetailMediCoreModel> Create(UserDetailMediCoreModel modelToCreate)
        {
            var response = this.InitErrorResponse();
            if ( Validate(modelToCreate) )
            {
                var en = this.GetEntityFromModel(modelToCreate);
                _unitOfWork.UserDetailRepository.Add(this.RemoveChildEntity(en));
                _unitOfWork.Commit();

                response = this.InitSuccessResponse(MessageConstant.Create);
                response.Item = this.GetModelFromEntity(en);
            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }

            return response;
        }

        public async Task<Response<UserDetailMediCoreModel>> Delete(int id)
        {
            var response = this.InitSuccessResponse(MessageConstant.Delete);
            var en = await _unitOfWork.UserDetailRepository.GetSingleAsync(id);
            _unitOfWork.UserDetailRepository.Delete(en);
            _unitOfWork.Commit();
            return response;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<UserDetailMediCoreModel> Edit(UserDetailMediCoreModel modelToEdit)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToEdit) )
            {
                var en = _unitOfWork.UserDetailRepository.GetSingle(modelToEdit.Id);
                if(en != null )
                {
                    en.FirstName = modelToEdit.FirstName;
                    en.LastName = modelToEdit.LastName;
                    en.Longitude = modelToEdit.Longitude;
                    en.Latitude = modelToEdit.Latitude;
                    en.PatientId = modelToEdit.PatientId;
                    en.RegencyId = modelToEdit.RegencyId;
                    en.Avatar = modelToEdit.Avatar;
                    en.Address = modelToEdit.Address;
                    
                    _unitOfWork.UserDetailRepository.Edit(this.RemoveChildEntity(en));
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(en);
                }
                else
                {
                    response.Message = MessageConstant.NotFound;
                }
                               
            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }
            return response;
        }

        public async Task<Response<UserDetailMediCoreModel>> EditWithPatientAsync(UserDetailMediCoreModel model)
        {
            var response = InitErrorResponse();
            try{
                var patient = new Patient();
                if (model.PatientId.HasValue && model.PatientId > 0)
                {
                    patient = _unitOfWork.PatientRepository.GetSingle(model.PatientId ?? 0);
                    patient.PatientName = model.Patient.PatientName;
                    patient.DateOfBirth = model.Patient.DateOfBirth;
                    patient.UpdatedDate = DateTime.UtcNow;
                    patient.UpdatedById = this.GetUserId();
                    patient.Gender = model.Patient.Gender;
                    patient.AssociatedUserId = model.Patient.AssociatedUserId;
                    patient.PatientStatus = model.Patient.PatientStatus;
                    patient.RelationshipStatus = model.Patient.RelationshipStatus;
                    _unitOfWork.PatientRepository.Edit(patient);
                }
                else
                {
                    patient = Mapper.Map<Patient>(model.Patient);
                    patient.CreatedDate = patient.UpdatedDate = DateTime.UtcNow;
                    patient.UpdatedById = patient.CreatedById = this.GetUserId();
                    _unitOfWork.PatientRepository.Add(patient);
                }

                var en = new UserDetailMediCore();
                if(model.Id == 0){
                    en = new UserDetailMediCore
                    {
                        Id = 0,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        UserId = model.UserId,
                        PatientId = patient.Id,
                        RegencyId = model.RegencyId
                    };
                    _unitOfWork.UserDetailRepository.Add(en);
                }else{
                    // find the user and update properties
                    en = await _unitOfWork.UserDetailRepository.GetSingleAsync(model.Id);
                    en.FirstName = model.FirstName;
                    en.LastName = model.LastName;
                    en.RegencyId = model.RegencyId;
                    en.Address = model.Address;
                    en.UserId = model.UserId;
                    en.PatientId = patient.Id;
                    _unitOfWork.UserDetailRepository.Edit(en);
                }

                _unitOfWork.Commit();


                response = InitSuccessResponse(MessageConstant.Update);
                response.Item = GetModelFromEntity(en);
            }catch(Exception ex){
                response.Message = ex.Message;
            }


            return response;
        }

        public async Task<Response<PaginationSet<UserDetailMediCoreModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var result = await _unitOfWork.UserDetailRepository.GetAllAsync(pageIndex, pageSize);
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);

            return response;
        }

        public async Task<Response<PaginationSet<UserDetailMediCoreModel>>> GetAllWithDetailAsync(int pageIndex, int pageSize, string clue)
        {
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            var result = await _unitOfWork.UserDetailRepository.GetAllWithDetailAsync(pageIndex, pageSize, clue);
            response.Item = GetModelFromEntity(result);
            return response;
        }

        public async Task<Response<UserDetailMediCoreModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();            
            var res = await _unitOfWork.UserDetailRepository.GetSingleAsync(xx => xx.UserId == id, xx => xx.User);
            if ( res != null )
            {
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(res);
            }
            else
            {
                response.Message = MessageConstant.NotFound;
            }
                     
            return response;
        }

        public async Task<Response<UserDetailMediCoreModel>> GetSingleWithPatientAsync(int id)
        {
            var response = InitErrorResponse();
            var res = await _unitOfWork.UserDetailRepository.GetSingleDetail(id);
            if (res != null)
            {
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(res);
            }
            else
            {
                response.Message = MessageConstant.NotFound;
            }

            return response;
        }

       
        public UserDetailMediCore RemoveChildEntity(UserDetailMediCore entity)
        {
            entity.Regency = null;
            entity.User = null;
            entity.Patient = null;
            return entity;
        }

        public bool Validate(UserDetailMediCoreModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }
    }
}
