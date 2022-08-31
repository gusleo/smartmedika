using MediCore.Data.Entities;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.service.Infrastructure;
using dna.core.auth;
using MediCore.Data.UnitOfWork;
using MediCore.Data.Infrastructure;
using AutoMapper;
using dna.core.libs.Extension;
using dna.core.libs.Sender;
using MediCore.Service.Helper.ContentGenerator;
using dna.core.service.Services.Abstract;
using MediCore.Service.Model.Custom;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MediCore.Service.Services
{
    public class HospitalAppointmentService : ReadWriteService<HospitalAppointmentModel, HospitalAppointment>, IHospitalAppointmentService
    {
        ISenderFactory _senderFact;
        readonly IHostingEnvironment _env;
       
        public HospitalAppointmentService(ISenderFactory senderFact,  IAuthenticationService authService, IHostingEnvironment env, IMediCoreUnitOfWork unitofWork) 
            : base (authService, unitofWork)
        {
            _senderFact = senderFact;
            _env = env;
           
        }

        public async Task<Response<HospitalAppointmentModel>> ChangeAppointmentStatusAsync(int id, AppointmentStatus status)
        {
            int userId = this.GetUserId();
            var response = InitErrorResponse();
            if ( !await IsUserAssignToHospital(userId) )
            {
                response.Message = MessageConstant.UserNotAllowed;
            }else
            {
                var en = await _unitOfWork.HospitalAppointmentRepository.GetSingleAsync(id);
                if ( en != null )
                {
                    en.AppointmentStatus = status;
                    en.UpdatedById = userId;
                    en.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.HospitalAppointmentRepository.Edit(en);
                    _unitOfWork.Commit();
                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(en);
                }
                else
                {
                    response.Message = MessageConstant.NotFound;
                }
            }
            
            
            return response;
        }

        public async Task<Response<HospitalAppointmentModel>> Create(HospitalAppointmentModel modelToCreate)
        {
            var response = InitErrorResponse();

            if ( Validate(modelToCreate) )
            {
                var userId = GetUserId();
                var en = GetEntityFromModel(modelToCreate);

               int nextNumber = await _unitOfWork.HospitalAppointmentRepository
                        .GetMaxQueueNumberAsync(modelToCreate.HospitalId, modelToCreate.MedicalStaffId, modelToCreate.AppointmentDate);
                DateTime estimateTime = await GetEstimateTimeAsync(modelToCreate, nextNumber);

                en.QueueNumber = nextNumber;
                en.AppointmentDate = estimateTime;
                en.UserId = userId;
                en.CreatedById = en.UpdatedById = userId;
                en.CreatedDate = en.UpdatedDate = DateTime.UtcNow;
                en.AppointmentStatus = AppointmentStatus.Pending;

                _unitOfWork.HospitalAppointmentRepository.Add(this.RemoveChildEntity(en));
                _unitOfWork.Commit();
                var apptDetails = new List<HospitalAppointmentDetail>();

                if ( en.Id > 0 )
                {
                    //insert appointemnt detail (patient)
                    foreach ( var item in modelToCreate.AppointmentDetails )
                    {
                        var enItem = Mapper.Map<HospitalAppointmentDetail>(item);
                        enItem.HospitalAppointment = null;
                        enItem.Patient = null;
                        enItem.HospitalAppointmentId = en.Id;
                        apptDetails.Add(enItem);
                    }

                    _unitOfWork.HospitalAppointmentDetailRepository.AddRange(apptDetails);
                    _unitOfWork.Commit();

                    var current = await _unitOfWork.HospitalAppointmentRepository.GetHospitalAppointmentDetailAsync(en.Id);

                    response = InitSuccessResponse(MessageConstant.Create);
                    response.Item = GetModelFromEntity(current);
                }
            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }
            
           
            return response;
        }

        private FirebasePayload GetFirebasePayload()
        {
            var data = new FirebasePayload();
            string filePath = System.IO.Path.Combine(_env.ContentRootPath, "Resources/firebasepayload.json");
            using ( StreamReader reader = File.OpenText(filePath))
            {
                string content = reader.ReadToEnd();
                data = JsonConvert.DeserializeObject<FirebasePayload>(content);

            }
            return data;
        }
        public async Task<Response<HospitalAppointmentModel>> CreateForNonRegisteredUserAsync(HospitalAppointmentModel modelToCreate)
        {
            var response = InitErrorResponse();

            if ( Validate(modelToCreate) )
            {

                int nextNumber = await _unitOfWork.HospitalAppointmentRepository.GetMaxQueueNumberAsync(modelToCreate.HospitalId, modelToCreate.MedicalStaffId, modelToCreate.AppointmentDate);
                DateTime estimateTime = await GetEstimateTimeAsync(modelToCreate, nextNumber);

                var userId = GetUserId();
                var en = GetEntityFromModel(modelToCreate);
                en.UserId = null;
                en.CreatedById = en.UpdatedById = userId;
                en.CreatedDate = en.UpdatedDate = DateTime.UtcNow;
                en.AppointmentStatus = AppointmentStatus.Pending;
                en.QueueNumber = nextNumber;
                en.AppointmentDate = estimateTime;
                _unitOfWork.HospitalAppointmentRepository.Add(this.RemoveChildEntity(en));
                _unitOfWork.Commit();

                var current = await _unitOfWork.HospitalAppointmentRepository.GetHospitalAppointmentDetailAsync(en.Id);
                var content = ContentGenerator.BookingDoctor(GetModelFromEntity<HospitalAppointment, HospitalAppointmentModel>(current));

                ISender sender = _senderFact.Create(SenderFactory.SMS);
                var result = await sender.SendAsync(en.PhoneNumber, content.Title, content.Body);

                response = InitSuccessResponse(MessageConstant.Create);
                response.Item = GetModelFromEntity(en);
            }
            else
            {
                response.Message = MessageConstant.ValidationError;
            }
            
            
            return response;
        }

        private async Task<DateTime> GetEstimateTimeAsync(HospitalAppointmentModel modelToCreate, int nextNumber)
        {
            
            var setting = await _unitOfWork.HospitalMedicalStaffRepository.GetSingleAsync(x => x.HospitalId == modelToCreate.HospitalId
                                                    && x.MedicalStaffId == modelToCreate.MedicalStaffId, x => x.OperatingHours);
            var openTime = setting.OperatingHours.Where(x => x.Day == (int)modelToCreate.AppointmentDate.DayOfWeek).FirstOrDefault();

            DateTime estimateTime = modelToCreate.AppointmentDate.StartOfDay();
            TimeSpan time = TimeSpan.Parse(openTime.StartTime); //hh:mm (24 hours format)
            int estimateTimeForPatient = setting.EstimateTimeForPatient ?? 0;
            //estimate time is zero then use default
            if( estimateTimeForPatient == 0 )
            {
                estimateTimeForPatient = 15;
            }
            estimateTime = estimateTime.Add(time).AddMinutes((nextNumber - 1) * estimateTimeForPatient);
            return estimateTime;
        }

        private DateTime GetNextPatientEstimateTime(int EstimateTimeForPatient, DateTime currentDate, int nextNumber)
        {           
            var estimateTime = currentDate.AddMinutes((nextNumber - 1) * EstimateTimeForPatient);
            return estimateTime;
        }
        public async Task<Response<HospitalAppointmentModel>> Delete(int id)
        {
            var response = InitErrorResponse();            
            var en = await _unitOfWork.HospitalAppointmentRepository.GetSingleAsync(id);
            if(en != null )
            {
                en.AppointmentStatus = AppointmentStatus.Cancel;
                en.UpdatedById = GetUserId();
                en.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.HospitalAppointmentRepository.Edit(en);
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

        public async Task<Response<HospitalAppointmentModel>> Edit(HospitalAppointmentModel modelToEdit)
        {
            var response = InitErrorResponse();
            
            if ( Validate(modelToEdit) )
            {
                
                var en = await _unitOfWork.HospitalAppointmentRepository.GetSingleAsync(modelToEdit.Id);
                var userId = GetUserId();

                if ( userId == en.CreatedById )
                {
                    var entity = GetEntityFromModel(modelToEdit);
                    entity.UpdatedById = userId;
                    entity.UpdatedDate = DateTime.UtcNow;

                    _unitOfWork.HospitalAppointmentRepository.Edit(this.RemoveChildEntity(entity));
                    _unitOfWork.Commit();

                    response = InitSuccessResponse(MessageConstant.Update);
                    response.Item = GetModelFromEntity(entity);
                }
                else
                    response.Message = MessageConstant.UserNotAllowed;
                    
                
            }
            else
                response.Message = MessageConstant.ValidationError;
            
            return response;
        }

        public async Task<Response<HospitalAppointmentDetailModel>> AddEditAppointmentDetailAsync(int id, HospitalAppointmentDetailModel appointmentDetail)
        {
            var response = InitErrorResponse<HospitalAppointmentDetailModel>();            
            
            var en = await _unitOfWork.HospitalAppointmentRepository.GetSingleAsync(id);
            var userId = GetUserId();

            if ( userId == en.CreatedById )
            {
                string message;
                var entity = Mapper.Map<HospitalAppointmentDetail>(appointmentDetail);
                entity.HospitalAppointment = null;
                entity.Patient = null;
                if(entity.Id == 0 )
                {
                    _unitOfWork.HospitalAppointmentDetailRepository.Add(entity);
                    message = MessageConstant.Create;
                }else
                {
                    _unitOfWork.HospitalAppointmentDetailRepository.Edit(entity);
                    message = MessageConstant.Update;
                }

                _unitOfWork.Commit();

                response = InitSuccessResponse<HospitalAppointmentDetailModel>(message);                    
            }
            else
                response.Message = MessageConstant.UserNotAllowed;

                  
           

            return response;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentModel>>> GetAllAsync(int pageIndex, int pageSize = Constant.PageSize)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            
            var result = await _unitOfWork.HospitalAppointmentRepository.GetAllAsync(pageIndex, pageSize, incl => incl.AppointmentDetails);
            response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            response.Item = Mapper.Map<PaginationSet<HospitalAppointmentModel>>(result);
            
            return response;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentModel>>> GetHospitalAppointmentAsync(int hospitalId, DateTime startDate, DateTime endDate, int pageIndex, int pageSize = 20, AppointmentStatus[] filter = null)
        {
            var response = InitErrorResponse(pageIndex, pageSize);
            if ( await IsUserAssignToHospital(hospitalId) )
            {
                
                var result = await _unitOfWork.HospitalAppointmentRepository.GetHospitalStaffAppointmentAsync(hospitalId, 0, startDate, endDate, pageIndex, pageSize, filter);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = Mapper.Map<PaginationSet<HospitalAppointmentModel>>(result);

            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }

            return response;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentModel>>> GetHospitalAppointmentByDoctorAsync(int hospitalId, int staffId, DateTime startDate, DateTime endDate, int pageIndex, int pageSize = 20, AppointmentStatus[] filter = null)
        {
            filter = filter ?? new AppointmentStatus[] { AppointmentStatus.Finish, AppointmentStatus.Pending, AppointmentStatus.Process, AppointmentStatus.Cancel };
            var response = InitErrorResponse(pageIndex, pageSize);
            if ( await IsUserAssignToHospital(hospitalId) )
            {

                var result = await _unitOfWork.HospitalAppointmentRepository.GetHospitalStaffAppointmentAsync(hospitalId, staffId, startDate, endDate, pageIndex, pageSize, filter);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = GetModelFromEntity(result);
                

            }
            else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }

            return response;
        }

        public async Task<Response<HospitalAppointmentModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            try
            {
                var en = await _unitOfWork.HospitalAppointmentRepository.GetHospitalAppointmentDetailAsync(id);

                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(en);
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentModel>>> GetUserAppointmentAsync(int userId = 0, int pageIndex = 1, int pageSize = 20, AppointmentStatus[] filter = null)
        {
            userId = userId == 0 ? GetUserId() : userId;
            filter = filter ?? new AppointmentStatus[] { AppointmentStatus.Process, AppointmentStatus.Pending, AppointmentStatus.Finish };
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                var data = await _unitOfWork.HospitalAppointmentRepository.GetUserAppointmentAsync(userId, filter, pageIndex, pageSize);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = GetModelFromEntity(data);
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<HospitalAppointmentModel> RescheduleAppointment(int id, DateTime appointmentDate)
        {
            throw new Exception();
        }

        public bool Validate(HospitalAppointmentModel modelToValidate)
        {
          
            return _validationDictionary.IsValid;
        }

        public async Task<Response<HospitalAppointmentDetailModel>> DeleteAppointmentDetailAsync(int id)
        {
            var response = InitErrorResponse<HospitalAppointmentDetailModel>();
            try
            {
                var en = await _unitOfWork.HospitalAppointmentDetailRepository.GetSingleAsync(x => x.Id == id, 
                                    includeProperties => includeProperties.HospitalAppointment);

                if ( en.HospitalAppointment.CreatedById == GetUserId() )
                {
                    _unitOfWork.HospitalAppointmentDetailRepository.Delete(en);
                    _unitOfWork.Commit();
                    response = InitSuccessResponse<HospitalAppointmentDetailModel>(MessageConstant.Delete);
                }
                else
                {
                    response.Message = MessageConstant.UserNotAllowed;
                }
            }catch(Exception ex )
            {
                response.Message = ex.Message;
            }

            return response;
            
        }

        public async Task<Response<HospitalAppointmentModel>> ProcessAppointment(int id)
        {
            var response = InitErrorResponse();
            var appt = await _unitOfWork.HospitalAppointmentRepository.GetSingleAsync(x => x.Id == id && x.AppointmentStatus == AppointmentStatus.Pending);
            if(appt == null )
            {
                response.Message = MessageConstant.NotFound;
                return response;
            }

            if ( await IsUserAssignToHospital(appt.HospitalId)){
                //update current appointment

                appt.AppointmentStatus = AppointmentStatus.Process;
                appt.AppointmentStarted = DateTime.UtcNow;
                appt.UpdatedDate = DateTime.UtcNow;
                appt.UpdatedById = GetUserId();
                _unitOfWork.HospitalAppointmentRepository.Edit(appt);
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Update);
                response.Item = GetModelFromEntity(appt);

                var notifications = await SendPendingAppointment(appt);
                if (notifications.Any())
                {
                    try
                    {
                        _unitOfWork.NotificationRepository.AddRange(notifications);
                        _unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        var mes = ex.InnerException.Message;
                    }

                }


               
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }
            

            return response;

        }

        // asyncronous method for sending appointment
        async Task<IList<Notification>> SendPendingAppointment(HospitalAppointment currentAppointment){
            //get all pending appointment related to the current appointment
            var currentDate = DateTime.UtcNow;
            var pendingAppointment = await _unitOfWork.HospitalAppointmentRepository
                .FindByAsync(x => x.AppointmentStatus == AppointmentStatus.Pending
                             && x.HospitalId == currentAppointment.HospitalId && x.MedicalStaffId == currentAppointment.MedicalStaffId
                             && x.QueueNumber > currentAppointment.QueueNumber
                        && x.AppointmentDate >= currentDate.StartOfDay() && x.AppointmentDate <= currentDate.EndOfDay());

            //define setting
            var setting = await _unitOfWork.HospitalMedicalStaffRepository.GetSingleAsync(x => x.HospitalId == currentAppointment.HospitalId
                                                                                         && x.MedicalStaffId == currentAppointment.MedicalStaffId, x => x.OperatingHours);

            //get detail staff and hospital
            var medicalStaff = await _unitOfWork.MedicalStaffRepository.GetStaffWithHospitalDetailAsync(currentAppointment.MedicalStaffId, false);
            var hospital = await _unitOfWork.HospitalRepository.GetSingleAsync(currentAppointment.HospitalId);
            var notifications = new List<Notification>();

            // Convert PendingAppointment to PendingList Model
            // to make entities untracked, because if not
            // entities will doing arbitary insert
            var pendingList = GetModelFromEntity(pendingAppointment.ToList());
            foreach (var appt in pendingList)
            {
                // just send pending appointment FCM
                // to registered user
                var item = GetEntityFromModel(appt);
                if (item.UserId != null)
                {
                    //update estime time
                    item.AppointmentDate = GetNextPatientEstimateTime(setting.EstimateTimeForPatient ?? 15, currentDate, item.QueueNumber);
                    item.Hospital = hospital;
                    item.MedicalStaff = medicalStaff;
                    item.CurrentQueueNumber = currentAppointment.QueueNumber;

                    var option = new FCMOption
                    {
                        TargetScreen = NotificationModel.APPOINTMENT_QUEUE_SCREEN,
                        JsonData = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            Formatting = Formatting.None,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }),
                        Priority = FCMOption.PriorityHigh

                    };


                    //send notification for all user devices
                    var content = ContentGenerator.CurrentAppointment(GetModelFromEntity<HospitalAppointment, HospitalAppointmentModel>(item));
                    var userDeviceIds = await _unitOfWork.FirebaseUserMapRepository.FindByAsync(x => x.UserId == item.UserId);


                    ISender sender = _senderFact.Create(SenderFactory.FCM);
                    await sender.SendMultipleAsync(userDeviceIds.Select(x => x.DeviceToken).ToList(),
                                                   content.Title, content.Body, option);
                    notifications.Add(new Notification
                    {
                        Id = 0,
                        TargetScreen = option.TargetScreen,
                        JsonData = option.JsonData,
                        Title = content.Title,
                        Message = content.Body,
                        ObjectId = item.Id,
                        UserId = item.UserId ?? 0,
                        IsRead = false,
                        CreatedById = GetUserId(),
                        UpdatedById = GetUserId(),
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    });
                }

            }

            return notifications;
        }

        public async Task<Response<HospitalAppointmentModel>> FinishAppointment(int id)
        {
            var response = InitErrorResponse();
            var appt = await _unitOfWork.HospitalAppointmentRepository.GetSingleAsync(x => x.Id == id);
            if ( appt == null )
            {
                response.Message = MessageConstant.NotFound;
                return response;
            }

            if ( await IsUserAssignToHospital(appt.HospitalId) )
            {

                if(appt.AppointmentStatus == AppointmentStatus.Process )
                {
                    //update status
                    appt.AppointmentStatus = AppointmentStatus.Finish;
                    appt.UpdatedById = GetUserId();
                    appt.UpdatedDate = DateTime.UtcNow;
                    appt.AppointmentFinished = DateTime.UtcNow;
                    appt.UpdatedById = GetUserId();
                    _unitOfWork.HospitalAppointmentRepository.Edit(appt);
                    _unitOfWork.Commit();

                    if(appt.UserId != null )
                    {
                        var userDeviceIds = await _unitOfWork.FirebaseUserMapRepository.FindByAsync(x => x.UserId == appt.UserId);
                        ISender sender = _senderFact.Create(SenderFactory.FCM);
                        var fcmPayload = GetFirebasePayload();

                        // ask user to fill doctor's review
                        var medicalStaff = await _unitOfWork.MedicalStaffRepository.GetStaffWithHospitalDetailAsync(appt.MedicalStaffId, false);

                        //generate json data
                        var item = new
                        {
                            HospitalAppointmentId = appt.Id,
                            Cover = medicalStaff.Images.Any()? medicalStaff.Images.First().Image.ImageUrl : "",
                            medicalStaff.Title,
                            medicalStaff.FirstName,
                            medicalStaff.LastName,
                            Specialist = medicalStaff.MedicalStaffSpecialists.Any()? medicalStaff.MedicalStaffSpecialists.FirstOrDefault().MedicalStaffSpecialist.Name : ""
                        };
                        var option = new FCMOption
                        {
                            TargetScreen = NotificationModel.DOCTOR_RATING_REQUEST,
                            JsonData = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                            {
                                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                Formatting = Formatting.None,
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            }),
                            Priority = FCMOption.PriorityHigh
                        };

                        var title = fcmPayload.Review.Title;
                        var content = fcmPayload.Review.Content;
                        await sender.SendMultipleAsync(userDeviceIds.Select(x => x.DeviceToken).ToList(),
                            title, content, option);

                        //save sending message to database
                        _unitOfWork.NotificationRepository.Add(new Notification
                        {
                            Id = 0,
                            TargetScreen = option.TargetScreen,
                            JsonData = option.JsonData,
                            Title = title,
                            Message = content,
                            ObjectId = appt.Id,
                            UserId = appt.UserId ?? 0,
                            IsRead = false,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            CreatedById = GetUserId(),
                            UpdatedById = GetUserId()
                        });
                        _unitOfWork.Commit();
                    }

                }
                response = InitSuccessResponse(MessageConstant.Update);
                response.Item = GetModelFromEntity(appt);
            }else
            {
                response.Message = MessageConstant.UserNotAllowed;
            }

            return response;
        }

        public async Task<Response<HospitalAppointmentModel>> CancelAppointment(int id, string reason)
        {
            var response = InitErrorResponse();
            var appt = await _unitOfWork.HospitalAppointmentRepository.GetSingleAsync(id);
            if(appt != null )
            {
                int userId = GetUserId();
                appt.AppointmentStatus = AppointmentStatus.Cancel;
                appt.CancelledReason = reason;
                appt.UpdatedById = userId;
                appt.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.HospitalAppointmentRepository.Edit(appt);
                if(await IsUserAssignToHospital(appt.HospitalId) )
                {
                    _unitOfWork.Commit();
                }else if(appt.UserId != null && appt.UserId == userId )
                {
                    _unitOfWork.Commit();
                }
                else
                {
                    response.Message = MessageConstant.UserNotAllowed;
                    return response;
                }
                response = InitSuccessResponse(MessageConstant.Update);
                response.Item = GetModelFromEntity(appt);
            }else
            {
                response.Message = MessageConstant.NotFound;
            }
            return response;

        }

        /// <summary>
        /// Gets the estimate appointment time async
        /// for queue already running
        /// </summary>
        /// <returns>The estimate async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<Response<HospitalAppointmentModel>> GetEstimateAsync(int id)
        {
            var response = InitErrorResponse();
            var appt = await _unitOfWork.HospitalAppointmentRepository.GetQuequeEstimateAsync(id);
            if( appt != null){
                //define setting
                var setting = await _unitOfWork.HospitalMedicalStaffRepository.GetSingleAsync(x => x.HospitalId == appt.HospitalId
                                                    && x.MedicalStaffId == appt.MedicalStaffId, x => x.OperatingHours);
                // update estimate date time
                appt.AppointmentDate = GetNextPatientEstimateTime(setting.EstimateTimeForPatient ?? 15, DateTime.UtcNow, appt.QueueNumber);
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(appt);
            }

            return response;

        }

       

        public HospitalAppointment RemoveChildEntity(HospitalAppointment entity)
        {
            entity.User = entity.UpdatedByUser = entity.CreatedByUser = null;
            entity.MedicalStaff = null;
            entity.Hospital = null;
            entity.AppointmentDetails = null;
            return entity;
        }

        public async Task<Response<PaginationSet<HospitalAppointmentModel>>> GetUserAppointmentNotRatedAsync(int userId = 0, int pageIndex = 1, int pageSize = 20, AppointmentStatus[] filter = null)
        {
            userId = userId == 0 ? GetUserId() : userId;
            filter = filter ?? new AppointmentStatus[] { AppointmentStatus.Process, AppointmentStatus.Pending, AppointmentStatus.Finish };
            var response = InitErrorResponse(pageIndex, pageSize);
            try
            {
                var data = await _unitOfWork.HospitalAppointmentRepository.GetUserAppointmentNotRatedAsync(userId, filter, pageIndex, pageSize);
                response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
                response.Item = GetModelFromEntity(data);
            }
            catch ( Exception ex )
            {
                response.Message = ex.Message;
            }
            return response;
        }


        #region "Hide Method"
        Response<HospitalAppointmentModel> IHospitalAppointmentService.Edit(HospitalAppointmentModel modelToEdit)
        {
            throw new NotImplementedException();
        }

        Response<HospitalAppointmentModel> IReadWriteService<HospitalAppointmentModel, HospitalAppointment>.Create(HospitalAppointmentModel modelToCreate)
        {
            throw new NotImplementedException();
        }

        Response<HospitalAppointmentModel> IReadWriteService<HospitalAppointmentModel, HospitalAppointment>.Edit(HospitalAppointmentModel modelToEdit)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
