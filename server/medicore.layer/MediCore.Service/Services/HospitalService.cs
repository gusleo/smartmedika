using MediCore.Data.Entities;
using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using dna.core.service.Infrastructure;
using MediCore.Data.UnitOfWork;
using AutoMapper;
using MediCore.Data.Infrastructure;
using dna.core.auth;
using MediCore.Service.Services.Abstract;
using Microsoft.AspNetCore.Http;
using dna.core.service.Services.Abstract;
using Microsoft.Extensions.Options;
using dna.core.libs.Upload.Config;
using System.Threading.Tasks;

namespace MediCore.Service.Services
{
    public class HospitalService : ReadWriteService<HospitalModel, Hospital>, IHospitalService
    {
        private readonly IImageService _imageService;
        private readonly ServerConfig _config;
        public HospitalService(IOptions<ServerConfig> config, IMediCoreUnitOfWork unitOfWork, IAuthenticationService authService, IImageService imageService) : base(authService, unitOfWork)
        {
            _imageService = imageService;
            _config = config.Value;
        }


        public bool Validate(HospitalModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }

        public async Task<Response<HospitalModel>> Create(HospitalModel modelToCreate)
        {
            var response = InitErrorResponse();

            if ( Validate(modelToCreate) )
            {
                int userId = this.GetUserId();
                var en = GetEntityFromModel(modelToCreate);
                en.Status = HospitalStatus.Active;
                en.CreatedById = userId;
                en.UpdatedById = userId;
                en.CreatedDate = DateTime.UtcNow;
                en.UpdatedDate = DateTime.UtcNow;

                //null the child
                en.OperatingHours = null;
                en.PolyClinicMaps = null;
                en.Images = null;

                _unitOfWork.HospitalRepository.Add(this.RemoveChildEntity(en));
                _unitOfWork.Commit();
                if ( en.Id > 0 )
                {
                    if ( modelToCreate.OperatingHours != null )
                    {                       
                        await ModifyOperatingHoursAsync(en.Id, modelToCreate.OperatingHours.ToList());
                    }
                   

                    if ( modelToCreate.PolyClinicMaps != null && modelToCreate.PolyClinicMaps.Count > 0 )
                    {
                       await AssignPolyClinicToHospitalAsync(en.Id, modelToCreate.PolyClinicMaps.ToList());
                    }

                    

                    response = InitSuccessResponse(MessageConstant.Create);
                    response.Item = GetModelFromEntity(en);

                    
                }
            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }

           
            return response;
        }

        public async Task<Response<HospitalModel>> Edit(HospitalModel modelToEdit)
        {
            var response = InitErrorResponse();

            if ( !Validate(modelToEdit) )
            {
                response.Message = MessageConstant.ValidationError;
                return response;
            }

            if ( await IsUserAssignToHospital(modelToEdit.Id) )
            {

                var en = GetEntityFromModel(modelToEdit);

                int userId = this.GetUserId();
                en.UpdatedById = userId;
                en.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.HospitalRepository.Edit(this.RemoveChildEntity(en));
                _unitOfWork.Commit();
                if ( en.Id > 0 )
                {
                    if ( modelToEdit.OperatingHours != null )
                    {
                       
                        await ModifyOperatingHoursAsync(en.Id, modelToEdit.OperatingHours.ToList());
                    }


                    if ( modelToEdit.PolyClinicMaps != null && modelToEdit.PolyClinicMaps.Count > 0 )
                    {
                        await AssignPolyClinicToHospitalAsync(en.Id, modelToEdit.PolyClinicMaps.ToList());
                    }

                   
                    response = InitSuccessResponse(MessageConstant.Create);
                    response.Item = GetModelFromEntity(en);

                }


            }
            else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }


            return response;

        }


