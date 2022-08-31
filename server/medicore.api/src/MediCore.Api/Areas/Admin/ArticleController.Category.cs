using dna.core.service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.Areas.Admin
{
    public partial class ArticleController
    {
        /// <summary>
        /// Get category detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Category/{id}")]
        public async Task<IActionResult> GetCategoryDetail(int id)
        {
            var response = await _categoryService.GetSingleAsync(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get all category
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Category/{page}/{pageSize}")]
        public async Task<IActionResult >GetAllCategory(int page, int pageSize)
        {
            var response = await _categoryService.GetAllAsync(page, pageSize);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="modelToCreate"></param>
        /// <returns></returns>
        [HttpPost("Category")]
        public IActionResult PostCategory([FromBody] ArticleCategoryModel modelToCreate)
        {
           
            if ( String.IsNullOrWhiteSpace(modelToCreate.Slug)){
                modelToCreate.Slug = modelToCreate.Name.Replace(" ", "-").ToLower();
            }
            AssignModelState();
            var response = _categoryService.Create(modelToCreate);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Edit category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelToEdit"></param>
        /// <returns></returns>
        [HttpPut("Category/{id}")]
        public IActionResult EditCategory(int id, [FromBody] ArticleCategoryModel modelToEdit)
        {
            AssignModelState();
            modelToEdit.Id = id;
            var response = _categoryService.Edit(modelToEdit);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Upload Category Image
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut("Category/Image/{id}")]
        public async Task<IActionResult> UpdateCategoryImageAsync(int id, IFormFile file)
        {

            AssignModelState();
            var response = await _categoryService.UploadCoverImageAsync(id, file);
            
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
            
        }


    }
}
