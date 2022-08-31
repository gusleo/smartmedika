using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.libs.Validation;
using MediCore.Service.Model;
using MediCore.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Notification API
    /// </summary>
    
    [Authorize]
    [Route("[controller]")]
    public class NotificationController : Controller
    {
        readonly INotificationService _notificationService;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="notificationService"></param>
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Create the notification.
        /// </summary>
        /// <returns>Notification</returns>
        /// <param name="model">Model.</param>
        [HttpPost]
        public IActionResult Create([FromBody] NotificationModel model){
            AssignModelState();
            var response = _notificationService.Create(model);
            if (response.Success)
                return Ok(response.Item);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Get Notification by Object Id
        /// </summary>
        /// <param name="id">Object Id</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns></returns>
        [HttpGet("User/{id}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetByObjectId(int id, int pageIndex, int pageSize)
        {
            var response = await _notificationService.GetNotificationByObjectId(id, pageIndex, pageSize);
            if ( response.Success )
            {
                return Ok(response.Item);
            }
                
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Get User Total Notification
        /// </summary>
        /// <returns></returns>
        [HttpGet("Total")]
        public async Task<IActionResult> GetNotificationTotal()
        {
            var response = await _notificationService.GetUserUnReadNotification();
            if ( response.Success )
                return Ok(response.Item);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _notificationService.Dispose();
        }

        void AssignModelState(){
            _notificationService.Initialize(new ModelStateWrapper(ModelState));
        }

    }
}