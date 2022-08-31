using dna.core.libs.Validation;
using dna.core.service.Models;
using dna.core.service.Services;
using dna.core.service.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Firebase controller
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    public class FirebaseController : Controller
    {
        
        private readonly IFirebaseUserMapUserService _firebaseService;

        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="service">Firebase service</param>
        public FirebaseController(IFirebaseUserMapUserService service)
        {
            _firebaseService = service;
        }

        /// <summary>
        /// Register token for current device
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] FirebaseUserMapModel param)
        {
            AssignState();
            var result = _firebaseService.Create(param);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete token
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            AssignState();
            var result = await _firebaseService.Delete(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }



        private void AssignState()
        {
            _firebaseService.Initialize(new ModelStateWrapper(ModelState));
        }

        

    }
}
