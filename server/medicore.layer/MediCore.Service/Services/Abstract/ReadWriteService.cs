using dna.core.auth;
using dna.core.data.Abstract;
using dna.core.service.Models.Abstract;
using dna.core.service.Services.Abstract;
using MediCore.Data.Infrastructure;
using MediCore.Data.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public class ReadWriteService<TModel, TEntity> : ReadWriteServiceBase<TModel, TEntity>
        where TModel : IModelBase, new()
        where TEntity : IEntityBase, new()
    {
       
        protected readonly IMediCoreUnitOfWork _unitOfWork;
        public ReadWriteService(IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork) : base(authService)
        {
           
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetUserHospitalId()
        {
            int userId = _authService.GetUserId() ?? 0;
            var hospital = await _unitOfWork.HospitalOperatorRepository.FindByAsync(x => x.UserId == userId);
            return hospital == null ? 0 : hospital.FirstOrDefault().HospitalId;
        }
       
        
        protected async Task<bool> IsUserAssignToHospital(int hospitalId)
        {
            
            if ( _authService.IsSuperAdmin() )
                return true;
            else
            {
                int userId = _authService.GetUserId() ?? 0;

                var hospital = await _unitOfWork.HospitalOperatorRepository
                    .FindByAsync(x => x.UserId == userId && x.HospitalId == hospitalId && x.Status == HospitalStaffStatus.Active);
                return hospital == null ? false : true;
            }
            
        }
    }
}
