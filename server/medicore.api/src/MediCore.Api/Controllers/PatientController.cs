using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services.Abstract;
using dna.core.auth;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using dna.core.libs.Validation;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Class controller of Patient
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAuthenticationService _authService;
        /// <summary>
        /// Initilize method
        /// </summary>
        /// <param name="patientService"></param>
        /// <param name="authService"></param>
        public PatientController(IPatientService patientService, IAuthenticationService authService)
        {
            _patientService = patientService;
            _authService = authService;
        }

        private void AssignModelState()
        {
            _patientService.Initialize(new ModelStateWrapper(ModelState));
        }
        /// <summary>
        /// Get user's patient
        /// </summary>
        /// <param name="pageIndex">Current Page</param>
        /// <param name="pageSize">Page Width</param>
        /// <returns></returns>
        [HttpGet("{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetAllPatient(int pageIndex, int pageSize)
        {
            PatientStatus[] status = new PatientStatus[] { PatientStatus.Active, PatientStatus.Death };
            int userId = _authService.GetUserId() ?? 0;
            var result = await _patientService.GetUserPatientAsync(pageIndex, pageSize, status);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Get patient detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _patientService.GetSingleAsync(id);
            if ( result.Success )
                return Ok(result.Item);
            else
                return BadRequest(result.Message);
        }

        /// <summary>
        /// Create patient
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]PatientModel param)
        {
            AssignModelState();

            var res = _patientService.Create(param);
            if ( res.Success )
                return Ok(res.Item);
            else
                return BadRequest(res.Message);
        }

        /// <summary>
        /// Edit patient
        /// </summary>
        /// <param name="id">Patient Id</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PatientModel param)
        {
            AssignModelState();

            param.Id = id;
            var res = _patientService.Edit(param);
            if ( res.Success )
                return Ok(res.Item);
            else
                return BadRequest(res.Message);
        }

        /// <summary>
        /// InActive the patient
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _patientService.Delete(id);
            if ( res.Success )
                return Ok(res.Item);
            else
                return BadRequest(res.Message);
        }

    }

    
}
