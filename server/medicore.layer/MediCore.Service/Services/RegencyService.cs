using dna.core.auth;
using MediCore.Data.Entities;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.service.Infrastructure;
using dna.core.data.Infrastructure;

namespace MediCore.Service.Services
{
    public class RegencyService : ReadWriteService<RegencyModel, Regency>, IRegencyService
    {
        public RegencyService(IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork) : base(authService, unitOfWork)
        {

        }

        public Response<RegencyModel> Create(RegencyModel modelToCreate)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToCreate) )
            {
                var en = GetEntityFromModel(modelToCreate);
                _unitOfWork.RegencyRepository.Add(this.RemoveChildEntity(en));
                _unitOfWork.Commit();

                response = InitSuccessResponse(MessageConstant.Create);
                response.Item = GetModelFromEntity(en);

            }else
            {
                response.Message = MessageConstant.ValidationError;
            }
            return response;
        }

        public async Task<Response<RegencyModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            var en = await  _unitOfWork.RegencyRepository.GetSingleAsync(id);
            if ( en != null)
            {
                _unitOfWork.RegencyRepository.Edit(this.RemoveChildEntity(en));
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

        public Response<RegencyModel> Edit(RegencyModel modelToEdit)
        {
            var response = InitErrorResponse();
            if ( Validate(modelToEdit) )
            {
                var en = GetEntityFromModel(modelToEdit);
                _unitOfWork.RegencyRepository.Edit(this.RemoveChildEntity(en));
                _unitOfWork.Commit();

                response = InitSuccessResponse(MessageConstant.Delete);
                response.Item = GetModelFromEntity(en);
            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }           
            
            return response;
        }

        public async Task<Response<PaginationSet<RegencyModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            var result = await _unitOfWork.RegencyRepository.GetAllAsync(pageIndex, pageSize, x => x.Region, x => x.Region.Country);
            response.Item = GetModelFromEntity(result);
            return response;
        }

        public async Task<Response<RegencyModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.RegencyRepository.GetSingleAsync(x => x.Id == id, x => x.Region, x => x.Region.Country);
            if(en != null){
                response.Message = MessageConstant.Load;
                response.Item = GetModelFromEntity(en);
            }else{
                response.Message = MessageConstant.NotFound;
            }
            return response;
        }
        public async Task<Response<IList<RegencyModel>>> GetRegencyByRegionAsync(int regionId)
        {
            var response = InitErrorListResponse();

            var result = await _unitOfWork.RegencyRepository.FindByAsync(x => x.RegionId == regionId);

            response = InitSuccessListResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(result.ToList());

            return response;
        }

        public Regency RemoveChildEntity(Regency entity)
        {
            entity.Region = null;
            return entity;
        }

        public bool Validate(RegencyModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public async Task<Response<PaginationSet<RegencyModel>>> GetRegencyDetailByCountryAsync(int countryId, Status[] Status, int page, int pageSize = 20, string clue = "")
        {
            var response = InitSuccessResponse(page, pageSize, MessageConstant.Load);
            var result = await _unitOfWork.RegencyRepository.GetRegencyDetailByCountry(countryId, Status, page, pageSize, clue);

            response.Item = GetModelFromEntity(result);

            return response;
        }

        public async Task<Response<RegencyModel>> ChangeRegencyStatusAsync(int id, Status status)
        {
            var response = InitErrorResponse();
            var result = await _unitOfWork.RegencyRepository.GetSingleAsync(id);
            if(result != null )
            {
                result.Status = status;
                _unitOfWork.RegencyRepository.Edit(result);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(result);
            }else
            {
                response.Message = MessageConstant.NotFound;
            }

            return response;
        }
    }
}
