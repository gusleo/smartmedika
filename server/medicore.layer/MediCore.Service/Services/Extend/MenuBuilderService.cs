using dna.core.auth;
using dna.core.service.Services;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Services.Extend.Abstract;

namespace MediCore.Service.Services.Extend
{
    public class MenuBuilderService : TreeMenuService, IMenuBuilderService
    {
        
        public MenuBuilderService(IMediCoreUnitOfWork unitOfWork, IAuthenticationService auth) : base(auth, unitOfWork)
        {
           
        }
    }
}
