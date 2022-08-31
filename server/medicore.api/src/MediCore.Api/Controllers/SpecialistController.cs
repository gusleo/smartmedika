using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services;
using dna.core.libs.Cache;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Class of specialist
    /// </summary>
    [Route("[controller]")]
    [Cache(Duration = 24 * 60)]
    public class SpecialistController : Controller
    {
        private readonly IMedicalSpecialistService _medicalSpecialistService;
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="medicalSpecialistService"></param>
        public SpecialistController(IMedicalSpecialistService medicalSpecialistService)
        {
            _medicalSpecialistService = medicalSpecialistService;
        }
        
        /// <summary>
        /// Method to get all specialist
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _medicalSpecialistService.GetAllAsync(1, 1000);
            if ( result.Success )
                return Ok(result.Item.Items);
            else
                return BadRequest(result.Message);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _medicalSpecialistService.Dispose();

        }

    }
}
