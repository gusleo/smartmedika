using dna.core.libs.Cache;
using MediCore.Service.Model;
using Microsoft.AspNetCore.Authorization;
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
        /// Get Staff By user
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        
        [Authorize]
        [Cache(IgnoreCache = true)]
        [HttpGet("Favorite/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetStaffFavoriteUser(int pageIndex, int pageSize)
        {
            var results = await _favoriteServices.GetAllByUserAsync(pageIndex, pageSize);
            if ( results.Success )
                return Ok(results.Item);
            else
                return BadRequest(results.Message);
        }

        /// <summary>
        /// Create Favorite Staff
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [Authorize]
        [HttpPost("Favorite")]
        public IActionResult CreateStaffFavorite([FromBody] MedicalStaffFavoriteModel model)
        {
            AssignModelState();
            var results = _favoriteServices.Create(model);
            if ( results.Success )
                return Ok(results.Item);
            else
                return BadRequest(results.Message);
        }

        /// <summary>
        /// Delete Staff Favorite
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>

        [Authorize]
        [HttpDelete("Favorite/{staffId}")]
        public async Task<IActionResult> DeleteStaffFavorite(int staffId)
        {
           
            var results = await _favoriteServices.DeleteByStaffIdAsync(staffId);
            if ( results.Success )
                return Ok(results.Item);
            else
                return BadRequest(results.Message);
        }
    }
}
