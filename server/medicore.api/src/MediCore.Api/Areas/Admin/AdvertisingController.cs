using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using dna.core.service.Services.Abstract;
using dna.core.service.Models;
using dna.core.libs.Validation;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Controller of advertising
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.SuperAdmin)]
    public class AdvertisingController : Controller
    {
        private readonly IAdvertisingService _advertisingService;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="advertisingService"></param>

        public AdvertisingController(IAdvertisingService advertisingService)
        {
            _advertisingService = advertisingService;

        }

        /// <summary>
        /// Gets the adversting.
        /// </summary>
        /// <returns>The adversting.</returns>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        [HttpGet("{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetAdversting(int pageIndex, int pageSize){
            var response = await _advertisingService.GetAllAsync(pageIndex, pageSize);
            if (response.Success)
                return Ok(response.Item);
            return BadRequest(response.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id){
            var response = await _advertisingService.GetSingleAsync(id);
            if (response.Success)
                return Ok(response.Item);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Post slider
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] AdvertisingModel param)
        {
            AssignModelState();

            var response = _advertisingService.Create(param);
            if (response.Success)
                return Ok(response.Item);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Update adversting
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="param">Parameter.</param>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AdvertisingModel param){
            AssignModelState();
            var response = _advertisingService.Edit(param);
            if (response.Success)
                return Ok(response.Item);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Delete the specified id.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">Identifier.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            var response = await _advertisingService.Delete(id);
            if (response.Success)
                return Ok(response.Item);
            return BadRequest(response.Message);
        }

        void AssignModelState()
        {
            _advertisingService.Initialize(new ModelStateWrapper(ModelState));
        }
    }
}
