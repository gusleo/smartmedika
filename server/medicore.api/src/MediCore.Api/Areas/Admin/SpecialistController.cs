using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MediCore.Api.CustomEntity;
using MediCore.Service.Services;
using dna.core.libs.Stream.Option;
using dna.core.service.Services;
using MediCore.Service.Model;
using dna.core.libs.Validation;
using dna.core.service.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Class of doctor
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public class SpecialistController : Controller
    {
        private readonly IMedicalSpecialistService _medicalSpecialistService;
        private readonly IFileServices _fileService;

        /// <summary>
        /// Initialize of controller
        /// </summary>
        /// <param name="medicalSpecialistService">Medical Specialist services <see cref="IMedicalSpecialistService"/></param>
        /// <param name="fileService">File services <see cref="IFileServices"/></param>
        public SpecialistController(IMedicalSpecialistService medicalSpecialistService,
            IFileServices fileService)
        {
            _medicalSpecialistService = medicalSpecialistService;
            _fileService = fileService;

            //init 
            _medicalSpecialistService.Initialize(new ModelStateWrapper(this.ModelState));
        }

        /// <summary>
        /// Import data spesialist from excel/csv file
        /// </summary>
        /// <returns>return type <see cref="OkObjectResult"/></returns>
        [HttpPost("ImportData")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult ImportData()
        {
            
            //read all uploaded file
            IFormFileCollection files = Request.Form.Files;

            if( files.Count > 0 )
            {
                //set option for how to read file
                StreamAdvanceOption option = 
                    new StreamAdvanceOption(firstRowAsColumnName: true);

                
                try
                {
                    //get the first file and extract the data
                    //with spesific class
                    IFormFile file = files.FirstOrDefault();
                    IList<SpecialistExcelEntity> result = _fileService
                        .ImportData<SpecialistExcelEntity>(file, option);

                    //fetch data into model
                    List<MedicalStaffSpecialistModel> importedData = new List<MedicalStaffSpecialistModel>();
                    foreach(SpecialistExcelEntity en in result )
                    {
                        importedData.Add(new MedicalStaffSpecialistModel
                        {
                            Name = en.Name,
                            Description = en.Description,
                            Alias = en.Alias
                        });
                    }
                    //insert
                    _medicalSpecialistService.CreateRange(importedData);
                    return Ok(result);
                }catch(Exception ex )
                {
                    return BadRequest(ex.Message);
                }
                
            }else
            {
                return BadRequest("File not found");
            }
        }

        
        /// <summary>
        /// Method to insert specialist
        /// </summary>ff
        /// <param name="param">Input param <see cref="MedicalStaffSpecialistModel"/></param>
        /// <returns><see cref="OkObjectResult"/></returns>
        [HttpPost]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult Insert([FromBody] MedicalStaffSpecialistModel param)
        {

            if ( ModelState.IsValid )
            {
                var response = _medicalSpecialistService.Create(param);
                if ( response.Success )
                {
                    return Ok(response.Item);
                }

            }

            //as far all is failed
            return BadRequest();
        }

        /// <summary>
        /// Update specialist data
        /// </summary>
        /// <param name="id">Input SpecialistId</param>
        /// <param name="param">Input <see cref="MedicalStaffSpecialistModel"/></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public IActionResult Update(int id, [FromBody] MedicalStaffSpecialistModel param)
        {
            if ( ModelState.IsValid )
            {
                param.Id = id;
                var response = _medicalSpecialistService.Edit(param);
                if ( response.Success )
                {
                    return Ok(response.Item);
                }
            }

            //as far all is invalid
            return BadRequest();
        }

        /// <summary>
        /// Delete specialist for spesific id
        /// </summary>
        /// <param name="id">Specialist id</param>
        /// <returns></returns>        
        [HttpDelete("{id}")]
        [Authorize(Roles = MembershipConstant.SuperAdmin)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _medicalSpecialistService.Delete(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get all specialist
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _medicalSpecialistService.GetAllAsync(1, 1000);
            if ( response.Success )
                return Ok(response.Item.Items);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get specialist detail
        /// </summary>
        /// <param name="id">Specialist Id</param>
        /// <returns></returns>
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var response = await _medicalSpecialistService.GetSingleAsync(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }


        /// <inheritdoc/>        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _medicalSpecialistService.Dispose();
        }


    }
}
