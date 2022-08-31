using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services.Abstract;
using MediCore.Service.Model;
using MediCore.Data.Infrastructure;
using dna.core.service.Infrastructure;
using dna.core.libs.Cache;
using Microsoft.AspNetCore.Authorization;
using dna.core.libs.Validation;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Class to manage medical staff
    /// </summary>
    [Route("[controller]")]
    [Cache(Duration = 24 * 60)]
    public partial class StaffController : Controller
    {
        private readonly IMedicalStaffService _medicalService;
        private readonly IMedicalStaffFavoriteService _favoriteServices;


        /// <summary>
        /// Init class of StaffController
        /// </summary>
        /// <param name="medicalService"><see cref="IMedicalStaffService"/> medicalService</param>
        /// <param name="favoriteServices"><see cref="IMedicalStaffFavoriteService"/>favoriteServices</param>
        public StaffController(IMedicalStaffService medicalService, IMedicalStaffFavoriteService favoriteServices)
        {
            _medicalService = medicalService;
            _favoriteServices = favoriteServices;
        }        

        /// <summary>
        /// Get staff from ID
        /// </summary>
        /// <param name="id">int Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _medicalService.GetSingleDetailAsync(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
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
            if (result.Success)
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }


        
        
        private void AssignModelState(){
            _medicalService.Initialize(new ModelStateWrapper(ModelState));
            _favoriteServices.Initialize(new ModelStateWrapper(ModelState));
        }
        /// <inheritdoc/>        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _medicalService.Dispose();
            _favoriteServices.Dispose();
        }
    }
}
