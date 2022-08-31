using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.service.Infrastructure;
using MediCore.Data.UnitOfWork;
using dna.core.auth;
using MediCore.Data.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using dna.core.libs.Upload.Config;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Service.Services
{
    /// <summary>
    /// Class to CRUD MedicalStaffService
    /// <note>
    /// We are not cheking the rules to create/edit here
    /// since, medical staff have multiple rules
    /// authorization checked on controller
    /// </note>
    /// </summary>
    public partial class MedicalStaffService : ReadWriteService<MedicalStaffModel, MedicalStaff>, IMedicalStaffService
    {
        private readonly IImageService _imageService;
        private readonly ServerConfig _config;
        private readonly IMedicalStaffFavoriteService _favoriteServices;

        public MedicalStaffService(IOptions<ServerConfig> config, IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork, IImageService imageService, IMedicalStaffFavoriteService favoriteServices)
            : base(authService, unitOfWork)
        {
            _imageService = imageService;
            _favoriteServices = favoriteServices;
            _config = config.Value;
        }

        public async Task<Response<IList<HospitalMedicalStaffModel>>> AssignToHospitalAsync(int hospitalId, List<int> staffIds, bool isUnassociated)
        {
            var response = InitErrorListResponse<HospitalMedicalStaffModel>();
            if(IsSuperAdmin() || await IsUserAssignToHospital(hospitalId) )
            {
                var userId = _authService.GetUserId() ?? 0;
                var hospitalStaff = await _unitOfWork.HospitalMedicalStaffRepository.FindByAsync(x => x.HospitalId == hospitalId);                
                List<int> newStaffIds = new List<int>();
                foreach ( var id in staffIds )
                {
                    var en = hospitalStaff.Where(x => x.MedicalStaffId == id).FirstOrDefault();
                    if ( en != null )
                    {
                        en.Status = isUnassociated == true ? HospitalStaffStatus.InActive : HospitalStaffStatus.Active;
                        en.UpdatedById = userId;
                        en.UpdatedDate = DateTime.UtcNow;
                        _unitOfWork.HospitalMedicalStaffRepository.Edit(en);
                    }
                    else
                    {
                        newStaffIds.Add(id);
                    }
                }
                if ( newStaffIds.Count() > 0 && isUnassociated == false )
                {
                    var newStaff = new List<HospitalMedicalStaff>();
                    foreach ( var id in newStaffIds )
                    {
                        newStaff.Add(new HospitalMedicalStaff()
                        {
                            MedicalStaffId = id,
                            HospitalId = hospitalId,
                            Status = HospitalStaffStatus.Active,
                            CreatedById = userId,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedById = userId,
                            UpdatedDate = DateTime.UtcNow
                        });
                    }

                    _unitOfWork.HospitalMedicalStaffRepository.AddRange(newStaff);
                }

                _unitOfWork.Commit();
                var list = await _unitOfWork.HospitalMedicalStaffRepository.FindByAsync(x => x.HospitalId == hospitalId
                    && x.Status == HospitalStaffStatus.Active);
                response = InitSuccessListResponse<HospitalMedicalStaffModel>(MessageConstant.Update);
                response.Item = GetModelFromEntity<HospitalMedicalStaff, HospitalMedicalStaffModel>(list.ToList());
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
            
                

            
            return response;
        }

        public async Task<Response<HospitalMedicalStaffModel>> AssignToHospitalAsync(int id, int hospitalId, bool isUnassociated)
        {
            var response = InitErrorResponse<HospitalMedicalStaffModel>();
            var userId = _authService.GetUserId() ?? 0;
            if(IsSuperAdmin() || await IsUserAssignToHospital(hospitalId) )
            {
                if ( isUnassociated )
                {
                    var assign = await _unitOfWork.HospitalMedicalStaffRepository
                                    .GetSingleAsync(x => x.MedicalStaffId == id && x.HospitalId == hospitalId);
                    assign.Status = HospitalStaffStatus.InActive;
                    assign.UpdatedById = userId;
                    assign.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.HospitalMedicalStaffRepository.Edit(assign);
                    response = InitSuccessResponse<HospitalMedicalStaffModel>(MessageConstant.Update);
                    response.Item = GetModelFromEntity<HospitalMedicalStaff, HospitalMedicalStaffModel>(assign);
                }
                else
                {
                    var assign = new HospitalMedicalStaff()
                    {
                        HospitalId = hospitalId,
                        MedicalStaffId = id,
                        CreatedById = userId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedById = userId,
                        UpdatedDate = DateTime.UtcNow,
                        Status = HospitalStaffStatus.Active
                    };
                    _unitOfWork.HospitalMedicalStaffRepository.Add(assign);
                    response = InitSuccessResponse<HospitalMedicalStaffModel>(MessageConstant.Create);
                    response.Item = GetModelFromEntity<HospitalMedicalStaff, HospitalMedicalStaffModel>(assign);
                }
                _unitOfWork.Commit();
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
            
            return response;

        }

        public Response<MedicalStaffModel> Create(MedicalStaffModel modelToCreate)
        {
            var response = InitErrorResponse();
            
            var en = GetEntityFromModel(modelToCreate);
            var userId = this.GetUserId();
            en.CreatedById = userId;
            en.UpdatedById = userId;
            en.CreatedDate = DateTime.UtcNow;
            en.UpdatedDate = DateTime.UtcNow;
                         
            _unitOfWork.MedicalStaffRepository.Add(this.RemoveChildEntity(en));
            _unitOfWork.Commit();

            if ( en.Id > 0 )
            {
                var result = InitErrorListResponse<MedicalStaffSpecialistMapModel>();
                if ( modelToCreate.MedicalStaffSpecialists != null )
                {
                    result = Task.Run(() => AssignSpecialistToStaffAsync(en.Id, 
                        modelToCreate.MedicalStaffSpecialists.ToList()).Result).Result;
                }

                if ( result.Success )
                {
                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(en);
                }
                else
                {
                    response.Message = result.Message;
                }


            }
            
            return response;
        }

      
        public async Task<Response<MedicalStaffModel>> Delete(int id)
        {
            var response = InitErrorResponse();            
            var en = await _unitOfWork.MedicalStaffRepository.GetSingleAsync(id);
            en.Status = MedicalStaffStatus.InActive;
            _unitOfWork.MedicalStaffRepository.Edit(en);
            _unitOfWork.Commit();

            response = InitSuccessResponse(MessageConstant.Delete);
            response.Item = GetModelFromEntity(en);
            
            return response;

        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<MedicalStaffModel> Edit(MedicalStaffModel modelToEdit)
        {
            
            var response = InitErrorResponse();
            
            var en = GetEntityFromModel(modelToEdit);           
            en.UpdatedById = this.GetUserId();           
            en.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.MedicalStaffRepository.Edit(this.RemoveChildEntity(en));
            _unitOfWork.Commit();

            if ( en.Id > 0 )
            {
                var result = InitErrorListResponse<MedicalStaffSpecialistMapModel>();
                if ( modelToEdit.MedicalStaffSpecialists != null )
                {
                    result = Task.Run(() => AssignSpecialistToStaffAsync(en.Id,
                        modelToEdit.MedicalStaffSpecialists.ToList()).Result).Result;
                }

                if ( result.Success )
                {
                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(en);
                }else
                {
                    response.Message = result.Message;
                }
                    

            }             
            
            
            return response;


        }

       

        public async Task<Response<PaginationSet<MedicalStaffModel>>> GetAllAsync(int pageIndex, int pageSize)
        {
  
            var result = await _unitOfWork.MedicalStaffRepository.GetAllAsync(pageIndex, pageSize);
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<MedicalStaffModel>>(result);
            
            return response;
           
        }

        public async Task<Response<MedicalStaffModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            try
            {
                var result = await _unitOfWork.MedicalStaffRepository.GetSingleAsync(x => x.Id == id, x => x.Regency, x => x.Regency.Region);
                if ( result != null )
                {
                    var specialist = await _unitOfWork.MedicalStaffSpecialistMapRepository.FindByAsync(x => x.MedicalStaffId == id, x => x.MedicalStaffSpecialist);
                    result.MedicalStaffSpecialists = specialist.ToList();
                    var images = await _unitOfWork.MedicalStaffImageRepository.FindByAsync(x => x.MedicalStaffId == id, x => x.Image);
                    result.Images = images.ToList();
                    response = InitSuccessResponse(MessageConstant.Load);
                    response.Item = GetModelFromEntity(result);
                }
                else
                {
                    response.Message = String.Format("Staff with Id {0} not found", id);
                }
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }

            return response;
            
            
        }
        public async Task<Response<MedicalStaffModel>> GetSingleDetailAsync(int id)
        {
            var response = InitErrorResponse();
            try
            {
                var result = await _unitOfWork.MedicalStaffRepository.GetStaffWithHospitalDetailAsync(id);
               
                if ( result != null )
                {
                    response = InitSuccessResponse(MessageConstant.Load);
                    response.Item = Mapper.Map<MedicalStaffModel>(result);
                }
                else
                {
                    response.Message = String.Format("Staff with Id {0} not found", id);
                }
            }
            catch ( Exception ex )
            {
                response.Message = ex.Message;
            }

            return response;


        }

        public async Task<Response<IList<HospitalStaffOperatingHoursModel>>> SetOperatingHoursAsync(int hospitalId, int staffId, int estimateTime, IList<HospitalStaffOperatingHoursModel> operatingHours)
        {
            var response = InitErrorListResponse<HospitalStaffOperatingHoursModel>();
            var entities = GetEntityFromModel<HospitalStaffOperatingHoursModel, HospitalStaffOperatingHours>(operatingHours);
            var currentHospitalStaff = await _unitOfWork.HospitalMedicalStaffRepository.GetSingleAsync(x => x.HospitalId == hospitalId
                                    && x.MedicalStaffId == staffId);

            if(currentHospitalStaff == null )
            {
                response.Message = "Staff not assign to the hospital";
                return response;
            }

            int userId = this.GetUserId();
            if (IsSuperAdmin() || await IsUserAssignToHospital(hospitalId) )
            {
                currentHospitalStaff.EstimateTimeForPatient = estimateTime;
                _unitOfWork.HospitalMedicalStaffRepository.Edit(currentHospitalStaff);
                _unitOfWork.Commit();

                foreach ( var item in entities )
                {
                    item.CreatedByUser = item.UpdatedByUser = null;
                    item.HospitalStaff = null;

                    if ( item.Id == 0 )
                    {
                        item.CreatedById = item.UpdatedById = userId;
                        item.CreatedDate = item.UpdatedDate = DateTime.UtcNow;
                        item.HospitalMedicalStaffId = currentHospitalStaff.Id;                       
                        _unitOfWork.HospitalStaffOperatingHoursRepository.Add(item);
                    }
                    else
                    {
                        item.UpdatedById = userId;
                        item.UpdatedDate = DateTime.UtcNow;
                        item.HospitalMedicalStaffId = currentHospitalStaff.Id;
                        _unitOfWork.HospitalStaffOperatingHoursRepository.Edit(item);
                    }
                }


                _unitOfWork.Commit();

                response = InitSuccessListResponse<HospitalStaffOperatingHoursModel>(MessageConstant.Update);
                response.Item = GetModelFromEntity<HospitalStaffOperatingHours, HospitalStaffOperatingHoursModel>(entities);
            }
            else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }

            return response;
        }

        public async Task<Response<MedicalStaffModel>> SuspendAsync(int id)
        {
            var response = InitErrorResponse();
            try
            {
                var en = await _unitOfWork.MedicalStaffRepository.GetSingleAsync(id);
                en.Status = MedicalStaffStatus.Suspended;
                _unitOfWork.MedicalStaffRepository.Edit(en);
                _unitOfWork.Commit();

                response = InitSuccessResponse(MessageConstant.Delete);
            }
            catch ( Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<Response<PaginationSet<MedicalStaffModel>>> GetAllDoctorByHospitalAsync(int hospitalId, int pageIndex = 1, int pageSize = Constant.PageSize, MedicalStaffStatus[] status = null, bool includeOperatingHours = false, string clue = "")
        {
            status = status ?? new MedicalStaffStatus[] { MedicalStaffStatus.Active, MedicalStaffStatus.InActive, MedicalStaffStatus.Suspended, MedicalStaffStatus.Death };       
            return await GetAllStaffByHospitalAndTypeAsync(hospitalId, MedicalStaffType.Doctor, status, pageIndex, pageSize, includeOperatingHours, clue);
        }
        public async Task<Response<PaginationSet<MedicalStaffModel>>> GetActiveDoctorByHospitalAsync(int hospitalId, int pageIndex = 1, int pageSize = Constant.PageSize, bool includeOperatingHours = false, string clue = "")
        {
            var status = new MedicalStaffStatus[] { MedicalStaffStatus.Active };
            return await GetAllStaffByHospitalAndTypeAsync(hospitalId, MedicalStaffType.Doctor, status, pageIndex, pageSize, includeOperatingHours, clue);
        }
        public async Task<Response<PaginationSet<MedicalStaffModel>>> FindNearestDoctorReferenceByHospitalAsync(double longitude, double latitude, int radius,
            List<int> polyClinicIds, string search = "", int pageIndex = 1, int pageSize = Constant.PageSize)
        {
            return await FindNearestStaffReferenceByHospitalAsync(longitude, latitude, radius, polyClinicIds, search, MedicalStaffType.Doctor,  pageIndex, pageSize);
        }
        

        public async Task<Response<IList<MedicalStaffSpecialistMapModel>>> AssignSpecialistToStaffAsync(int id, IList<MedicalStaffSpecialistMapModel> specialist)
        {
            var response = InitErrorListResponse<MedicalStaffSpecialistMapModel>();
            var staff = await _unitOfWork.MedicalStaffRepository.GetSingleAsync(id);

            if(staff == null )
            {
                response.Message = MessageConstant.NotFound;
                return response;
            }


            if (this.IsSuperAdmin() || IsAllowedUser(staff.AssociatedToUserId) )
            {
                
                int userId = this.GetUserId();
                int[] medicalStaffSpecialistIds = specialist.Select(x => x.MedicalStaffSpecialistId).ToArray();

                var medicalStaffSpecialistMap = new List<MedicalStaffSpecialistMap>();
                var existSpecialist = await _unitOfWork.MedicalStaffSpecialistMapRepository
                                                                    .FindByAsync(x => x.MedicalStaffId == id);
                var deleted = existSpecialist.Where(x => !medicalStaffSpecialistIds.Contains(x.MedicalStaffSpecialistId)).ToList();
                foreach ( int medicalStaffId in medicalStaffSpecialistIds )
                {
                    bool isExist = existSpecialist.Where(x => x.MedicalStaffSpecialistId.Equals(medicalStaffId)).FirstOrDefault() != null;
                    if ( !isExist )
                    {
                        medicalStaffSpecialistMap.Add(new MedicalStaffSpecialistMap
                        {
                            MedicalStaffId = id,
                            MedicalStaffSpecialistId = medicalStaffId,
                            CreatedById = userId,
                            UpdatedById = userId,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow 
                        });
                    }

                }
                if ( medicalStaffSpecialistMap != null )
                    _unitOfWork.MedicalStaffSpecialistMapRepository.AddRange(medicalStaffSpecialistMap);
                if ( deleted != null )
                    _unitOfWork.MedicalStaffSpecialistMapRepository.DeleteRange(deleted);

                _unitOfWork.Commit();
                response = InitSuccessListResponse<MedicalStaffSpecialistMapModel>(MessageConstant.Update);
                response.Item = GetModelFromEntity<MedicalStaffSpecialistMap, MedicalStaffSpecialistMapModel>(medicalStaffSpecialistMap);
               
            }
            else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }



            return response;
        }

        private bool IsAllowedUser(int? associateToUserId)
        {
            if ( associateToUserId.HasValue )
                return this.GetUserId() == associateToUserId;
            else
                return false;
        }
        public bool Validate(MedicalStaffModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public MedicalStaff RemoveChildEntity(MedicalStaff entity)
        {
            entity.AssociatedToUser = entity.CreatedByUser = entity.UpdatedByUser = null;
            entity.MedicalStaffClinics = null;
            entity.MedicalStaffSpecialists = null;
            entity.Regency = null;
            return entity;
        }

        public async Task<Response<PaginationSet<MedicalStaffModel>>> GetAvailableStaffSortByDistanceAsync(int hospitalId, int radius, MedicalStaffType[] types, MedicalStaffStatus[] status, string search = "", double longitude = 0, double latitude = 0, int pageIndex = 1, int pageSize = 20)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            int userId = this.GetUserId();
            if ( await IsUserAssignToHospital(hospitalId) )
            {
                var staffs = await _unitOfWork.HospitalMedicalStaffRepository.FindByAsync(x => x.HospitalId == hospitalId
                                    && x.Status == HospitalStaffStatus.Active);
                int[] staffIds = staffs.Select(x => x.MedicalStaffId).ToArray();

                if(longitude == 0 && latitude == 0 )
                {
                    var hospital = await _unitOfWork.HospitalRepository.GetSingleAsync(x => x.Id == hospitalId,
                        includeProperties => includeProperties.Regency);

                    if ( hospital != null && hospital.Longitude != null && hospital.Latitude != null )
                    {
                        longitude = hospital.Longitude ?? hospital.Regency.Longitude;
                        latitude = hospital.Latitude ?? hospital.Regency.Latitude;
                    }
                    
                    
                }
                
                var result = await _unitOfWork.MedicalStaffRepository.GetAllStaffOrderByDistanceAsync(longitude, latitude, radius, search, staffIds, types, status, pageIndex, pageSize);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = GetModelFromEntity(result);
            } else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
            return response;
        }

        public async Task<Response<MedicalStaffModel>> ChangeStaffStatusAsync(int id, MedicalStaffStatus status)
        {
            var en = await _unitOfWork.MedicalStaffRepository.GetSingleAsync(id);
            en.Status = status;
            _unitOfWork.MedicalStaffRepository.Edit(en);
            _unitOfWork.Commit();

            var response = InitSuccessResponse(MessageConstant.Update);
            response.Item = GetModelFromEntity(en);

            return response;
        }

        public async Task<Response<PaginationSet<MedicalStaffModel>>> GetAllDoctorByClueAsync(int pageIndex = 1, int pageSize = 20, string clue = "")
        {
            MedicalStaffType[] types = new MedicalStaffType[] { MedicalStaffType.Doctor };            
            var result = await _unitOfWork.MedicalStaffRepository.GetAllStaffByTypeAsync(types, pageIndex, pageSize, clue);
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            return response;
        }

        public async Task<Response<IList<MedicalStaffImageModel>>> UploadStaffImageAsync(int id, IList<IFormFile> files, bool isPrimary)
        {
            var response = InitErrorListResponse<MedicalStaffImageModel>();
            if ( await IsUserAssignToHospital(id) )
            {

                var result = await _imageService.UploadImageAsync(files, isPrimary);
                if ( result.Success )
                {
                    var images = new List<MedicalStaffImage>();
                    foreach ( var item in result.Item )
                    {
                        images.Add(new MedicalStaffImage
                        {
                            MedicalStaffId = id,
                            ImageId = item.Id
                        });
                    }
                    _unitOfWork.MedicalStaffImageRepository.AddRange(images);
                    _unitOfWork.Commit();

                    response = InitSuccessListResponse<MedicalStaffImageModel>(MessageConstant.Create);
                    response.Item = GetModelFromEntity<MedicalStaffImage, MedicalStaffImageModel>(images);

                }
                else
                {
                    response.Message = result.Message;
                }


            }
            else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }


            return response;
        }

        public async Task<Response<MedicalStaffImageModel>> UploadStaffImageCoverAsync(int id, IFormFile file)
        {

            var images = await _unitOfWork.MedicalStaffImageRepository.FindByAsync(x => x.MedicalStaffId == id
                                && x.Image.IsPrimary == true, img => img.Image);
            var image = images.ToList().FirstOrDefault();

            if ( image != null )
            {
                //auto delete hospitalimage, sinces it uses cascade.delete
                await _imageService.Delete(image.Image.Id);

            }
            var response = await UploadStaffImageAsync(id, new List<IFormFile>() { file }, true);
            if ( response.Success )
            {
                int imgId = response.Item[0].ImageId;
                var en = await _unitOfWork.ImageRepository.GetSingleAsync(imgId);
                en.IsPrimary = true;
                _unitOfWork.ImageRepository.Edit(en);
                _unitOfWork.Commit();
            }

            return new Response<MedicalStaffImageModel>
            {
                Success = response.Success,
                Message = response.Message,
                Item = response.Item.FirstOrDefault()
            };

        }

        public async Task<Response<HospitalMedicalStaffModel>> GetStaffOperatingHoursAsync(int hospitalId, int staffId)
        {
            var response = InitErrorResponse<HospitalMedicalStaffModel>();
            var result = await _unitOfWork.HospitalMedicalStaffRepository.GetSingleAsync(x => x.HospitalId == hospitalId
                                && x.MedicalStaffId == staffId, x => x.OperatingHours);
            if(result != null )
            {
                response = InitSuccessResponse<HospitalMedicalStaffModel>(MessageConstant.Load);
                response.Item = GetModelFromEntity<HospitalMedicalStaff, HospitalMedicalStaffModel>
                                    (result);
            }
            else
            {
                response.Message = MessageConstant.NotFound;
            }

            return response;
        }

        public async Task<Response<IList<HospitalMedicalStaffModel>>> GetStaffOperatingHoursByDateAsync(int staffId, int dayOfWeek)
        {
            var response = InitSuccessListResponse<HospitalMedicalStaffModel>(MessageConstant.Load);
            var result = await _unitOfWork.HospitalStaffOperatingHoursRepository.GetStaffOperatingHoursByDateAsync(staffId, dayOfWeek);
            response.Item = GetModelFromEntity<HospitalMedicalStaff, HospitalMedicalStaffModel>(result);
            return response;
        }
    }
}
