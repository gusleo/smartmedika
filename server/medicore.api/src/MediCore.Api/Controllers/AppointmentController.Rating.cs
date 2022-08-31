using MediCore.Service.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.Controllers
{
    public partial class AppointmentController
    {
        /// <summary>
        /// Get hospital rating
        /// </summary>
        /// <param name="hid">Hospital Id</param>
        /// <param name="pageIndex">Index page</param>
        /// <param name="pageSize">page width</param>
        /// <returns></returns>
        [HttpGet("Rating/{hid}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetHospitalRating(int hid, int pageIndex, int pageSize)
        {
            var response = await _ratingService.GetHospitalRatingAsync(hid, pageIndex, pageSize);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Delete rating
        /// </summary>
        /// <param name="id">Rating Id</param>
        /// <returns></returns>
        [HttpDelete("Rating/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var response = await _ratingService.Delete(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Crate new rating
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Rating")]
        public IActionResult CreateRating([FromBody] HospitalAppointmentRatingModel param)
        {
            AssignModelState();
            var response = _ratingService.Create(param);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Update rating by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("Rating/{id}")]
        public IActionResult UpdateRating(int id, [FromBody] HospitalAppointmentRatingModel param)
        {
            AssignModelState();
            param.Id = id;
            var response = _ratingService.Edit(param);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }
    }
}
