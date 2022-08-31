using MediCore.Data.Entities;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.service.Infrastructure;
using dna.core.auth;
using MediCore.Data.UnitOfWork;
using AutoMapper;
using dna.core.data.Infrastructure;

namespace MediCore.Service.Services
{
    public class CountryService : ReadWriteService<CountryModel, Country>, ICountryService
    {
        public CountryService(IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork) : base(authService, unitOfWork)
        {

        }
        public Response<CountryModel> Create(CountryModel modelToCreate)
        {
            var response = InitErrorResponse();
            
            var en = GetEntityFromModel(modelToCreate);
            _unitOfWork.CountryRepository.Add(this.RemoveChildEntity(en));
            _unitOfWork.Commit();
            response = InitSuccessResponse(MessageConstant.Create);
            response.Item = GetModelFromEntity(en);
            
            return response;
            

        }

        public async Task<Response<CountryModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            
            var en = await _unitOfWork.CountryRepository.GetSingleAsync(id);
            en.Status = Status.InActive;
            _unitOfWork.CountryRepository.Edit(this.RemoveChildEntity(en));
            _unitOfWork.Commit();
            response = InitSuccessResponse(MessageConstant.Delete);
            response.Item = GetModelFromEntity(en);
            
            return response;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<CountryModel> Edit(CountryModel modelToEdit)
        {
            var response = InitErrorResponse();
            //var current = _unitOfWork.CountryRepository.GetSingle(modelToEdit.Id);
            var en = GetEntityFromModel(modelToEdit);
            _unitOfWork.CountryRepository.Edit(this.RemoveChildEntity(en));
            _unitOfWork.Commit();
            response = InitSuccessResponse(MessageConstant.Update);
            response.Item = GetModelFromEntity(en);
            
            return response;
        }

        public async Task<Response<PaginationSet<CountryModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
           
            var result = await _unitOfWork.CountryRepository.GetAllAsync(pageIndex, pageSize);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<CountryModel>>(result);
            
            return response;
        }

        

        
        public async Task<Response<CountryModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
           
            var en = await _unitOfWork.CountryRepository.GetSingleAsync(id);                
            response = InitSuccessResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(en);
            
            
            return response;
        }

        public bool Validate(CountryModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public async Task<Response<PaginationSet<CountryModel>>> GetCountryByStatusAsync(int pageIndex, int pageSize = 20, bool includeInActive = false)
        {
            List<Status> status = new List<Status> { Status.Active };
            if ( includeInActive )
                status.Add(Status.InActive);
            return await GetCountryByStatusAsync(status, pageIndex, pageSize);
        }
        protected async Task<Response<PaginationSet<CountryModel>>> GetCountryByStatusAsync(List<Status> status, int pageIndex, int pageSize = 20)
        {
           
            var response = InitErrorResponse(pageIndex, pageSize);
            var result = await _unitOfWork.CountryRepository.GetCountryByStatusAsync(status.ToArray() , pageIndex, pageSize);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<CountryModel>>(result);

            return response;
        }

        public Country RemoveChildEntity(Country entity)
        {
            entity.Regions = null;
            return entity;
        }

        public async Task<Response<CountryModel>> ChangeCountryStatusAsync(int id, Status status)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.CountryRepository.GetSingleAsync(id);

            if ( en != null )
            {
                en.Status = status;
                _unitOfWork.CountryRepository.Edit(en);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Update);
                response.Item = GetModelFromEntity(en);
            }
            else
                response.Message = MessageConstant.NotFound;

            return response;
        }


        public async Task<Response<PaginationSet<UTCTimeBaseModel>>> GetAllUtcByCountryAsync(int countryId, Status[] status, int page, int pageSize = 20)
        {
            var response = InitSuccessResponse<UTCTimeBaseModel>(page, pageSize, MessageConstant.Load);
            var result = await _unitOfWork.UTCTimeBaseRepository.FindByAsync(x => x.CountryId == countryId && status.Contains(x.Status), page, pageSize);
            response.Item = GetModelFromEntity<UTCTimeBase, UTCTimeBaseModel>(result);

            return response;
        }

        
    }
}
