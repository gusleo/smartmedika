using dna.core.auth;
using MediCore.Data.Entities;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using dna.core.service.Infrastructure;
using MediCore.Service.Services.Abstract;
using MediCore.Data.Infrastructure;
using AutoMapper;
using System.Threading.Tasks;

namespace MediCore.Service.Services
{
    public class PatientService : ReadWriteService<PatientModel, Patient>, IPatientService
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MediCore.Service.Services.PatientService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="authService">Auth service.</param>
        public PatientService(IMediCoreUnitOfWork unitOfWork, IAuthenticationService authService)
            :base(authService, unitOfWork)
        {
           
        }

        public Response<PatientModel> Create(PatientModel modelToCreate)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToCreate) )
            {
                var userId = GetUserId();
                var en = GetEntityFromModel(modelToCreate);

                en.CreatedById = en.UpdatedById = userId;
                en.CreatedDate = en.UpdatedDate = DateTime.UtcNow;
                en.AssociatedUserId = modelToCreate.AssociatedUserId == 0 ? null :
                                            modelToCreate.AssociatedUserId;

                _unitOfWork.PatientRepository.Add(this.RemoveChildEntity(en));
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
       

        public Response<PatientModel> Edit(PatientModel modelToEdit)
        {
            var response = InitErrorResponse();

            if ( Validate(modelToEdit) )
            {
                if ( !AllowUserToEditPatient(modelToEdit.Id) )
                {
                    response.Message = MessageConstant.UserNotAllowed;

                }
                else
                {
                    var en = _unitOfWork.PatientRepository.GetSingle(modelToEdit.Id);
                    en.PatientName = modelToEdit.PatientName;
                    en.DateOfBirth = modelToEdit.DateOfBirth;
                    en.RelationshipStatus = modelToEdit.RelationshipStatus;
                    en.Gender = modelToEdit.Gender;
                    en.PatientStatus = en.PatientStatus;
                    en.UpdatedDate = DateTime.UtcNow;
                    en.UpdatedById = GetUserId();

                    _unitOfWork.PatientRepository.Edit(RemoveChildEntity(en));
                    _unitOfWork.Commit();

                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(en);
                }
            }else
            {
                response.Message = MessageConstant.ValidationError;
            }

           

           
            return response;
            
        }

        public async Task<Response<PatientModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            if ( AllowUserToEditPatient(id) )
            {
                
                int userId = GetUserId();
                var en = await _unitOfWork.PatientRepository.GetSingleAsync(id);
                en.PatientStatus = PatientStatus.InActive;

                en.UpdatedById = userId;
                en.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.PatientRepository.Edit(en);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Delete);
                response.Item = GetModelFromEntity(en);
                
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
           

            return response;
        }

        public bool Validate(PatientModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public async Task<Response<PatientModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            if ( AllowUserToEditPatient(id) )
            {
                var en = await _unitOfWork.PatientRepository.GetSingleAsync(id);
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(en);
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
                
           
            return response;
        }

        
        public async Task<Response<PaginationSet<PatientModel>>> GetAllAsync(int pageIndex, int pageSize = Constant.PageSize)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            if ( this.IsSuperAdmin() )
            {                
                var result = await _unitOfWork.PatientRepository.GetAllAsync(pageIndex, pageSize);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = GetModelFromEntity(result);
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
            
            
            
            return response;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }



        private bool AllowUserToEditPatient(int patientId)
        {           
            int userId = GetUserId();
            var patient = _unitOfWork.PatientRepository.AllowedUserToEdit(patientId, userId);
            return patient;
        }

        public async Task<Response<PaginationSet<PatientModel>>> GetUserPatientAsync(int pageIndex, int pageSize = 20, PatientStatus[] filter = null)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            var userId = this.GetUserId();            
                
            filter = filter ?? new PatientStatus[] { PatientStatus.Active, PatientStatus.Death, PatientStatus.InActive };
            IList<PatientStatus> status = new List<PatientStatus>(filter);

            var result = await _unitOfWork.PatientRepository.FindByAsync(x => x.CreatedById == userId && status.Contains(x.PatientStatus), pageIndex, pageSize);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);            
            return response;
        }

        public Patient RemoveChildEntity(Patient entity)
        {
            entity.AssociatedUser = null;
            entity.CreatedByUser = null;
            entity.UpdatedByUser = null;
            return entity;
        }

    }
}
