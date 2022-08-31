using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services.Abstract;
using MediCore.Service.Model;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using dna.core.libs.Validation;
using dna.core.data.Infrastructure;
using dna.core.libs.Cache;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// API about Country, Region and Regency
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    [Cache(IgnoreSuperAdmin = false)]
    public class GeoLocationController : Controller
    {
        private readonly ICountryService _geoService;
        private readonly IRegionService _regionService;
        private readonly IRegencyService _regencyService;
        private readonly IUTCTimeBaseService _UTCTimeBaseService;
        
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="geoService"></param>
        /// <param name="regionService"></param>
        /// <param name="regencyService"></param>
        /// <param name="UTCTimeBaseService"></param>
        public GeoLocationController(ICountryService geoService, IRegionService regionService, IRegencyService regencyService, IUTCTimeBaseService UTCTimeBaseService)
        {
            _geoService = geoService;
            _regionService = regionService;
            _regencyService = regencyService;
            _UTCTimeBaseService = UTCTimeBaseService;
        }

        /// <summary>
        /// Get all region by clue
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <param name="includeInActive">Include InActive Region</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Page Width</param>
        /// <param name="clue">Search text</param>
        /// <returns></returns>
        [HttpGet("Region/{countryId}/{includeInActive}/{page}/{pageSize}")]
        [HttpGet("Region/{countryId}/{includeInActive}/{page}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetAllRegionAsync(int countryId, bool includeInActive, int page, int pageSize, string clue = "")
        {
            List<Status> status = new List<Status> { Status.Active };

            if ( includeInActive )
                status.Add(Status.InActive);

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
        public async Task<IActionResult> GetRegionByCountry(int id)
        {
            List<Status> status = new List<Status> { Status.Active };
            var result = await _regionService.GetRegionByCountryAndClueAsync(id, status.ToArray(), 1, 10000);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Edit Region
        /// </summary>
        /// <param name="id">Country Id</param>
        /// <param name="model">Country attribute</param>
        /// <returns></returns>
        [HttpPut("Region/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult EditRegion(int id, [FromBody] RegionModel model)
        {
            model.Id = id;
            AssignModelState();
            var result = _regionService.Edit(model);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        private void AssignModelState()
        {
            _regencyService.Initialize(new ModelStateWrapper(ModelState));
            _regionService.Initialize(new ModelStateWrapper(ModelState));
            _geoService.Initialize(new ModelStateWrapper(ModelState));
            _UTCTimeBaseService.Initialize(new ModelStateWrapper(ModelState));
        }

        /// <summary>
        /// Add Region
        /// </summary>
        /// <param name="model">country attribute</param>
        /// <returns></returns>
        [HttpPost("Region")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult AddRegion([FromBody] RegionModel model)
        {
            AssignModelState();
            var result = _regionService.Create(model);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Change status region
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("Region/Status/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> ChangeStatusRegion(int id, [FromBody] Status status)
        {
            var result = await _regionService.ChangeRegionStatusAsync(id, status);
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
        public async Task<IActionResult> GetRegionDetail(int countryId)
        {
            var result = await _regionService.GetRegionByCountryAsync(countryId, true);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Edit Region
        /// </summary>
        /// <param name="id">Country Id</param>
        /// <param name="model">Country attribute</param>
        /// <returns></returns>
        [HttpPut("Regency/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult EditRegency(int id, [FromBody] RegencyModel model)
        {
            model.Id = id;
            AssignModelState();
            var result = _regencyService.Edit(model);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        

        /// <summary>
        /// Add Region
        /// </summary>
        /// <param name="model">country attribute</param>
        /// <returns></returns>
        [HttpPost("Regency")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult AddRegency([FromBody] RegencyModel model)
        {
            AssignModelState();
            var result = _regencyService.Create(model);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Change status region
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("Regency/Status/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> ChangeStatusRegency(int id, [FromBody] Status status)
        {
            var result = await _regencyService.ChangeRegencyStatusAsync(id, status);
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
        public async Task<IActionResult> GetRegency(int regionId)
        {
            var result = await _regencyService.GetRegencyByRegionAsync(regionId);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);

        }
        /// <summary>
        /// Get list of regency with region spesific by country
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <param name="includeInActive">Include inactive regency (true/false)</param>
        /// <param name="page">Current page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="clue">regency name to search</param>
        /// <returns></returns>
        [HttpGet("Regency/{countryId}/{includeInActive}/{page}/{pageSize}")]
        [HttpGet("Regency/{countryId}/{includeInActive}/{page}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetRegencyDetailByCountry(int countryId, bool includeInActive, int page, int pageSize, string clue = "")
        {
            var status = new List<Status> { Status.Active };
            if ( includeInActive )
                status.Add(Status.InActive);

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
        [HttpGet("Country/All/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetCountry(int pageIndex, int pageSize)
        {
            var result = await _geoService.GetAllAsync(pageIndex, pageSize);
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
        [HttpGet("Country/Active/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetActiveCountry(int pageIndex, int pageSize)
        {
            var result = await _geoService.GetCountryByStatusAsync(pageIndex, pageSize);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);

        }
       
        /// <summary>
        /// Edit country
        /// </summary>
        /// <param name="id">Country Id</param>
        /// <param name="model">Country attribute</param>
        /// <returns></returns>
        [HttpPut("Country/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult EditCountry(int id, [FromBody] CountryModel model)
        {
            model.Id = id;
            AssignModelState();
            var result = _geoService.Edit(model);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Add country
        /// </summary>
        /// <param name="model">country attribute</param>
        /// <returns></returns>
        [HttpPost("Country")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult AddCountry([FromBody] CountryModel model)
        {
            AssignModelState();
            var result = _geoService.Create(model);
            if(result.Success)
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }


        /// <summary>
        /// Delete country
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Country/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var result = await _geoService.Delete(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Change country status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("Country/Status/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> ChangeStatusCountry(int id, [FromBody] Status status)
        {
            var result = await _geoService.ChangeCountryStatusAsync(id, status);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get UTC by country
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <param name="includeInActive">Include InActive UTC</param>
        /// <param name="page">Page Index</param>
        /// <param name="pageSize">Page Width</param>
        /// <returns></returns>
        [HttpGet("UTC/{countryId}/{includeInActive}/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllUtcByCountry(int countryId, bool includeInActive, int page, int pageSize)
        {
            List<Status> status = new List<Status> { Status.Active };
            if ( includeInActive )
                status.Add(Status.InActive);

            var result = await _geoService.GetAllUtcByCountryAsync(countryId, status.ToArray(), page, pageSize);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get list of utc incude Active/InActive
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("UTC/All/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetUTC(int pageIndex, int pageSize)
        {
            var result = await _UTCTimeBaseService.GetAllAsync(pageIndex, pageSize);
            if (result.Success)
                return Ok(result.Item);
            else
                return BadRequest(result.Message);

        }


        /// <summary>
        /// Edit UTC
        /// </summary>
        /// <param name="id">Country Id</param>
        /// <param name="model">Country attribute</param>
        /// <returns></returns>
        [HttpPut("UTC/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult EditUTC(int id, [FromBody] UTCTimeBaseModel model)
        {
            model.Id = id;
            AssignModelState();
            var result = _UTCTimeBaseService.Edit(model);
            if (result.Success)
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Add UTC
        /// </summary>
        /// <param name="model">country attribute</param>
        /// <returns></returns>
        [HttpPost("UTC")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult AddUTC([FromBody] UTCTimeBaseModel model)
        {
            AssignModelState();
            var result = _UTCTimeBaseService.Create(model);
            if (result.Success)
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }


        /// <summary>
        /// Delete UTC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("UTC/{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> DeleteUTC(int id)
        {
            var result = await _UTCTimeBaseService.Delete(id);
            if (result.Success)
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _geoService.Dispose();

        }

    }
}
