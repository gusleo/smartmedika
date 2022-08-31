using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using dna.core.data.Infrastructure;
using dna.core.libs.Cache;

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// API about Country, Region and Regency
    /// </summary>
    [Route("[controller]")]
    [Cache(Duration = 24 * 60)]
    public class GeoLocationController : Controller
    {
        private readonly ICountryService _geoService;
        private readonly IRegionService _regionService;
        private readonly IRegencyService _regencyService;
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="geoService"></param>
        /// <param name="regionService"></param>
        /// <param name="regencyService"></param>
        public GeoLocationController(ICountryService geoService, IRegionService regionService, IRegencyService regencyService)
        {
            _geoService = geoService;
            _regionService = regionService;
            _regencyService = regencyService;
        }

        /// <summary>
        /// Get all region by clue
        /// </summary>
        /// <param name="countryId">Country Id</param>       
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Page Width</param>
        /// <param name="clue">Search text</param>
        /// <returns></returns>
        [HttpGet("Region/{countryId}/{page}/{pageSize}")]
        [HttpGet("Region/{countryId}/{page}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetAllRegionAsync(int countryId, int page, int pageSize, string clue = "")
        {
            List<Status> status = new List<Status> { Status.Active };
            
            var result = await _regionService.GetRegionByCountryAndClueAsync(countryId, status.ToArray(), page, pageSize, clue);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }
        /// <summary>
        /// Get region list
        /// </summary>
        /// <param name="id">CountryId</param>
        /// <returns></returns>
        [HttpGet("Region/{id}")]
        public async Task<IActionResult> GetRegionByCountryAsync(int id)
        {
            List<Status> status = new List<Status> { Status.Active };
            var result = await _regionService.GetRegionByCountryAndClueAsync(id, status.ToArray(), 1, 10000);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }


        /// <summary>
        /// Get region and regency list
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <returns></returns>
        [HttpGet("RegionWithRegency/{countryId}")]
        public async Task<IActionResult> GetRegionDetailAsync(int countryId)
        {
            var result = await _regionService.GetRegionByCountryAsync(countryId, true);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get list of regency
        /// </summary>
        /// <param name="regionId">RegionId</param>
        /// <returns></returns>
        [HttpGet("Regency/{regionId}")]
        public async Task<IActionResult> GetRegencyAsync(int regionId)
        {
            var result = await _regencyService.GetRegencyByRegionAsync(regionId);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);

        }
        /// <summary>
        /// Get list regency include region by clue
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="clue"></param>
        /// <returns></returns>
        [HttpGet("Regency/{countryId}/{page}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetRegencyDetailByCountry(int countryId, int page, int pageSize, string clue)
        {
            var status = new List<Status> { Status.Active };
            var result = await _regencyService.GetRegencyDetailByCountryAsync(countryId, status.ToArray(), page, pageSize, clue);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);

        }

        

        /// <summary>
        /// Get list of country incude Active/InActive
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Country/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetActiveCountryAsync(int pageIndex, int pageSize)
        {
            var result = await _geoService.GetCountryByStatusAsync(pageIndex, pageSize);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);

        }

        

        /// <summary>
        /// Get UTC by country
        /// </summary>
        /// <param name="countryId">Country Id</param>        
        /// <param name="page">Page Index</param>
        /// <param name="pageSize">Page Width</param>
        /// <returns></returns>
        [HttpGet("utc/{countryId}/{includeInActive}/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllUtcByCountry(int countryId, int page, int pageSize)
        {
            List<Status> status = new List<Status> { Status.Active };
            
            var result = await _geoService.GetAllUtcByCountryAsync(countryId, status.ToArray(), page, pageSize);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _geoService.Dispose();

        }
    }
}
