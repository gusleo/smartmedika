using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using dna.core.libs.Upload.Abstract;
using dna.core.libs.Upload;
using Microsoft.AspNetCore.Hosting;
using MediCore.Service.Model.Custom;
using dna.core.service.Services;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using dna.core.service.Services.Abstract;
using System.IO;
using dna.core.libs.Validation;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Controller of fileuploader
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public class FileController : Controller
    {
        private readonly IFileServices _fileServices;
        private readonly IImageService _imageServices;

        /// <summary>
        /// File manipulation
        /// </summary>
        /// <param name="fileServices"></param>
        /// <param name="imageServices"></param>
        public FileController(IFileServices fileServices, IImageService imageServices)
        {
            _fileServices = fileServices;
            _imageServices = imageServices;
        }
        /// <summary>
        /// Upload image multiple
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("images")]
        public async Task<IActionResult> UploadImageAsync(IList<IFormFile> files)
        {

            var response = await _fileServices.UploadAsync(files);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
            
        }
        /// <summary>
        /// Upload single image
        /// </summary>
        /// <param name="cover">Is primary image</param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut("image/{cover}")]
        public async Task<IActionResult> UploadImageAsync(bool cover, IFormFile file)
        {

            var response = await _fileServices.UploadAsync(file);
            if ( response.Success )
            {
                var item = response.Item;
                AssignModelState();
                var result =_imageServices.Create(new dna.core.service.Models.ImageModel
                {
                    Id = 0,
                    FileExtension = Path.GetExtension(item.FileName),
                    FileName = item.FileName,
                    ImageUrl = item.Path,
                    Description = String.Empty,
                    IsPrimary = cover
                });
                return Ok(result.Item);
            }
                
            else
                return BadRequest(response.Message);

        }

        private void AssignModelState()
        {
            _imageServices.Initialize(new ModelStateWrapper(ModelState));
        }
    }
}