        public async Task<Response<HospitalModel>> AssignPolyClinicToHospitalAsync(int id, IList<PolyClinicToHospitalMapModel> polyClinics)
        {
            var response = InitErrorResponse();
            if ( await IsUserAssignToHospital(id) )
            {
                try
                {
                    int[] polyClinicIds = polyClinics.Select(x => x.PolyClinicId).ToArray();

                    IList<PolyClinicToHospitalMap> polyClinicMaps = new List<PolyClinicToHospitalMap>();
                    var existPolyClinic = await _unitOfWork.PolyClinicToHospitalMapRepository
                                                                        .FindByAsync(x => x.HospitalId == id);
                    var deleted = existPolyClinic.Where(x => !polyClinicIds.Contains(x.PolyClinicId)).ToList();
                    foreach ( int polyId in polyClinicIds )
                    {
                        bool isExist = existPolyClinic.Where(x => x.PolyClinicId.Equals(polyId)).FirstOrDefault() != null;
                        if ( !isExist )
                        {
                            polyClinicMaps.Add(new PolyClinicToHospitalMap
                            {
                                HospitalId = id,
                                PolyClinicId = polyId
                            });
                        }

                    }
                    if ( polyClinicMaps != null )
                        _unitOfWork.PolyClinicToHospitalMapRepository.AddRange(polyClinicMaps);
                    if ( deleted != null )
                        _unitOfWork.PolyClinicToHospitalMapRepository.DeleteRange(deleted);

                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Update);
                }
                catch ( Exception ex )
                {
                    response.Message = ex.Message;
                }
            }
            else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }



            return response;
        }

        private async Task<Response<IList<HospitalOperatingHoursModel>>> ModifyOperatingHoursAsync(int hospitalId, IList<HospitalOperatingHoursModel> operatingHours)
        {
            var response = InitErrorListResponse<HospitalOperatingHoursModel>();
            var entities = GetEntityFromModel<HospitalOperatingHoursModel, HospitalOperatingHours>(operatingHours);

            int userId = this.GetUserId();
            if ( await this.IsUserAssignToHospital(userId) )
            {
                foreach ( var item in entities )
                {
                    item.HospitalId = hospitalId;
                    item.UpdatedById = userId;
                    item.UpdatedDate = DateTime.UtcNow;

                    if ( item.Id == 0 )
                    {
                       
                        item.CreatedById = userId;
                        item.CreatedDate = DateTime.UtcNow;
                        _unitOfWork.HospitalOperatingHours.Add(item);
                    }
                    else
                    {
                        _unitOfWork.HospitalOperatingHours.Edit(item);
                    }
                }


                _unitOfWork.Commit();

                response = InitSuccessListResponse<HospitalOperatingHoursModel>(MessageConstant.Update);
                response.Item = GetModelFromEntity<HospitalOperatingHours, HospitalOperatingHoursModel>(entities);
            }
            else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }

            return response;
        }

       
        public Response<IList<HospitalModel>> CreateRange(List<HospitalModel> listOfData)
        {
            var response = InitErrorListResponse();            
            if ( this.IsSuperAdmin() )
            {
                int userId = this.GetUserId();
                var data = Mapper.Map<IList<Hospital>>(listOfData);
                data.All(x => {
                    x.Status = HospitalStatus.Active;
                    x.CreatedById = userId;
                    x.CreatedDate = DateTime.UtcNow;
                    x.UpdatedById = userId;
                    x.UpdatedDate = DateTime.UtcNow;
                    return true;
                });//update all

                _unitOfWork.HospitalRepository.AddRange(data);
                _unitOfWork.Commit();
                response = InitSuccessListResponse(MessageConstant.Create);
                response.Item = GetModelFromEntity(data);
                 
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
            
            return response;
           
        }

        public async Task<Response<HospitalModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            if ( this.IsSuperAdmin() )
            {
                
                    var en = await _unitOfWork.HospitalRepository.GetSingleAsync(id);
                    if ( en != null )
                    {
                        en.Status = HospitalStatus.InActive;
                        en.UpdatedById = this.GetUserId();
                        en.UpdatedDate = DateTime.UtcNow;
                        _unitOfWork.HospitalRepository.Edit(en);
                        _unitOfWork.Commit();

                        response.Success = true;
                        response.Message = MessageConstant.Delete;
                    }
                    else
                    {
                        response.Message = MessageConstant.NotFound;
                    }                      
                
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
            
            return response;
           
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }       

       

       
        public async Task<Response<HospitalModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();           
            var result = await _unitOfWork.HospitalRepository.GetSingleAsync(x => x.Id == id,
                        poly => poly.PolyClinicMaps, meta => meta.OperatingHours);
            response = InitSuccessResponse(MessageConstant.Load);
            response.Item = GetModelFromEntity(result);            
            return response;

        }

        public async Task<Response<PaginationSet<HospitalModel>>> GetAllAsync(int pageIndex, int pageSize = Constant.PageSize)
        {
            var response = InitErrorResponse(pageIndex, pageSize);           
            var result = await _unitOfWork.HospitalRepository.GetAllAsync(pageIndex, pageSize);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<HospitalModel>>(result);            
            return response;
            
        }
        public async Task<Response<PaginationSet<HospitalModel>>> GetActiveHospitalAsync(int pageIndex, int pageSize)
        {
            HospitalStatus[] status = new HospitalStatus[] { HospitalStatus.Active };
            var response = await FindHospitalByFilterAsync(pageIndex, pageSize, 0, String.Empty, status);
            return response;
        }



        public async Task<Response<HospitalMetadataModel>> UpdateHospitalMetadataAsync(int id, HospitalMetadataModel modelToEdit)
        {
            var response = InitErrorResponse<HospitalMetadataModel>();
            if ( await IsUserAssignToHospital(id) )
            {
                
                int userId = this.GetUserId();
                var en = await _unitOfWork.HospitalMetadataRepository.GetSingleAsync(x => x.HospitalId == id && x.MetaType == modelToEdit.MetaType);
                if ( en == null )
                {
                    en = new HospitalMetadata
                    {
                        Id = 0,
                        HospitalId = id,
                        MetaType = modelToEdit.MetaType,
                        CreatedById = userId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedById = userId,
                        UpdatedDate = DateTime.UtcNow,                            
                        JsonValue = modelToEdit.JsonValue
                    };
                    _unitOfWork.HospitalMetadataRepository.Add(en);
                }
                else
                {
                    en.UpdatedById = userId;
                    en.UpdatedDate = DateTime.UtcNow;
                    en.JsonValue = modelToEdit.JsonValue;
                    _unitOfWork.HospitalMetadataRepository.Edit(en);
                }
                _unitOfWork.Commit();
                response = InitSuccessResponse<HospitalMetadataModel>(MessageConstant.Update);
                response.Item = Mapper.Map<HospitalMetadataModel>(en);
                
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }

            return response;
            
        } 
        public async Task<Response<IList<HospitalImageModel>>> UploadHospitalImageAsync(int id, IList<IFormFile> files, bool isPrimary)
        {
            var response = InitErrorListResponse<HospitalImageModel>();
            if ( await IsUserAssignToHospital(id) )
            {

                var result = await _imageService.UploadImageAsync(files, isPrimary);
                if ( result.Success )
                {
                    var hospitalImages = new List<HospitalImage>();
                    foreach(var item in result.Item )
                    {
                        hospitalImages.Add(new HospitalImage
                        {
                            HospitalId = id,
                            ImageId = item.Id
                        });
                    }
                    _unitOfWork.HospitalImageRepository.AddRange(hospitalImages);
                    _unitOfWork.Commit();

                    response = InitSuccessListResponse<HospitalImageModel>(MessageConstant.Create);
                    response.Item = GetModelFromEntity<HospitalImage, HospitalImageModel>(hospitalImages);

                }else
                {
                    response.Message = result.Message;
                }                
                        

            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }


            return response;
        }

        public async Task<Response<HospitalImageModel>> UploadHospitalImageCoverAsync(int id, IFormFile file)
        {

            var image = await _unitOfWork.HospitalImageRepository.GetSingleAsync(x => x.HospitalId == id &&
                                x.Image.IsPrimary == true, img => img.Image);

            if ( image != null )
            {
                //auto delete hospitalimage, sinces it uses cascade.delete
               await _imageService.Delete(image.Image.Id);               
                
            }
            var response = await UploadHospitalImageAsync(id, new List<IFormFile>() { file }, true);

            return new Response<HospitalImageModel>
            {
                Success = response.Success,
                Message = response.Message,
                Item = response.Item.FirstOrDefault()
            };
            
        }



        public async Task<Response<HospitalModel>> GetHospitalDetailAsync(int id)
        {
            var response = InitErrorResponse();
           
            var result = await _unitOfWork.HospitalRepository.GetHospitalDetailAsync(id); 
            if(result != null )
            {
                var img = await  _unitOfWork.HospitalImageRepository.FindByAsync(x => x.HospitalId == id, image => image.Image);

                result.Images = img.ToList();
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(result);
            }
            else
            {
                response.Message = MessageConstant.NotFound;
            }
                                         
            return response;
        }
        public async Task<Response<PaginationSet<HospitalModel>>> FindNearestHospitalAsync(double longitude, double latitude, int radius, List<int> polyClinicIds, int pageIndex, int pageSize, string clue = "")
        {
            var result = await _unitOfWork.HospitalRepository.FindNearestHospitalAsync(longitude, latitude, radius, polyClinicIds, pageIndex, pageSize, clue);

            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = GetModelFromEntity(result);
            return response;
        }

        public async Task<Response<PaginationSet<HospitalModel>>> FindHospitalByFilterAsync(int pageIndex, int pageSize, int regionId, string clue = "", HospitalStatus[] status = null)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                status = status ?? new HospitalStatus[] {
                            HospitalStatus.Active,
                            HospitalStatus.InActive,
                            HospitalStatus.Suspended }
