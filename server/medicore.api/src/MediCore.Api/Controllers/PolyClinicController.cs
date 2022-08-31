using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Class of polyclinic
    /// </summary>
    [Route("[controller]")]
    public class PolyClinicController : Controller
    {
        private readonly IPolyClinicService _polyClinicService;
        /// <summary>
        /// Initialize method
        /// </summary>
        /// <param name="polyClinicService"></param>
        public PolyClinicController(IPolyClinicService polyClinicService)
        {
            _polyClinicService = polyClinicService;
        }
        
        /// <summary>
        /// Method to get all polyclinic
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _polyClinicService.GetAllAsync(1, 1000);
            if ( result.Success )
                return Ok(result.Item.Items);
            else
                return BadRequest(result.Message);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _polyClinicService.Dispose();

        }

    }
}
