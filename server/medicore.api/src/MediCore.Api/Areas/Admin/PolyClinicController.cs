using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediCore.Service.Services;
using MediCore.Api.InputParam;
using Microsoft.AspNetCore.Http;
using dna.core.libs.Stream.Option;
using MediCore.Api.CustomEntity;
using MediCore.Service.Model;
using dna.core.service.Services;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Controller of PolyClinic
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public class PolyClinicController : Controller
    {
        private readonly IPolyClinicService _polyClinicService;
        private readonly IFileServices _fileService;
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="polyClinicService">DI <see cref="IPolyClinicService"/></param>
        /// <param name="fileService">DI <see cref="IFileServices"/></param>
        public PolyClinicController(IPolyClinicService polyClinicService, IFileServices fileService)
        {
            _polyClinicService = polyClinicService;
            _fileService = fileService;
        }

        /// <summary>
        /// Method to add specialist to Polyclinic
        /// </summary>
        /// <remarks>
        /// This method used to mapping between Polyclinic and specialist
        /// (one to many)
        /// </remarks>
        /// <param name="model">input parameter <see cref="AddSpecialistToPolyClinicParam"/></param>       
        /// <returns></returns>
        [HttpPost("AddSpecialist")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult AddSpecialistToPolyClinic([FromBody] AddSpecialistToPolyClinicParam model)
        {
            var response  = _polyClinicService
                                .AddSpecialistToPolyClinic(model.PolyClinicId, model.SpecialistIds);
            if(response.Success == true )
            {
                return Ok(response.Item);
            }
            return BadRequest(response.Message);
        }
        /// <summary>
        /// Import data polyclinic from excel/csv file
        /// </summary>
        /// <returns>return type <see cref="OkObjectResult"/></returns>
        [HttpPost("ImportData")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult ImportData()
        {

            //read all uploaded file
            IFormFileCollection files = Request.Form.Files;

            if ( files.Count > 0 )
            {
                //set option for how to read file
                StreamAdvanceOption option =
                    new StreamAdvanceOption(firstRowAsColumnName: true);


                try
                {
                    //get the first file and extract the data
                    //with spesific class
                    IFormFile file = files.FirstOrDefault();
                    IList<PoliclinicExcelEntity> result = _fileService
                        .ImportData<PoliclinicExcelEntity>(file, option);

                    //fetch data into model
                    List<PolyClinicModel> importedData = new List<PolyClinicModel>();
                    foreach ( PoliclinicExcelEntity en in result )
                    {
                        importedData.Add(new PolyClinicModel
                        {
                            Name = en.Name
                           
                        });
                    }
                    //insert
                    _polyClinicService.CreateRange(importedData);
                    return Ok(result);
                }
                catch ( Exception ex )
                {
                    return BadRequest(ex.Message);
                }

            }
            else
            {
                return BadRequest("File not found");
            }
        }
        
        /// <summary>
        /// Get all polyclinic
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _polyClinicService.GetAllAsync(1, 100);
            if ( response.Success )
                return Ok(response.Item.Items);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get polyclinic by clue
        /// </summary>
        /// <param name="clue">search string</param>
        /// <returns></returns>
        [HttpGet("{clue}")]
        public async Task<IActionResult> GetByClueAsync(string clue)
        {
            var response = await _polyClinicService.GetByClueAsync(clue);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get polyclinic detail
        /// </summary>
        /// <param name="id">PolyClinic Id</param>
        /// <returns></returns>
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetSingleAsync(int id)
        {
            var response = await _polyClinicService.GetSingleAsync(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }



        /// <summary>
        /// Add new polyclinic
        /// </summary>
        /// <param name="param"><see cref="PolyClinicModel"/> param</param>
        [HttpPost]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult Post([FromBody]PolyClinicModel param )
        {
            var response = _polyClinicService.Create(param);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Update polyclinic by Id
        /// </summary>
        /// <param name="id">int PolyClinicId</param>
        /// <param name="param"><see cref="PolyClinicModel"/> param</param>
        [HttpPut("{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult Update(int id, [FromBody]PolyClinicModel param)
        {
            param.Id = id;
            var response = _polyClinicService.Edit(param);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Delete polyclinic by Id
        /// </summary>
        /// <param name="id">int PolyClinicId</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _polyClinicService.Delete(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _polyClinicService.Dispose();

        }

    }
}
