using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using dna.core.service.Infrastructure;
using MediCore.Data.UnitOfWork;
using MediCore.Data.Entities;
using AutoMapper;
using MediCore.Service.Services.Abstract;
using dna.core.auth;
using System.Threading.Tasks;
using System.Linq;

namespace MediCore.Service.Services
{
    public class PolyClinicService : ReadWriteService<PolyClinicModel, PolyClinic>, IPolyClinicService
    {
       
        

        public PolyClinicService(IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork) : base(authService, unitOfWork)
        {

        }

        public Response<PolyClinicModel> AddSpecialistToPolyClinic(int polyClinicId, int[] specialistIds)
        {

            var response = InitErrorResponse();
            try
            {
                //create entities
                IList<PolyClinicSpesialistMap> maps = new List<PolyClinicSpesialistMap>();
                foreach ( var specialistId in specialistIds )
                {
                    maps.Add(new PolyClinicSpesialistMap
                    {
                        PolyClinicId = polyClinicId,
                        MedicalStaffSpecialistId = specialistId
                    });
                }

                //insert then commit
                _unitOfWork.PolyClinicSpecialistMapRepository.AddRange(maps);
                _unitOfWork.Commit();

                response = InitSuccessResponse(MessageConstant.Create);
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }

            return response;
                
        }

        public bool AllowUserToEdit(PolyClinicModel modelToEdit)
        {
            throw new NotImplementedException();
        }

        public Response<PolyClinicModel> Create(PolyClinicModel modelToCreate)
        {
            //init response
            var response = InitErrorResponse();

            var en = GetEntityFromModel(modelToCreate);
            _unitOfWork.PolyClinicRepository.Add(en);
            _unitOfWork.Commit();
            
            if(en.Id > 0 )
            {
                response.Success = true;
                response.Message = MessageConstant.Create;
                response.Item = GetModelFromEntity(en);
            }

            return response;
        }

        public Response<IList<PolyClinicModel>> CreateRange(List<PolyClinicModel> listOfData)
        {
            var response = InitErrorListResponse();
           
            var data = GetEntityFromModel(listOfData);
            _unitOfWork.PolyClinicRepository.AddRange(data);
            _unitOfWork.Commit();
            response = InitSuccessListResponse(MessageConstant.Create);
            response.Item = GetModelFromEntity(data);
            
            return response;
            
        }

        public async Task<Response<PolyClinicModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            
                //find delete and commit
                var en = await _unitOfWork.PolyClinicRepository.GetSingleAsync(id);
            if(en != null )
            {
                _unitOfWork.PolyClinicRepository.Delete(en);
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

        public Response<PolyClinicModel> Edit(PolyClinicModel modelToEdit)
        {
            var response = InitErrorResponse();
            try
            {
                var en = GetEntityFromModel(modelToEdit);
                _unitOfWork.PolyClinicRepository.Edit(en);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Update);
                response.Item = GetModelFromEntity(en);
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        

        public async Task<Response<PolyClinicModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();            
            var res = await _unitOfWork.PolyClinicRepository.GetSingleAsync(id);
            response = InitSuccessResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(res);            
            return response;
        }
        
        public async Task<Response<PaginationSet<PolyClinicModel>>> GetAllAsync(int pageIndex, int pageSize)
        {
         
            var result = await _unitOfWork.PolyClinicRepository.GetAllAsync(pageIndex, pageSize);
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            
            return response;
        }
        public async Task<Response<IList<PolyClinicModel>>> GetByClueAsync(string clue)
        {
           
            var result =  await _unitOfWork.PolyClinicRepository.FindByAsync(x => x.Name.Contains(clue));
            var response = InitSuccessListResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(result.ToList());
            return response;
        }

        public bool Validate(PolyClinicModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public PolyClinic RemoveChildEntity(PolyClinic entity)
        {
            return entity;
        }

        
    }
}
