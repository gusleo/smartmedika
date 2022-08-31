using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services.Abstract;
using dna.core.libs.Validation;
using MediCore.Service.Model;
using dna.core.auth;
using dna.core.service.Infrastructure;
using System.Threading.Tasks;
using MediCore.Api.InputParam;
using Microsoft.AspNetCore.Authorization;
using MediCore.Service.Services;

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Controller of Hospital Appointment
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    public partial class AppointmentController : Controller
    {
        private readonly IHospitalAppointmentService _appointmentService;
        private readonly IAuthenticationService _authService;
        private readonly IHospitalAppointmentRatingService _ratingService;
        

       /// <summary>
       /// Initialize controller
       /// </summary>
       /// <param name="appointmentService"></param>
       /// <param name="ratingService"></param>
       /// <param name="authService"></param>
        public AppointmentController(IHospitalAppointmentService appointmentService, IHospitalAppointmentRatingService ratingService, IAuthenticationService authService)
        {
            _appointmentService = appointmentService;
            _authService = authService;
            _ratingService = ratingService;
        }
        private void AssignModelState()
        {
            _appointmentService.Initialize(new ModelStateWrapper(ModelState));
            _ratingService.Initialize(new ModelStateWrapper(ModelState));
        }

        /// <summary>
        /// Create appointment for login user
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserAppointmentViewModel param)
        {

            var appt = new HospitalAppointmentModel
            {
                Id = param.Id,
                AppointmentDate = param.AppointmentDate,
                MedicalStaffId = param.MedicalStaffId,
                HospitalId = param.HospitalId
            };
            int? userId = _authService.GetUserId();
            appt.UserId = userId ?? 0;

            if(param.PatientProblems != null )
            {
                List<HospitalAppointmentDetailModel> patiens = new List<HospitalAppointmentDetailModel>();
                foreach(var item in param.PatientProblems )
                {
                    patiens.Add(new HospitalAppointmentDetailModel
                    {
                        PatientId = item.PatientId,
                        Problem = item.Problems
                    });
                }
                appt.AppointmentDetails = patiens;
            }

            AssignModelState();
            var result = await _appointmentService.Create(appt);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Edit appointment for login user
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] HospitalAppointmentModel param)
        {
            AssignModelState();
            param.Id = id;
            param.UserId = _authService.GetUserId() ?? 0;

            var result = _appointmentService.Edit(param);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Add/Edit appointment detail
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("Detail/{id}")]
        public async Task<IActionResult> AddEditAppointmentDetail(int id, [FromBody] HospitalAppointmentDetailModel param)
        {
            if ( ModelState.IsValid )
            {
                var result = await _appointmentService.AddEditAppointmentDetailAsync(id, param);
                if ( result.Success )
                    return Ok(result.Item);
                else
                    return BadRequest(result.Message);
            }else
            {
                return BadRequest(MessageConstant.ValidationError);
            }
        }

        /// <summary>
        /// Delete appointment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _appointmentService.Delete(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete appointment detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Detail/{id}")]
        public async Task<IActionResult> DeleteAppointmentDetail(int id)
        {
            var result = await _appointmentService.DeleteAppointmentDetailAsync(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }


        /// <summary>
        /// Get list appointment for login user
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("{pageIndex}/{pageSize}")]
        public async Task<IActionResult> Get(int pageIndex, int pageSize)
        {
            var result = await _appointmentService.GetUserAppointmentAsync(pageIndex: pageIndex, pageSize: pageSize);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

       /// <summary>
       /// Get User Appointment Not Rated
       /// </summary>
       /// <param name="pageIndex">Page Index</param>
       /// <param name="pageSize">Page Size</param>
       /// <returns></returns>
        [HttpGet("NotRated/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetUserAppointmentNotRatedAsync(int pageIndex, int pageSize)
        {
            var result = await _appointmentService.GetUserAppointmentNotRatedAsync(pageIndex: pageIndex, pageSize: pageSize);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }
        /// <summary>
        /// Get appointment detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _appointmentService.GetSingleAsync(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get appointment estimate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Estimate/{id}")]
        public async Task<IActionResult> GetEstimateAsync(int id)
        {
            var result = await _appointmentService.GetEstimateAsync(id);
            if (result.Success)
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _appointmentService.Dispose();
        }
    }
}
