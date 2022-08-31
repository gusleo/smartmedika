using dna.core.service.Infrastructure;
using dna.core.service.Models;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public interface IHospitalOperatorService : IReadWriteService<HospitalOperatorModel, HospitalOperator>
    {
        Task<Response<PaginationSet<HospitalOperatorModel>>> GetHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize,  HospitalStaffStatus[] status = null, string clue = "");
        Task<Response<IList<HospitalOperatorModel>>> GetOperatorHospitalAsync(int userId);
        new Task<Response<HospitalOperatorModel>> Create(HospitalOperatorModel modelToCreate);
        new Task<Response<HospitalOperatorModel>> Edit(HospitalOperatorModel modelToEdit);
        Task<Response<PaginationSet<UserModel>>> GetNonHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, HospitalStaffStatus[] status = null, string clue = "");
        Task<Response<IList<HospitalOperatorModel>>> ReAssignUserToHospital(int userId, List<HospitalOperatorModel> hospitalOperators);

    }
}
