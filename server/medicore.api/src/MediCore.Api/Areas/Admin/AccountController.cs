using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dna.core.auth;
using dna.core.auth.Model;
using IdentityModel.Client;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using MediCore.Service.Services;
using Microsoft.AspNetCore.Authorization;
using dna.core.auth.Infrastructure;
using dna.core.service.Infrastructure;
using MediCore.Service.Model;
using Microsoft.AspNetCore.Http;
using dna.core.libs.Validation;
using dna.core.service.Services;
using System;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    /// <summary>
    /// Controller of account
    /// </summary>
    [Authorize(Roles = MembershipConstant.MultipleRolesAdmin)]
    [Route("admin/[controller]")]
    public class AccountController : Controller
    {

        readonly IAuthenticationService _auth;
        readonly IUserDetailService _userDetailService;
        readonly IFileServices _fileServices;

        /// <summary>
        /// Constructor of <see cref="AccountController"/>
        /// </summary>
        /// <param name="auth">Interface of <see cref="AuthenticationService"/></param>
        /// <param name="userDetailService">Interface of <see cref="IUserDetailService"/></param>
        /// <param name="fileServices">Instance of <see cref="IFileServices"/></param>
        public AccountController(IAuthenticationService auth, IUserDetailService userDetailService, IFileServices fileServices)
        {
            _auth = auth;
            _userDetailService = userDetailService;
            _fileServices = fileServices;
        }

        /// <summary>
        /// Edits the user detail.
        /// </summary>
        /// <returns>The user detail.</returns>
        /// <param name="id">User detail identifier</param>
        /// <param name="param">Parameter <see cref="UserDetailMediCoreModel"/></param>
        [HttpPut("Detail/{id}")]
        public IActionResult EditUserDetail(int id, [FromBody]UserDetailMediCoreModel param){
            AssignModelState();
            param.Id = id;
            param.UserId = _auth.GetUserId() ?? 0;
            var response = _userDetailService.Edit(param);
            if (response.Success)
                return Ok(response.Item);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Register user method
        /// </summary>
        /// <param name="register">Register user <see cref="RegisterModel"/></param>
        /// <returns>ObjectResult</returns>
        [HttpPost("Register")]
        public async Task<ObjectResult> Register([FromBody]RegisterModel register)
        {
            var result = await _auth.Register(register, false, false);
            return Ok(result.Result.Succeeded);
        }

        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="model">Login user <see cref="LoginModel"/></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ObjectResult> Login([FromBody] LoginModel model)
        {
            var result = await _auth.Login(model);
            return Ok(result.Succeeded);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="clue"></param>
        /// <returns></returns>

        
        [HttpGet("{pageIndex}/{pageSize}")]
        [HttpGet("{pageIndex}/{pageSize}/{clue}")]
        public async Task<ObjectResult> GetAll(int pageIndex, int pageSize, string clue = "")
        {
            var response = await _userDetailService.GetAllWithDetailAsync(pageIndex, pageSize, clue);
            if ( response.Success )
                return Ok(response.Item);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Get Login user detail
        /// </summary>
        /// <returns></returns>
        [HttpGet("Detail")]
        public async Task<IActionResult> GetUserLogin()
        {
            var userId = _auth.GetUserId() ?? 0;
            if (userId > 0)
            {
                var response = await _userDetailService.GetSingleAsync(userId);
                if (response.Success)
                    return Ok(response.Item);
                if (response.Message == MessageConstant.NotFound)
                    return NotFound(response.Message);
                return BadRequest(response.Message);


            }
            return Forbid(MessageConstant.UserNotAllowed);

        }

        /// <summary>
        /// Change Avatar
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("Avatar")]
        public async Task<IActionResult> ChangeAvatar(IFormFile file)
        {
                var userId = _auth.GetUserId() ?? 0;
                if ( userId == 0 )
                    return BadRequest(MessageConstant.UserNotAllowed);

                var userResponse = await _userDetailService.GetDetailByUserId(userId);
                if ( userResponse.Success )
                {
                    AssignModelState();
                    var user = userResponse.Item;
                    try
                    {

                        if ( !String.IsNullOrWhiteSpace(user.Avatar) )
                        {
                            await _fileServices.DeleteAsync(user.Avatar);
                        }
                        var res = await _fileServices.UploadAsync(file);
                        user.Avatar = res.Item.Path;
                        var response = _userDetailService.Edit(user);
                        if ( response.Success )
                            return Ok(response.Item);

                        return BadRequest(response.Message);
                    }
                    catch ( Exception ex )
                    {
                        return BadRequest(ex.Message);
                    }
            }

            return BadRequest(userResponse.Message);
            

        }

        void AssignModelState()
        {
            _userDetailService.Initialize(new ModelStateWrapper(ModelState));
           
        }


    }
}
