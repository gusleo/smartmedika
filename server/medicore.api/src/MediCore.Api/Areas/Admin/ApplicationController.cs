using Microsoft.AspNetCore.Mvc;
using dna.core.service.Models;
using dna.core.data.Infrastructure;
using dna.core.libs.Validation;
using MediCore.Service.Services.Extend.Abstract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Controller of application
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public class ApplicationController : Controller
    {
       
        private readonly IMenuBuilderService _menuService;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="menuservice"></param>
        public ApplicationController(IMenuBuilderService menuservice)
        {
            _menuService = menuservice;
           
        }

        private void AssignModelState()
        {
            _menuService.Initialize(new ModelStateWrapper(ModelState));
        }

        /// <summary>
        /// Get Menu by menu type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("Menu/Tree/{type}")]
        public async Task<IActionResult> Get(MenuType type)
        {
            var result = await _menuService.GetMenuByTypeAsync(type);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get parent menu by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("Menu/Parent/{type}")]
        public async Task<IActionResult> GetParentMenu(MenuType type)
        {
            var result = await _menuService.GetAllParentMenuAsync(type);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get menu detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Menu/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _menuService.GetSingleAsync(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get menu by type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Menu/{type}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetMenuByType(MenuType type, int pageIndex, int pageSize)
        {
            var result = await _menuService.GetMenuByTypeAsync(type, pageIndex, pageSize);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }


        /// <summary>
        /// Create new Menu
        /// </summary>
        /// <param name="param"></param>
        [HttpPost("Menu")]
        public IActionResult Post([FromBody]TreeMenuModel param)
        {
            this.AssignModelState();
            var result = _menuService.Create(param);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Edit Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        [HttpPut("Menu/{id}")]
        public IActionResult Put(int id, [FromBody]TreeMenuModel param)
        {
            AssignModelState();
            param.Id = id;
            var result = _menuService.Edit(param);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete Menu
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("Menu/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _menuService.Delete(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }


    }
}
