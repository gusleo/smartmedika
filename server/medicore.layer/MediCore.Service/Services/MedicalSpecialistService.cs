using System.Collections.Generic;
using dna.core.service.Infrastructure;
using MediCore.Service.Model;
using MediCore.Data.UnitOfWork;
using MediCore.Data.Entities;
using AutoMapper;
using dna.core.auth;
using MediCore.Service.Services.Abstract;
using System.Threading.Tasks;

namespace MediCore.Service.Services
{
    public class MedicalSpecialistService : ReadWriteService<MedicalStaffSpecialistModel, MedicalStaffSpecialist>,
        IMedicalSpecialistService
    {
        
        public MedicalSpecialistService(IMediCoreUnitOfWork unitOfWork, IAuthenticationService authService) : base(authService, unitOfWork)
        {
            
        }     
        public bool AllowUserToEdit(MedicalStaffSpecialistModel modelToEdit)
        {
            return IsSuperAdmin();
        }

        public Response<MedicalStaffSpecialistModel> Create(MedicalStaffSpecialistModel modelToCreate)
        {
            if ( Validate(modelToCreate) )
            {
                var en = GetEntityFromModel(modelToCreate);
                _unitOfWork.MedicalStaffSpecialistRepository.Add(this.RemoveChildEntity(en));
                _unitOfWork.Commit();

                return new Response<MedicalStaffSpecialistModel>(true, MessageConstant.Create)
                {
                    Item = GetModelFromEntity(en)
                };
            }

            return new Response<MedicalStaffSpecialistModel>(false, MessageConstant.Error);
        }
        
        public Response<IList<MedicalStaffSpecialistModel>> CreateRange(List<MedicalStaffSpecialistModel> listOfData)
        {

            var response = InitErrorListResponse();            
            
            
            IList<MedicalStaffSpecialist> data =
                Mapper.Map<IList<MedicalStaffSpecialistModel>, IList<MedicalStaffSpecialist>>(listOfData);

            _unitOfWork.MedicalStaffSpecialistRepository.AddRange(data);
            _unitOfWork.Commit();
            response = InitSuccessListResponse(MessageConstant.Create);
            
            return response;
            
        }

        public async Task<Response<MedicalStaffSpecialistModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            
            var en = await _unitOfWork.MedicalStaffSpecialistRepository.GetSingleAsync(id);
            if(en != null )
            {
                _unitOfWork.MedicalStaffSpecialistRepository.Delete(en);
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

        public Response<MedicalStaffSpecialistModel> Edit(MedicalStaffSpecialistModel modelToEdit)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToEdit) )
            {
                if ( AllowUserToEdit(modelToEdit) )
                {
                    var en = GetEntityFromModel(modelToEdit);                   
                    _unitOfWork.MedicalStaffSpecialistRepository.Edit(this.RemoveChildEntity(en));
                    _unitOfWork.Commit();

                    response = InitSuccessResponse(MessageConstant.Update);
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

       
        public async Task<Response<MedicalStaffSpecialistModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();           
            var en = await _unitOfWork.MedicalStaffSpecialistRepository.GetSingleAsync(id);
            response = InitSuccessResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(en);
            
            return response;
        }

        public async Task<Response<PaginationSet<MedicalStaffSpecialistModel>>> GetAllAsync(int pageIndex, int pageSize = Constant.PageSize)
        {
            var response = InitErrorResponse(pageIndex, pageSize);            
            var result = await _unitOfWork.MedicalStaffSpecialistRepository.GetAllAsync(pageIndex, pageSize);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<MedicalStaffSpecialistModel>>(result);
           
            return response;
            
        }
       
        
        public bool Validate(MedicalStaffSpecialistModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public MedicalStaffSpecialist RemoveChildEntity(MedicalStaffSpecialist entity)
        {
            entity.PolyClinic = null;
            return entity;
        }
    }
}
