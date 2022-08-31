using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCore.Service.Services;
using dna.core.libs.Validation;
using MediCore.Service.Model;
using dna.core.libs.Cache;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Controller of rating
    /// </summary>
    [Route("[controller]")]
    [Cache(Duration = 24 * 60)]
    public class RatingController : Controller
    {
        private readonly IHospitalAppointmentRatingService _ratingService;
        readonly ILogger<RatingController> _logger;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="ratingService"></param>
        /// <param name="logger"></param>
        public RatingController(IHospitalAppointmentRatingService ratingService, ILogger<RatingController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        private void AssignModelState()
        {
            _ratingService.Initialize(new ModelStateWrapper(ModelState));
        }

        /// <summary>
        /// Post rating to spesific appointment
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] HospitalAppointmentRatingModel param)
        {
            AssignModelState();
            var result = _ratingService.Create(param);
            if ( result.Success )
            {
                return Ok(result.Item);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Get user reviews
        /// </summary>
        /// <returns>The review by user.</returns>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        [Authorize]
        [HttpGet("User/{pageIndex}/{pageSize}")]
        [Cache(IgnoreCache = true)]
        public async Task<IActionResult> GetRatingByUser(int pageIndex, int pageSize){
            var result = await _ratingService.GetUserRatingAsync(pageIndex, pageSize);
            if (result.Success)
            {
                return Ok(result.Item);
            }
            else
            {
                return BadRequest(result.Message);
            }
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
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Get Hospital Total Rating
        /// </summary>
        /// <param name="id">Hospital Id.</param>
        /// <returns></returns>
        [HttpGet("HospitalTotal/{id}")]
        public async Task<IActionResult> GetTotalHospitalRatingAsync(int id)
        {
            var result = await _ratingService.GetTotalHospitalRatingAsync(id);
            if ( result.Success )
            {
                return Ok(new {
                    Rating1 = result.Item.Where(x => x.Rating == 1).Count(),
                    Rating2 = result.Item.Where(x => x.Rating == 2).Count(),
                    Rating3 = result.Item.Where(x => x.Rating == 3).Count(),
                    Rating4 = result.Item.Where(x => x.Rating == 4).Count(),
                    Rating5 = result.Item.Where(x => x.Rating == 5).Count(),
                    Total = result.Item.Count()
                });
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Get medical staff rating
        /// </summary>
        /// <param name="id">Staff Id</param>
        /// <param name="pageIndex">Curent Page</param>
        /// <param name="pageSize">Page Width</param>
        /// <returns></returns>
        [HttpGet("Staff/{id}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetStaffRatingAsync(int id, int pageIndex, int pageSize)
        {
            var result = await _ratingService.GetStaffRatingAsync(id, pageIndex, pageSize);
            if ( result.Success )
            {
                return Ok(result.Item);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Get Staff Total Rating
        /// </summary>
        /// <param name="id">Staff Id.</param>
        /// <returns></returns>
        [HttpGet("StaffTotal/{id}")]
        public async Task<IActionResult> GetTotalStaffRatingAsync(int id)
        {
            var result = await _ratingService.GetTotalStaffRatingAsync(id);
            if ( result.Success )
            {
                return Ok(new
                {
                    Rating1 = result.Item.Where(x => x.Rating == 1).Count(),
                    Rating2 = result.Item.Where(x => x.Rating == 2).Count(),
                    Rating3 = result.Item.Where(x => x.Rating == 3).Count(),
                    Rating4 = result.Item.Where(x => x.Rating == 4).Count(),
                    Rating5 = result.Item.Where(x => x.Rating == 5).Count(),
                    Total = result.Item.Count()
                });
            }
            else
            {
                return BadRequest(result.Message);
            }
        }


    }
}
