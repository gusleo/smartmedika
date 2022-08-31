using System;
using System.Threading.Tasks;
using AutoMapper;
using dna.core.auth;
using dna.core.data.Infrastructure;
using dna.core.service.Infrastructure;
using MediCore.Data.Entities;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;

namespace MediCore.Service.Services
{
    public class UTCTimeBaseService : ReadWriteService<UTCTimeBaseModel, UTCTimeBase>, IUTCTimeBaseService
    {
        public UTCTimeBaseService(IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork) : base(authService, unitOfWork)
        {

        }
        public Response<UTCTimeBaseModel> Create(UTCTimeBaseModel modelToCreate)
        {
            var response = InitErrorResponse();

            var en = GetEntityFromModel(modelToCreate);
            _unitOfWork.UTCTimeBaseRepository.Add(this.RemoveChildEntity(en));
            _unitOfWork.Commit();
            response = InitSuccessResponse(MessageConstant.Create);
            response.Item = GetModelFromEntity(en);

            return response;


        }

        public async Task<Response<UTCTimeBaseModel>> Delete(int id)
        {
            var response = InitErrorResponse();

            var en = await _unitOfWork.UTCTimeBaseRepository.GetSingleAsync(id);
            en.Status = Status.InActive;
            _unitOfWork.UTCTimeBaseRepository.Edit(this.RemoveChildEntity(en));
            _unitOfWork.Commit();
            response = InitSuccessResponse(MessageConstant.Delete);
            response.Item = GetModelFromEntity(en);

            return response;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<UTCTimeBaseModel> Edit(UTCTimeBaseModel modelToEdit)
        {
            var response = InitErrorResponse();
            var en = GetEntityFromModel(modelToEdit);
            _unitOfWork.UTCTimeBaseRepository.Edit(this.RemoveChildEntity(en));
            _unitOfWork.Commit();
            response = InitSuccessResponse(MessageConstant.Update);
            response.Item = GetModelFromEntity(en);

            return response;
        }

        public async Task<Response<PaginationSet<UTCTimeBaseModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitErrorResponse(pageIndex, pageSize);

            var result = await _unitOfWork.UTCTimeBaseRepository.GetAllAsync(pageIndex, pageSize);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<UTCTimeBaseModel>>(result);

            return response;
        }




        public async Task<Response<UTCTimeBaseModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();

            var en = await _unitOfWork.UTCTimeBaseRepository.GetSingleAsync(id);
            response = InitSuccessResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(en);


            return response;
        }

        public bool Validate(UTCTimeBaseModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }
        public UTCTimeBase RemoveChildEntity(UTCTimeBase entity)
        {
            return entity;
        }
    }
}
