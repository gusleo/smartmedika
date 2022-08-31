using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services.Abstract;
using MediCore.Service.Model;
using MediCore.Api.InputParam;
using dna.core.libs.Validation;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using System.Collections.Generic;
using MediCore.Data.Infrastructure;
using dna.core.service.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Class to manage medical staff
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public partial class StaffController : Controller
    {
        private readonly IMedicalStaffService _medicalService;
        private readonly IFileServices _fileService;

        /// <summary>
        /// Init class of StaffController
        /// </summary>
        /// <param name="medicalService"><see cref="IMedicalStaffService"/></param>
        /// <param name="fileServices"><see cref="IFileServices"/></param>
        public StaffController(IMedicalStaffService medicalService, IFileServices fileServices)
        {
            _medicalService = medicalService;
            _fileService = fileServices;
        }

        private void AssignModelState()
        {
            _medicalService.Initialize(new ModelStateWrapper(ModelState));
        }

        

        /// <summary>
        /// Get staff from ID
        /// </summary>
        /// <param name="id">int Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _medicalService.GetSingleAsync(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        
        /// <summary>
        /// Assign staff to hospital
        /// </summary>
        /// <param name="param">Input Parameter</param>
        /// <returns></returns>
        [HttpPost("AssignToHospital")]
        public async Task<IActionResult> AssignStaffToHospitalAsync([FromBody] AssignStaffToHospitalParam param)
        {
            var result = await _medicalService.AssignToHospitalAsync(param.HospitalId, param.StaffIds, param.IsDeleteFromHospital);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Set Operating Hours for Medical Staff to Hospital
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="staffId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("OperatingHours/{hospitalId}/{staffId}")]
        public async Task<IActionResult> SetOperatingHoursAsync(int hospitalId, int staffId, [FromBody] HospitalStaffOperatingHoursAndSettingParam param)
        {
            
            var result = await _medicalService.SetOperatingHoursAsync(hospitalId, staffId, param.EstimateTimeForPatient, param.operatingHours);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get Staff operating hours
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        [HttpGet("OperatingHours/{hospitalId}/{staffId}")]
        public async Task<IActionResult> GetStaffOperatingHoursAsync(int hospitalId, int staffId)
        {
            var result = await _medicalService.GetStaffOperatingHoursAsync(hospitalId, staffId);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Upload image to staff
        /// </summary>
        /// <param name="id">HospitalId</param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut("CoverImage/{id}")]
        public async Task<IActionResult> UpdateStaffImage(int id, IFormFile file)
        {

            var coll = Request.Form.Files;
            var response = await _medicalService.UploadStaffImageCoverAsync(id, file);
            if ( response.Success )
            {
                return Ok(response.Item);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _medicalService.Dispose();

        }
    }
}
