using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using dna.core.service.Services.Abstract;
using dna.core.libs.Validation;
using dna.core.service.Models;
using dna.core.data.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using dna.core.data.Infrastructure;
using dna.core.service.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Article Controller
    /// </summary>
    [Route("admin/[controller]")]
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    public partial class ArticleController : Controller
    {
        private IArticleService _articleService;
        private IArticleCategoryService _categoryService;
        private IFileServices _fileServices;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="articleService"></param>
        /// <param name="categoryService"></param>
        /// <param name="fileServices"></param>
        public ArticleController(IArticleService articleService, IArticleCategoryService categoryService, IFileServices fileServices)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _fileServices = fileServices;
        }
        private void AssignModelState()
        {
            _articleService.Initialize(new ModelStateWrapper(ModelState));
            _categoryService.Initialize(new ModelStateWrapper(ModelState));
        }

        /// <summary>
        /// Get category detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleDetailAsync(int id)
        {
            var response = await _articleService.GetSingleAsync(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get all article
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("{page}/{pageSize}")]
        public async Task<IActionResult> GetAllArticleAsync(int page, int pageSize)
        {
            ArticleStatus[] status = new ArticleStatus[] { ArticleStatus.Confirmed, ArticleStatus.UnConfirmed };
            var response = await _articleService.FindByStatus(page, pageSize, status);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Create article
        /// </summary>
        /// <param name="modelToCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] ArticleModel modelToCreate)
        {
            AssignModelState();
            var response = _articleService.Create(modelToCreate);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Edit article
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelToEdit"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] ArticleModel modelToEdit)
        {
            AssignModelState();
            modelToEdit.Id = id;
            var response = _articleService.Edit(modelToEdit);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Change status of article
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("Status/{id}")]
        public async Task<IActionResult> ChangeArticleStatusAsync(int id, [FromBody] ArticleStatus status)
        {
            AssignModelState();
            var response = await _articleService.ChangeStatusAsync(id, status);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        // <summary>
        /// Upload image to article
        // </summary>
        /// <param name="id">ArticleId</param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut("CoverImage/{id}")]
        public async Task<IActionResult> UpdateArticleImage(int id, IFormFile file)
        {

            AssignModelState();
            var response = await _articleService.UploadArticleImageCoverAsync(id, file);
            if ( response.Success )
            {
                return Ok(response.Item);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        

    }
}
