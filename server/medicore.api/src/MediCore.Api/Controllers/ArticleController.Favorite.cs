using dna.core.libs.Cache;
using dna.core.service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.Controllers
{
    
    public partial class ArticleController
    {
        
        /// <summary>
        /// Get Favorite Article by User
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Favorite/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetFavoriteArticle(int pageIndex, int pageSize)
        {
           
            var response = await _favoriteArticleService.GetUserFavoriteArticleAsync(pageIndex, pageSize);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Bookmark article as favorite
        /// </summary>
        /// <param name="modelToCreate"></param>
        /// <returns></returns>
       
        [Authorize]
        [HttpPost("Favorite")]
        public IActionResult PostFavorite([FromBody] ArticleFavoriteModel modelToCreate)
        {
            AssignModelState();
            var response = _favoriteArticleService.Create(modelToCreate);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Delete favorite article
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Favorite/{id}")]
        public async Task<IActionResult> DeleteFavorite(int id)
        {
            var response = await _favoriteArticleService.Delete(id);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Removes the favorite.
        /// </summary>
        /// <returns>The favorite.</returns>
        /// <param name="id">Article id</param>
        [Authorize]
        [HttpDelete("Favorite/Remove/{id}")]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            var response = await _favoriteArticleService.DeleteByArticle(id);
            if (response.Success)
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }
    }
}
