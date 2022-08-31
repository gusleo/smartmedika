using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public interface IMedicalStaffFavoriteService : IReadWriteService<MedicalStaffFavoriteModel, MedicalStaffFavorite>
    {
        Task<Response<PaginationSet<MedicalStaffFavoriteModel>>> GetAllByUserAsync(int pageIndex, int pageSize = 20);
        Task<Response<MedicalStaffFavoriteModel>> DeleteByStaffIdAsync(int medicalStaffId);
    }
}
