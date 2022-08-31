using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MediCore.Api.Areas.Admin
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
    }
}
