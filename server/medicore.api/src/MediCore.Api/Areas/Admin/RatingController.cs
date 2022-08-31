using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using MediCore.Service.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Controller of Hospital & Doctor rating
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public class RatingController : Controller
    {
        private readonly IHospitalAppointmentRatingService _ratingService;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="ratingService"></param>
        public RatingController(IHospitalAppointmentRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        /// <summary>
        /// Get hospital rating
        /// </summary>
        /// <param name="hospitalId">Staff Id</param>
        /// <param name="pageIndex">Curent Page</param>
        /// <param name="pageSize">Page Width</param>
        /// <returns></returns>
        [HttpGet("Hospital/{id}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetHospitalRatingAsync(int hospitalId, int pageIndex, int pageSize)
        {
            var result = await _ratingService.GetHospitalRatingAsync(hospitalId, pageIndex, pageSize);
            if ( result.Success )
            {
                return Ok(result.Item);
            }else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Get medical staff rating
        /// </summary>
        /// <param name="staffId">Staff Id</param>
        /// <param name="pageIndex">Curent Page</param>
        /// <param name="pageSize">Page Width</param>
        /// <returns></returns>
        [HttpGet("Staff/{id}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetStaffRatingAsync(int staffId, int pageIndex, int pageSize)
        {
            var result = await _ratingService.GetStaffRatingAsync(staffId, pageIndex, pageSize);
            if ( result.Success )
            {
                return Ok(result.Item);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        
    }
}
