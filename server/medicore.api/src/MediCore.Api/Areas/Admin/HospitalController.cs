using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services;
using MediCore.Service.Model;
using dna.core.libs.Validation;
using System;
using MediCore.Data.Infrastructure;
using dna.core.service.Services;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Controller of Hospital
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]

    public class HospitalController : Controller
    {
        private readonly IHospitalService _hospitalService;
        
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="hospitalService"></param>
        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;            
            
        }
        private void AssignModelState()
        {
            _hospitalService.Initialize(new ModelStateWrapper(ModelState));
        }

        
        /// <summary>
        /// Method to get all hospital
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("{pageIndex}/{pageSize}")]
        [HttpGet("{pageIndex}/{pageSize}/{regionId}")]
        [HttpGet("{pageIndex}/{pageSize}/{regionId}/{clue}")]
        public async Task<IActionResult> Get(int pageIndex, int pageSize, int regionId = 0, string clue = "")
        {
            
            var result = await _hospitalService.FindHospitalByFilterAsync(pageIndex, pageSize, regionId, clue);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// GEt hospital detail
        /// </summary>
        /// <param name="id">Hospital Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _hospitalService.GetHospitalDetailAsync(id);
            if ( result.Success )
            {
                return Ok(result.Item);
            }else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Method to add hospital
        /// </summary>
        /// <param name="param"><see cref="HospitalModel"/> param</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]HospitalModel param)
        {
            AssignModelState();
            var result = await _hospitalService.Create(param);
            if ( result.Success )                
               return Ok(result.Item);
            else
                return BadRequest(result.Message);
                
        }

        /// <summary>
        /// Method to edit hospital
        /// </summary>
        /// <param name="id">int HospitalId</param>
        /// <param name="param"><see cref="HospitalModel"/> param</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]HospitalModel param)
        {
            AssignModelState();
            param.Id = id;
            var result = await _hospitalService.Edit(param);
            if ( result.Success )
            {                
                return Ok(result.Item);
            }
               

            return BadRequest(result.Message);
            
        }     
        

        /// <summary>
        /// Method to change hospital status
        /// </summary>
        /// <param name="id">Hospital Id</param>
        /// <param name="status">Hospital Status Enum</param>
        /// <returns></returns>
        [HttpPut("Status/{id}")]
        public async Task<IActionResult> ChangeHospitalStatus(int id, [FromBody]HospitalStatus status)
        {
            var result = await _hospitalService.ChangeHospitalStatusAsync(id, status);
            if ( result.Success )
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete hospital
        /// </summary>
        /// <param name="id">int HospitalId</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _hospitalService.Delete(id);
            if ( result.Success )
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update hospital metadata
        /// </summary>
        /// <param name="id">Hospital Id</param>
        /// <param name="param">Hospital Metadata</param>
        /// <returns></returns>
        [HttpPut("Metadata/{id}")]
        public async Task<IActionResult> UpdateHospitalMetadataAsync(int id, [FromBody]HospitalMetadataModel param)
        {
            var response = await _hospitalService.UpdateHospitalMetadataAsync(id, param);
            if ( response.Success )
                return Ok(response.Message);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Upload image to hospital
        /// </summary>
        /// <param name="id">HospitalId</param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut("CoverImage/{id}")]
        public async Task<IActionResult> UpdateHospitalImage(int id, IFormFile file)
        {
            
               
            var response = await _hospitalService.UploadHospitalImageCoverAsync(id, file);
            if ( response.Success )
            {
                return Ok(response.Item);
            }else
            {
                return BadRequest(response.Message);
            }
        }


        /// <summary>
        /// Get hospital assocaited to user
        /// </summary>
        /// <returns></returns>
        [HttpGet("AssociatedUser")]
        public async Task<IActionResult> GetHospitalAssociatedUserAsync()
        {
            var response = await _hospitalService.GetHospitalAssocatedUserAsync();
            if ( response.Success )
            {
                return Ok(response.Item);
            }else
            {
                return BadRequest(response.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _hospitalService.Dispose();

        }
    }
}
