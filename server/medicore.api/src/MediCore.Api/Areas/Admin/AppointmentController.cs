using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services.Abstract;
using MediCore.Api.InputParam;
using dna.core.libs.Validation;
using MediCore.Service.Services;
using dna.core.auth;
using dna.core.service.Infrastructure;
using MediCore.Service.Model;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Class controller of hospital appointment
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public partial class AppointmentController : Controller
    {
        private readonly IHospitalAppointmentService _appointmentService;
        private readonly IHospitalAppointmentRatingService _ratingService;  

        /// <summary>
        /// Initialize method
        /// </summary>
        /// <param name="appointmentService"></param>
        /// <param name="ratingService"></param>
        public AppointmentController(IHospitalAppointmentService appointmentService, IHospitalAppointmentRatingService ratingService)
        {
            _appointmentService = appointmentService;
            _ratingService = ratingService;
        }

        private void AssignModelState()
        {
            _appointmentService.Initialize(new ModelStateWrapper(ModelState));
        }

        /// <summary>
        /// Get hospital appointment for date range
        /// </summary>        
        /// <param name="param">Input parameter</param>
        [HttpPost("Hospital")]
        public async Task<IActionResult> GetAppointmentAsync([FromBody] HospitalAppointmentParam param)
        {
            var result = await _appointmentService.GetHospitalAppointmentAsync(param.HospitalId, param.StartDate, param.EndDate, param.PageIndex, param.PageSize, param.Filter);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);

        }

        /// <summary>
        /// Get doctor appointment specific to hospital for date range
        /// </summary>        
        /// <param name="param">Input parameter</param>
        [HttpPost("Doctor")]
        public async Task<IActionResult> GetAppointmentByDoctor([FromBody] HospitalAppointmentParam param)
        {
            var result = await _appointmentService.GetHospitalAppointmentByDoctorAsync(param.HospitalId, param.StaffId, param.StartDate, param.EndDate, 
                                param.PageIndex, param.PageSize, param.Filter);

            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);

        }

        /// <summary>
        /// Booking doctor for non registered user
        /// </summary>
        /// <param name="param">Input param</param>
        /// <returns><see cref="HospitalAppointmentModel"/></returns>
        [HttpPost("BookingDoctor")]
        public async Task<IActionResult> BookingDoctor([FromBody] HospitalAppointmentModel param)
        {
            AssignModelState();
            var result = await _appointmentService.CreateForNonRegisteredUserAsync(param);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Process current appointment and sending notification
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns></returns>
        [HttpGet("Process/{id}")]
        public async Task<IActionResult> ProcessAppointment(int id)
        {
            var result = await _appointmentService.ProcessAppointment(id);
            if ( result.Success )
            {
                return Ok(result.Item);
            }else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Finish current appointment and sending notification
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns></returns>
        [HttpGet("Finish/{id}")]
        public async Task<IActionResult> FinishAppointment(int id)
        {
            var result = await _appointmentService.FinishAppointment(id);
            if ( result.Success )
            {
                return Ok(result.Item);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Cancel appointment
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <param name="reason">Cancelled Reason</param>
        /// <returns></returns>
        [HttpPut("Cancel/{id}")]
        public async Task<IActionResult> CancelAppointment(int id, [FromBody] string reason)
        {
            var result = await _appointmentService.CancelAppointment(id, reason);
            if ( result.Success )
            {
                return Ok(result.Item);
            }else
            {
                return BadRequest(result.Message);
            }
        }
        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _appointmentService.Dispose();
            _ratingService.Dispose();
        }


    }
}
