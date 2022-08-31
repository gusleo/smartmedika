using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.service.Infrastructure;
using MediCore.Data.Entities;
using MediCore.Data.UnitOfWork;
using dna.core.auth;
using MediCore.Data.Infrastructure;
using dna.core.data.Infrastructure;

namespace MediCore.Service.Services
{
    public class RegionService : ReadWriteService<RegionModel, Region>, IRegionService
    {
       

        public RegionService(IAuthenticationService auth, IMediCoreUnitOfWork unitOfWork) : base(auth, unitOfWork)
        {

        }
        public Response<RegionModel> Create(RegionModel modelToCreate)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToCreate) )
            {
                var en = GetEntityFromModel<RegionModel, Region>(modelToCreate);
                _unitOfWork.RegionRepository.Add(this.RemoveChildEntity(en));
                _unitOfWork.Commit();
                response = InitSuccessResponse<RegionModel>(MessageConstant.Create);
                response.Item = GetModelFromEntity<Region, RegionModel>(en);
            }
            return response;
        }

        public async Task<Response<RegionModel>> Delete(int id)
        {
            var response = InitErrorResponse<RegionModel>();
            var en = await _unitOfWork.RegionRepository.GetSingleAsync(id);

            if(en != null )
            {
                en.Status = Status.InActive;

                _unitOfWork.RegionRepository.Edit(en);
                _unitOfWork.Commit();

                response = InitSuccessResponse<RegionModel>(MessageConstant.Delete);
                response.Item = GetModelFromEntity<Region, RegionModel>(en);
            }
            
            return response;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<RegionModel> Edit(RegionModel modelToEdit)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToEdit) )
            {
                var en = GetEntityFromModel<RegionModel, Region>(modelToEdit);
                _unitOfWork.RegionRepository.Edit(this.RemoveChildEntity(en));
                _unitOfWork.Commit();
                response = InitSuccessResponse<RegionModel>(MessageConstant.Update);
                response.Item = GetModelFromEntity<Region, RegionModel>(en);
            }
            return response;
        }
        public async Task<Response<RegionModel>> ChangeRegionStatusAsync(int id, Status status)
        {
            var response = InitErrorResponse<RegionModel>();
            var en = await _unitOfWork.RegionRepository.GetSingleAsync(id);
            en.Status = status;

            _unitOfWork.RegionRepository.Edit(en);
            _unitOfWork.Commit();

            response = InitSuccessResponse<RegionModel>(MessageConstant.Update);
            response.Item = GetModelFromEntity<Region, RegionModel>(en);

            return response;
        }

        public async Task<Response<PaginationSet<RegionModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            var result = await _unitOfWork.RegionRepository.GetAllAsync(pageIndex, pageSize);
            response.Item = GetModelFromEntity(result);

            return response;
        }

        public async Task<Response<RegionModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            var result = await _unitOfWork.RegionRepository.GetSingleAsync(id);
            if(result != null )
            {
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(result);
            }else
            {
                response.Message = MessageConstant.NotFound;
            }

            return response;
        }

        public Region RemoveChildEntity(Region entity)
        {
            entity.Country = null;
            entity.Regencys = null;
            return entity;
        }

        public bool Validate(RegionModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }
        public async Task<Response<PaginationSet<RegionModel>>> GetAllRegionByClueAsync(Status[] status, int page, int pageSize = 20, string clue = "")
        {

            var result = await _unitOfWork.RegionRepository.FindByAsync(x => x.Name.Contains(clue) 
                                && status.Contains(x.Status), page, pageSize,
                                    includeProperties => includeProperties.Country);

            var response = InitSuccessResponse<RegionModel>(page, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            return response;

        }
        public async Task<Response<IList<RegionModel>>> GetRegionByCountryAsync(int countryId, bool includeRegency = false)
        {
            var response = InitErrorListResponse<RegionModel>();

            IEnumerable<Region> result;
            if ( !includeRegency )
                result = await _unitOfWork.RegionRepository.FindByAsync(x => x.CountryId == countryId);
            else
                result = await _unitOfWork.RegionRepository.FindByAsync(x => x.CountryId == countryId,
                    includeProperties => includeProperties.Regencys);

            response = InitSuccessListResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(result.ToList());

            return response;
        }

        public async Task<Response<PaginationSet<RegionModel>>> GetRegionByCountryAndClueAsync(int countryId, Status[] status, int page, int pageSize = 20, string clue = "")
        {

            var result = await _unitOfWork.RegionRepository.FindByAsync(x => x.CountryId == countryId && x.Name.Contains(clue)
                        && status.Contains(x.Status), page, pageSize,
                            includeProperties => includeProperties.Country);

            var response = InitSuccessResponse(page, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            return response;
        }
    }
}
