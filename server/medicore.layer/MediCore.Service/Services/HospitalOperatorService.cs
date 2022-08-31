using MediCore.Data.Entities;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.service.Infrastructure;
using MediCore.Data.UnitOfWork;
using dna.core.auth;
using MediCore.Data.Infrastructure;
using AutoMapper;
using dna.core.service.Services.Abstract;
using dna.core.service.Models;

namespace MediCore.Service.Services
{
    public class HospitalOperatorService : ReadWriteService<HospitalOperatorModel, HospitalOperator>, IHospitalOperatorService
    {
        public HospitalOperatorService(IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork) : base(authService, unitOfWork)
        {

        }
        public async Task<Response<HospitalOperatorModel>> Create(HospitalOperatorModel modelToCreate)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToCreate))
            {
                if ( await IsUserAssignToHospital(modelToCreate.HospitalId) )
                {
                    var en = GetEntityFromModel(modelToCreate);
                    var exist = _unitOfWork.HospitalOperatorRepository.GetSingle(x => x.UserId == en.UserId
                                    && x.HospitalId == en.HospitalId);

                    if ( exist == null )
                    {
                        en.CreatedById = en.UpdatedById = this.GetUserId();
                        en.CreatedDate = en.UpdatedDate = DateTime.UtcNow;

                        _unitOfWork.HospitalOperatorRepository.Add(this.RemoveChildEntity(en));
                        _unitOfWork.Commit();

                        response = InitSuccessResponse(MessageConstant.Create);
                        response.Item = GetModelFromEntity(en);
                    }
                    else
                    {
                        exist.UpdatedById = this.GetUserId();
                        exist.UpdatedDate = DateTime.UtcNow;
                        exist.Status = en.Status;
                        _unitOfWork.HospitalOperatorRepository.Edit(exist);
                        _unitOfWork.Commit();

                        response = InitSuccessResponse(MessageConstant.Update);
                        response.Item = GetModelFromEntity(exist);

                    }
                    
                }else
                {
                    response.Message = MessageConstant.UserNotAllowed;
                }
                
            }else
            {
                response.Message = MessageConstant.ValidationError;
            }
            
            

            return response;
        }

        public async Task<Response<HospitalOperatorModel>> Delete(int id)
        {
            var response = InitErrorResponse();           
           
            var en = await _unitOfWork.HospitalOperatorRepository.GetSingleAsync(id);
            if(en != null )
            {
                if ( await IsUserAssignToHospital(en.HospitalId) )
                {
                    en.Status = HospitalStaffStatus.InActive;
                    en.UpdatedDate = DateTime.UtcNow;
                    en.UpdatedById = this.GetUserId();

                    _unitOfWork.HospitalOperatorRepository.Edit(en);
                    _unitOfWork.Commit();

                    response = InitSuccessResponse(MessageConstant.Delete);
                    response.Item = GetModelFromEntity(en);
                }else
                {
                    response.Message = MessageConstant.UserNotAllowed;
                }
                
            }else
            {
                response.Message = MessageConstant.NotFound;
            }
            

            return response;
        }

        public async Task<Response<IList<HospitalOperatorModel>>> ReAssignUserToHospital(int userId, List<HospitalOperatorModel> hospitalOperators){
            var response = InitErrorListResponse();
            if (IsSuperAdmin())
            {
                try
                {
                    var operators = GetEntityFromModel(hospitalOperators);

                    //update operators to make sure just userId from input params
                    operators = operators.Select(xx => { 
                                                        xx.UserId = userId; 
                                                        xx.CreatedDate = xx.UpdatedDate = DateTime.UtcNow; 
                                                        xx.CreatedById = xx.UpdatedById = GetUserId();
                                                        xx.User = null;
                                                        xx.Hospital = null;
                                                        return xx; }).ToList();

                    // find exsisting associated user to hospital 
                    var existingHospital = await _unitOfWork.HospitalOperatorRepository.FindByAsync(x => x.UserId == userId);

                    // if not listed from existing hospital then add new
                    var newHospital = operators.Where(xx => !existingHospital.Select(yy => yy.HospitalId).Contains(xx.HospitalId));
                    if(newHospital.Any()){
                        _unitOfWork.HospitalOperatorRepository.AddRange(newHospital);
                        _unitOfWork.Commit();
                    }
                        

                    // if listed on existing but not listed on input hospitaloperators then deleted
                    var deletedHospital = existingHospital.Where(xx => !hospitalOperators.Select(yy => yy.HospitalId).Contains(xx.HospitalId));
                    if(deletedHospital.Any()){
                        _unitOfWork.HospitalOperatorRepository.DeleteRange(deletedHospital);
                        _unitOfWork.Commit();
                    }

                    response = InitSuccessListResponse(MessageConstant.Create);
                    response.Item = GetModelFromEntity(newHospital.ToList());


                }catch(Exception ex){
                    response.Message = ex.Message;
                }

            }else{
                response.Message = MessageConstant.UserNotAllowed;
            }

            return response;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public async Task<Response<HospitalOperatorModel>> Edit(HospitalOperatorModel modelToEdit)
        {
            var response = InitErrorResponse();           
           

            if ( Validate(modelToEdit) )
            {
                if ( await IsUserAssignToHospital(modelToEdit.HospitalId) )
                {
                    var en = GetEntityFromModel(modelToEdit);
                    en.UpdatedById = this.GetUserId();
                    en.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.HospitalOperatorRepository.Edit(this.RemoveChildEntity(en));
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Update);
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

        public async Task<Response<PaginationSet<HospitalOperatorModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
           
            var result = await _unitOfWork.HospitalOperatorRepository.GetAllAsync(pageIndex, pageSize);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<HospitalOperatorModel>>(result);
            
            return response;
        }

        public async Task<Response<PaginationSet<HospitalOperatorModel>>> GetHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, HospitalStaffStatus[] status = null, string clue = "")
        {
            var response = InitErrorResponse(pageIndex, pageSize);
                          
            var result = await _unitOfWork.HospitalOperatorRepository.GetHospitalOperatorAsync(hospitalId, pageIndex, pageSize, status, clue);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            
            return response;
        }
        public async Task<Response<PaginationSet<UserModel>>> GetNonHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, HospitalStaffStatus[] status = null, string clue = "")
        {
            var response = InitErrorResponse<UserModel>(pageIndex, pageSize);

            var result = await _unitOfWork.HospitalOperatorRepository.GetNonHospitalOperatorAsync(hospitalId, pageIndex, pageSize, status, clue);
            response = InitSuccessResponse<UserModel>(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<UserModel>>(result);

            return response;
        }

        public async Task<Response<HospitalOperatorModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.HospitalOperatorRepository.GetSingleAsync(x => x.Id == id,
                includeProperties => includeProperties.CreatedByUser, includeProperties => includeProperties.Hospital);

            response = InitSuccessResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(en);
            

            return response;
        }
        public async Task<Response<IList<HospitalOperatorModel>>> GetOperatorHospitalAsync(int userId)
        {
            var response = InitErrorListResponse<HospitalOperatorModel>();
            
            var result = await _unitOfWork.HospitalOperatorRepository.GetUserHospitalAsync(userId);
            response = InitSuccessListResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            
            return response;
        }

        public bool Validate(HospitalOperatorModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public HospitalOperator RemoveChildEntity(HospitalOperator entity)
        {
            entity.CreatedByUser = entity.UpdatedByUser = entity.User = null;
            entity.Hospital = null;
            return entity;
        }

        #region "Hide"
        Response<HospitalOperatorModel> IReadWriteService<HospitalOperatorModel, HospitalOperator>.Create(HospitalOperatorModel modelToCreate)
        {
            throw new NotImplementedException();
        }

        Response<HospitalOperatorModel> IReadWriteService<HospitalOperatorModel, HospitalOperator>.Edit(HospitalOperatorModel modelToEdit)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