;
                                    

                var result = await _unitOfWork.HospitalRepository.FindHospitalByFilterStatusAsync(regionId, clue, status, pageIndex, pageSize);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = Mapper.Map<PaginationSet<HospitalModel>>(result);
            } catch(Exception ex )
            {
                response.Message = ex.Message;
            }

            return response;
        }



        public async Task<Response<HospitalModel>> ChangeHospitalStatusAsync(int id, HospitalStatus status)
        {
            var response = InitErrorResponse();

            if ( IsSuperAdmin() )
            {
                var en = await _unitOfWork.HospitalRepository.GetSingleAsync(id);
                int userId = this.GetUserId();
                if ( en != null )
                {
                    en.Status = status;
                    en.UpdatedById = userId;
                    en.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.HospitalRepository.Edit(en);
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(en);
                }
                else
                    response.Message = MessageConstant.NotFound;
            }
            else
                response.Message = MessageConstant.UserNotAllowed;
            

            return response;

        }

        public async Task<Response<IList<HospitalModel>>> GetHospitalAssocatedUserAsync()
        {
            int userId = this.GetUserId();
            userId = this.IsSuperAdmin() ? 0 : userId;
            var result = await _unitOfWork.HospitalRepository.GetHospitalAssociatedToUserAsync(userId);
            return new Response<IList<HospitalModel>>
            {
                Item = GetModelFromEntity(result),
                Message = MessageConstant.Load,
                Success = true
            };
        }

        public Hospital RemoveChildEntity(Hospital entity)
        {
            entity.OperatingHours = null;
            entity.PolyClinicMaps = null;
            entity.Images = null;
            entity.Regency = null;
            entity.UpdatedByUser = null;
            entity.CreatedByUser = null;

            return entity;
           
        }


        #region "hide"
        Response<HospitalModel> IReadWriteService<HospitalModel, Hospital>.Create(HospitalModel modelToCreate)
        {
            throw new NotImplementedException();
        }

        Response<HospitalModel> IReadWriteService<HospitalModel, Hospital>.Edit(HospitalModel modelToEdit)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
