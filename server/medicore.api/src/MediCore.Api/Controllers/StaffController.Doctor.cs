using dna.core.libs.Cache;
using MediCore.Api.InputParam;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.Controllers
{
    public partial class StaffController
    {
        /// <summary>
        /// Get staff doctor by hospital
        /// </summary>
        /// <param name="id">int hospitalId</param>
        /// <param name="pageIndex">int pageIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <returns></returns>
        [HttpGet("Doctor/{id}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetDoctorByHospital(int id, int pageIndex, int pageSize)
        {
            var response = await _medicalService.GetActiveDoctorByHospitalAsync(id, pageIndex, pageSize);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Find nearest doctor associated with hospital location
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Doctor/Nearest")]
        [Cache(IgnoreCache = true)]
        public async Task<IActionResult> GetNearestDoctorAsync([FromBody]NearestLocationParam param)
        {
            var response = await _medicalService.FindNearestDoctorReferenceByHospitalAsync(param.Longitude, param.Latitude, param.Radius, param.PolyClinicIds, 
                    param.Clue, param.PageIndex, param.PageSize);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get doctor operating hours on every hospital by day of week
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        [HttpGet("Doctor/OperatingHours/{staffId}/{dayOfWeek}")]
        public async Task<IActionResult> GetStaffOperatingHoursByDate(int staffId, int dayOfWeek)
        {
            var response = await _medicalService.GetStaffOperatingHoursByDateAsync(staffId, dayOfWeek);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);

        }
    }
}
