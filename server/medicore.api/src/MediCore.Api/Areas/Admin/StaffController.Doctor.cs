using dna.core.auth.Infrastructure;
using dna.core.service.Infrastructure;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.Areas.Admin
{
    public partial class StaffController
    {


        /// <summary>
        /// Get all doctor by hospital
        /// </summary>
        /// <param name="hospitalId">int HospitalId</param>
        /// <param name="pageIndex">int pageIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="clue">search keyword</param>
        /// <returns></returns>

        [HttpGet("Doctor/Registered/{hospitalId}/{pageIndex}/{pageSize}")]
        [HttpGet("Doctor/Registered/{hospitalId}/{pageIndex}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetAllDoctorByHospitalAsync(int hospitalId, int pageIndex, int pageSize, string clue = "")
        {
            var response = await _medicalService.GetAllDoctorByHospitalAsync(hospitalId, pageIndex, pageSize, clue: clue);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get All Doctor Operating Hours
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="clue"></param>
        /// <returns></returns>
        [HttpGet("Doctor/OperatingHours/{hospitalId}/{pageIndex}/{pageSize}")]
        [HttpGet("Doctor/OperatingHours/{hospitalId}/{pageIndex}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetActiveDoctorWithOperatingHoursByHospitalAsync(int hospitalId, int pageIndex, int pageSize, string clue = "")
        {
            var response = await _medicalService.GetActiveDoctorByHospitalAsync(hospitalId, pageIndex, pageSize, includeOperatingHours: true, clue: clue);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }
        /// <summary>
        /// Get all non registered doctor to hospital sorted by distance
        /// </summary>
        /// <param name="hospitalId">Current hospitalId</param>
        /// <param name="pageIndex">current page number</param>
        /// <param name="pageSize">Page width</param>
        /// <param name="clue">Search keyword</param>
        /// <returns></returns>
        [HttpGet("Doctor/NonRegistered/{hospitalId}/{pageIndex}/{pageSize}")]
        [HttpGet("Doctor/NonRegistered/{hospitalId}/{pageIndex}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetNonRegisteredDoctorAsync(int hospitalId, int pageIndex, int pageSize, string clue = "")
        {
            var types = new MedicalStaffType[] { MedicalStaffType.Doctor };
            var status = new MedicalStaffStatus[] { MedicalStaffStatus.Active };
            var response = await _medicalService.GetAvailableStaffSortByDistanceAsync(hospitalId, 50, types, status, clue);
            if ( response.Success )
            {
                return Ok(response.Item);
            }
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get all doctor
        /// </summary>
        /// <param name="pageIndex">int pageIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="clue">Search keyword</param>
        /// <returns></returns>

        [HttpGet("Doctor/{pageIndex}/{pageSize}")]
        [HttpGet("Doctor/{pageIndex}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetAllDoctorAsync(int pageIndex, int pageSize, string clue = "")
        {
            
            var response = await _medicalService.GetAllDoctorByClueAsync(pageIndex, pageSize, clue);
            
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Register doctor
        /// </summary>
        /// <param name="param"><see cref="MedicalStaffModel"/> param</param>
        /// <returns></returns>
        [HttpPost("Doctor")]
        public IActionResult RegisterDoctor([FromBody]MedicalStaffModel param)
        {
            AssignModelState();
            param.StaffType = MedicalStaffType.Doctor;
            param.Status = MedicalStaffStatus.Pending;
            var response = _medicalService.Create(param);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Edit doctor
        /// </summary>
        /// <param name="id">Staff Id</param>
        /// <param name="param">Input parameter</param>
        /// <returns></returns>
        [HttpPut("Doctor/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult EditDoctor(int id, [FromBody]MedicalStaffModel param)
        {
            AssignModelState();
            param.StaffType = MedicalStaffType.Doctor;           
            param.Id = id;
            var response = _medicalService.Edit(param);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);

        }

        /// <summary>
        /// Change doctor status
        /// </summary>
        /// <param name="id">Staff Id</param>
        /// <param name="status">doctor status</param>
        /// <returns></returns>
        [HttpPut("Doctor/Status/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> ChangeDoctorStatus(int id, [FromBody] MedicalStaffStatus status)
        {
            
            var response = await _medicalService.ChangeStaffStatusAsync(id, status);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);

        }



    }
}
