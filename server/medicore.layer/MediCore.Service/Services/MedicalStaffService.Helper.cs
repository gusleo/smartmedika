using AutoMapper;
using dna.core.service.Infrastructure;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Services
{
    public partial class MedicalStaffService
    {
       
        #region "Method helper"
        protected async Task<Response<PaginationSet<MedicalStaffModel>>> GetAllStaffByHospitalAndTypeAsync(int hospitalId, MedicalStaffType type, MedicalStaffStatus[] status, int pageIndex = 1, int pageSize = Constant.PageSize, bool includeOperatingHours = false, string clue = "")
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                var types = new MedicalStaffType[] { type };
                var result = await _unitOfWork.MedicalStaffRepository.GetAllStaffByHospitalAndTypeAsync(hospitalId, types, 
                                status, pageIndex, pageSize, includeOperatingHours, clue);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = Mapper.Map<PaginationSet<MedicalStaffModel>>(result);
                
            }
            catch ( Exception ex )
            {
                response.Message = ex.Message;
            }

            return response;
        }
        protected async Task<Response<PaginationSet<MedicalStaffModel>>> FindNearestStaffReferenceByHospitalAsync(double longitude, double latitude, int radius,
            List<int> polyClinicIds, string search = "", MedicalStaffType type = MedicalStaffType.Doctor, int pageIndex = 1, int pageSize = Constant.PageSize)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
               
                var result = await _unitOfWork.MedicalStaffRepository
                                .FindNearestStaffReferenceByHospitalAsync(longitude, latitude, radius, search, type, polyClinicIds, pageIndex, pageSize);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = Mapper.Map<PaginationSet<MedicalStaffModel>>(result);
                var userId = GetUserId();

                if(userId > 0){
                    var favoriteDoctors = await _unitOfWork.MedicalStaffFavoriteRepository.FindByAsync(x => x.UserId == userId);
                    var favoriteDoctorIds = favoriteDoctors.Select(x => x.MedicalStaffId).ToList();
                    response.Item.Items = response.Item.Items.Select(x => { x.IsFavorite = favoriteDoctorIds.Contains(x.Id); return x; }).ToList();
                }


            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
    }
}
