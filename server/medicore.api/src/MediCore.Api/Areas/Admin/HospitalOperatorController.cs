using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using MediCore.Service.Services.Abstract;
using MediCore.Data.Infrastructure;
using System.Threading.Tasks;
using MediCore.Service.Model;
using dna.core.libs.Validation;
using MediCore.Service.Services;

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// API for hospital operator
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public class HospitalOperatorController : Controller
    {
        private readonly IHospitalOperatorService _operatorService;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="operatorService"></param>
        public HospitalOperatorController(IHospitalOperatorService operatorService)
        {
            _operatorService = operatorService;
        }
        
        /// <summary>
        /// Get list of operator staff spesific by hospital
        /// </summary>
        /// <param name="hospitalId">Current hospital id</param>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize"></param>
        /// <param name="clue">Operator staff name to search</param>
        /// <returns></returns>
        [HttpGet("{hospitalId}/{pageIndex}/{pageSize}")]
        [HttpGet("{hospitalId}/{pageIndex}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, string clue = "")
        {
            var status = new List<HospitalStaffStatus>() { HospitalStaffStatus.Active}; // just load active
            var result = await _operatorService.GetHospitalOperatorAsync(hospitalId, pageIndex, pageSize, status.ToArray(), clue);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get list of operator staff not registered to spesific hospital
        /// </summary>
        /// <param name="hospitalId">Current hospital ID</param>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize"></param>
        /// <param name="clue">Operator staff name to search</param>
        /// <returns></returns>
        [HttpGet("NonRegistered/{hospitalId}/{pageIndex}/{pageSize}")]
        [HttpGet("NonRegistered/{hospitalId}/{pageIndex}/{pageSize}/{clue}")]
        public async Task<IActionResult> GetNonRegisteredHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, string clue = "")
        {
            var status = new List<HospitalStaffStatus>() { HospitalStaffStatus.Active };
            var result = await _operatorService.GetNonHospitalOperatorAsync(hospitalId, pageIndex, pageSize, status.ToArray(), clue);
            if (result.Success)
                return Ok(result.Item);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Get hospital associated to user
        /// </summary>
        /// <returns>The hospital user.</returns>
        /// <param name="userId">User identifier.</param>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetHospitalUser(int userId){
            var results = await _operatorService.GetOperatorHospitalAsync(userId);
            if (results.Success)
                return Ok(results.Item);
            return BadRequest(results.Message);
        }

        /// <summary>
        /// Associate user to hospital.
        /// </summary>
        /// <returns>The hospital to user.</returns>
        /// <param name="userId">User Id</param>
        /// <param name="hospitalOperators">Hospital operators.</param>
        [HttpPut("user/{userId}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> AssociatedHospitalToUser(int userId, [FromBody]List<HospitalOperatorModel> hospitalOperators){
            var results = await _operatorService.ReAssignUserToHospital(userId, hospitalOperators);
            if (results.Success)
                return Ok(results.Item);
            return BadRequest(results.Message);
        }

        /// <summary>
        /// Assign user to hospital
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]       
        public async Task<IActionResult> Create([FromBody] HospitalOperatorModel param)
        {
            AssignModelState();
            var result = await _operatorService.Create(param);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Edit operator staff
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut(("{id}"))]
        public async Task<IActionResult> Edit(int id, [FromBody] HospitalOperatorModel param)
        {
            AssignModelState();
            param.Id = id;
            var result = await _operatorService.Edit(param);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete Operator
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _operatorService.Delete(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        private void AssignModelState()
        {
            _operatorService.Initialize(new ModelStateWrapper(ModelState));
        }
    }
}
