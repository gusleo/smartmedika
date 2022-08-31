using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using dna.core.auth.Model;
using dna.core.auth;
using dna.core.service.Infrastructure;
using dna.core.auth.Entity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Roles user bussiness logic
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.SuperAdmin)]
    
    public class RolesController : Controller
    {
        private readonly IAuthenticationService _auth;
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="auth"></param>
        public RolesController(IAuthenticationService auth)
        {
            _auth = auth;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet("Roles")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _auth.GetAvailableRoleAsync();
            return Ok(result);
        }
        
        /// <summary>
        /// Get user roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("Detail/{userId}")]
        public async Task<IActionResult> Detail (int userId)
        {
            var result = await _auth.GetUserRoleAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Assign roles to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPut("AssignToUser/{userId}")]
        public async Task<IActionResult> Edit(int userId, [FromBody] string[] roles)
        {
            var result = await _auth.AssignRoleToUserAsync(userId, roles);
            if ( result.Suceess )
            {
                return Ok(roles);
            }
            else
            {
                return BadRequest(result.Message);
            }
           
        }

        
    }
}
