using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services;
using MediCore.Api.InputParam;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using dna.core.libs.Cache;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Controller of Hospital
    /// </summary>
    [Route("[controller]")]
    [Cache(Duration = 24 * 60)]
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

        /// <summary>
        /// Get all hospital by paging
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("{pageIndex}/{pageSize}")]
        public async Task<IActionResult> Get(int pageIndex, int pageSize)
        {            
            var result = await _hospitalService.GetActiveHospitalAsync(pageIndex, pageSize);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get hospital detil by hospital Id
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
        /// Find nearest hospital
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Nearest")]
        public async Task<IActionResult> NearestHospital([FromBody] NearestLocationParam param)
        {
            var result = await _hospitalService.FindNearestHospitalAsync(param.Longitude, param.Latitude, param.Radius,
                                                                         param.PolyClinicIds, param.PageIndex, param.PageSize, param.Clue);
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
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _hospitalService.Dispose();

        }



    }
}
