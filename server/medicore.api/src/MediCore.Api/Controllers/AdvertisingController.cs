using dna.core.data.Infrastructure;
using dna.core.libs.Cache;
using dna.core.libs.Validation;
using dna.core.service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Controller for advertising
    /// </summary>
    [Route("[controller]")]
    [Cache(Duration = 24 * 60)]
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
        /// Get active slider
        /// </summary>
        /// <returns></returns>
        [HttpGet("Slider")]
       
        public async Task<IActionResult> GetSlider()
        {
            var response = await  _advertisingService.GetAdvertisingByTypeAndStatusAsync(new AdvertisingType[] { AdvertisingType.Slider }, new Status[] { Status.Active });
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        private void AssignModelState()
        {
            _advertisingService.Initialize(new ModelStateWrapper(ModelState));
        }

    }
}
