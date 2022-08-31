using dna.core.auth;
using dna.core.service.Infrastructure;
using MediCore.Data.Entities;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Model;
using MediCore.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCore.Service.Services
{
    public class NotificationServices : ReadWriteService<NotificationModel, Notification>, INotificationService
    {
        public NotificationServices(IAuthenticationService authService, IMediCoreUnitOfWork unitOfWork) : base(authService, unitOfWork)
        {

        }

        public Response<NotificationModel> Create(NotificationModel modelToCreate)
        {
            var response = InitErrorResponse();
            if(Validate(modelToCreate)){
                
                modelToCreate.CreatedDate = modelToCreate.UpdatedDate = DateTime.UtcNow;
                modelToCreate.CreatedById = modelToCreate.UpdatedById = GetUserId();
                var en = GetEntityFromModel(modelToCreate);
                _unitOfWork.NotificationRepository.Add(RemoveChildEntity(en));
                _unitOfWork.Commit();
                response = InitSuccessResponse(MessageConstant.Create);
                response.Item = GetModelFromEntity(en);

            }else{
                response.Message = MessageConstant.UserNotAllowed;
            }

            return response;
        }

        public async Task<Response<NotificationModel>> Delete(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.NotificationRepository.GetSingleAsync(id);
            if(en != null )
            {
                _unitOfWork.NotificationRepository.Delete(en);
                _unitOfWork.Commit();
            }
            else
            {
                response.Message = MessageConstant.NotFound;
            }
            return response;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Response<NotificationModel> Edit(NotificationModel modelToEdit)
        {
            var response = InitSuccessResponse(MessageConstant.Update);
            modelToEdit.UpdatedDate = DateTime.UtcNow;
            modelToEdit.UpdatedById = GetUserId();
            var en = GetEntityFromModel(modelToEdit);
            _unitOfWork.NotificationRepository.Add(RemoveChildEntity(en));
            _unitOfWork.Commit();
            response.Item = GetModelFromEntity(en);
           
            return response;
        }

        public async Task<Response<PaginationSet<NotificationModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
        {
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            var results = await _unitOfWork.NotificationRepository.GetAllAsync(pageIndex, pageSize);
            response.Item = GetModelFromEntity(results);
            return response;
        }

        public async Task<Response<PaginationSet<NotificationModel>>> GetNotificationByObjectId(int objectId, int pageIndex, int pageSize)
        {
            var userId = GetUserId();
            var response = InitSuccessResponse(pageIndex, pageSize, MessageConstant.Load);
            var results = await _unitOfWork.NotificationRepository.FindByAsync(x => x.ObjectId == objectId && x.UserId == userId, pageIndex, pageSize);

            // update all unread notification to read
            var notificationUnread = results.Items.Where(x => x.IsRead == false).Select(y => { y.IsRead = true; return y; }).ToList();
            foreach(var notif in notificationUnread){
                _unitOfWork.NotificationRepository.Edit(notif);
            }
            _unitOfWork.Commit();
            // end of commit

            response.Item = GetModelFromEntity(results);
            return response;
        }

        public async Task<Response<NotificationModel>> GetSingleAsync(int id)
        {
            var response = InitErrorResponse();
            var en = await _unitOfWork.NotificationRepository.GetSingleAsync(id);
            if ( en != null )
            {
                response = InitSuccessResponse(MessageConstant.Load);
                response.Item = GetModelFromEntity(en);
            }
            else
            {
                response.Message = MessageConstant.NotFound;
            }
            return response;
        }

        public async Task<Response<IList<NotificationModel>>> GetUserUnReadNotification()
        {
            var userId = GetUserId();
            var response = InitSuccessListResponse(MessageConstant.Load);
            var results = await _unitOfWork.NotificationRepository.FindByAsync(x => x.UserId == userId && x.IsRead == false);
            
            var items = GetModelFromEntity(results.ToList());
            var group = items.ToList().GroupBy(x => new { x.TargetScreen, x.ObjectId }).Select(xl => new NotificationModel
            {
                TargetScreen = xl.First().TargetScreen,
                ObjectId = xl.First().ObjectId,
                TotalNotification = xl.Count()
            });

            response.Item = group.ToList();
            return response;
        }

        public Notification RemoveChildEntity(Notification entity)
        {
            entity.CreatedByUser = entity.UpdatedByUser = entity.User = null;
            return entity;
        }

        public bool Validate(NotificationModel modelToValidate)
        {
            return _validationDictionary.IsValid;
        }
    }
}
