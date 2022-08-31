using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dna.core.service.Services.Abstract;
using dna.core.libs.Validation;
using dna.core.data.Infrastructure;
using dna.core.libs.Cache;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Controllers
{
   
    /// <summary>
    /// Controller for article
    /// </summary>
    [Route("[controller]")]

    public partial class ArticleController : Controller
    {
        readonly IArticleService _articleService;
        readonly IArticleCategoryService _categoryService;
        readonly IArticleFavoriteService _favoriteArticleService;

        const int Duration = 24 * 60;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="articleService"></param>
        /// <param name="categoryService"></param>
        /// <param name="favoriteArticleService"></param>
        public ArticleController(IArticleService articleService, IArticleCategoryService categoryService, IArticleFavoriteService favoriteArticleService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _favoriteArticleService = favoriteArticleService;
        }
        private void AssignModelState()
        {
            _articleService.Initialize(new ModelStateWrapper(ModelState));
            _favoriteArticleService.Initialize(new ModelStateWrapper(ModelState));
        }
        /// <summary>
        /// Get newest article
        /// </summary>
        /// <returns></returns>
        [HttpGet("{pageIndex}/{pageSize}/{categoryId}")]
        public async Task<IActionResult> GetNewestArticle(int pageIndex, int pageSize, int categoryId)
        {
            var status = new List<ArticleStatus>() { ArticleStatus.Confirmed };
            var result = await _articleService.GetNewestArticleAsync(status, pageIndex, pageSize, categoryId);
            if ( result.Success )
            {
                return Ok(result.Item);
            }else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Get article by medical staff
        /// </summary>
        /// <param name="id">Staff Id</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Width</param>
        /// <returns></returns>
        [HttpGet("Staff/{id}/{pageIndex}/{pageSize}")]
        [Cache(Duration = Duration)]
        public async Task<IActionResult> GetArticleByStaff(int id, int pageIndex, int pageSize)
        {
            var status = new List<ArticleStatus>() { ArticleStatus.Confirmed };
            var result = await _articleService.GetArticleByStaff(id, pageIndex, pageSize, status.ToArray());
            if ( result.Success )
            {
                return Ok(result.Item);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Get article detail
        /// </summary>
        /// <param name="id">Article Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _articleService.GetSingleDetailAsync(id);
            if ( response.Success )
            {
                return Ok(response.Item);
            }else
            {
                return BadRequest(response.Message);
            }
        }

        /// <summary>
        /// Get Category Article with Article Count
        /// </summary>
        /// <returns>The article category.</returns>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>

        [HttpGet("Category/{pageIndex}/{pageSize}")]
        [Cache(Duration = Duration)]
        public async Task<IActionResult> GetArticleCategory(int pageIndex, int pageSize){
            var response = await _categoryService.GetCategoryWithCountAsync(pageIndex, pageSize, true);
            if (response.Success)
            {
                return Ok(response.Item);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

       
        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _articleService.Dispose();

        }
    }
}
