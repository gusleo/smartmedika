using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dna.core.auth;
using dna.core.auth.Model;
using dna.core.libs.Sender;
using dna.core.libs.Validation;
using dna.core.service.Infrastructure;
using dna.core.service.Services;
using dna.core.service.Services.Abstract;
using MediCore.Api.InputParam;
using MediCore.Data.Infrastructure;
using MediCore.Service.Helper.Template;
using MediCore.Service.Model;
using MediCore.Service.Services;
using MediCore.Service.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;



namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Account and User Controller
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    public class AccountController : Controller
    {
        readonly IAuthenticationService _authServices;
        readonly IUserDetailService _userDetailService;
        readonly ISenderFactory _sender;
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IPatientService _patientService;
        readonly IUserService _userService;
        readonly IFileServices _fileServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MediCore.Api.Controllers.AccountController"/> class.
        /// </summary>
        /// <param name="userService">User Service</param>
        /// <param name="authServices">Auth services.</param>
        /// <param name="userDetailService">User detail service.</param>
        /// <param name="patientService">File service.</param>
        /// <param name="fileServices">Patient service.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="hostingEnvironment">Hosting environment.</param>
        public AccountController(IUserService userService, IAuthenticationService authServices, IUserDetailService userDetailService, IPatientService patientService, IFileServices fileServices, ISenderFactory sender, IHostingEnvironment hostingEnvironment){
            _userService = userService;
            _authServices = authServices;
            _userDetailService = userDetailService;
            _patientService = patientService;
            _sender = sender;
            _fileServices = fileServices;
            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// Changes the avatar.
        /// </summary>
        /// <returns>The avatar.</returns>
        /// <param name="file">File.</param>
        [HttpPost("Avatar")]
        public async Task<IActionResult> ChangeAvatar([FromBody] FileBase64 file){
            
            var bytes = Convert.FromBase64String(file.Base64File);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                var formFile = new FormFile(ms, 0, ms.Length, "file", file.FileName)
                {
                    Headers = new HeaderDictionary
                {
                    { "Content-Disposition", string.Format("attachment; filename={0})", file.FileName) },
                    { "Content-Type", "multipart/form-data" }
                }
                };

                var userId = _authServices.GetUserId() ?? 0;
                if (userId == 0)
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
                        var res = await _fileServices.UploadAsync(formFile);
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
                ms.Dispose();
            }

            return BadRequest("Error on Server");


        }

        /// <summary>
        /// Registers the account.
        /// </summary>
        /// <returns>The account.</returns>
        /// <param name="model">Model.</param>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterAccountModel model)
        {
            // check the phone is already registered or not
            var isPhoneNumberExist = _userService.IsUserPhoneNumberExist(model.PhoneNumber);
            if ( isPhoneNumberExist )
            {
                return Conflict("Phone number already exist");
            }

            var result = await _authServices.Register(new RegisterModel
               {
                    Username = model.Email,
                    Email = model.Email,
                    Password = model.Password,
                    CellPhone = model.PhoneNumber,
                    ConfirmPassword = model.Password
               }, true, true);


            if (result.Result.Succeeded)
            {
                AssignModelState();
                var response = _userDetailService.Create(new UserDetailMediCoreModel
                {
                    Id = 0,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserId = result.UserId
                });

                if (!String.IsNullOrWhiteSpace(result.GeneratedToken))
                {
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = result.UserId, code = result.GeneratedToken }, protocol: HttpContext.Request.Scheme);
                    var sendgrid = _sender.Create(SenderFactory.EMAIL);
                    string template = TemplateGenerator.GenerateTemplate(new TemplateOption
                    {
                        TemplateType = TemplateType.Activation,
                        TemplateFolderPath = String.Format("{0}/{1}/{2}", _hostingEnvironment.ContentRootPath, "Resources", "Template")
                    });

                    template = template.Replace("{CONFIRMATION_LINK}", callbackUrl);
                    template = template.Replace("{FIRST_NAME}", model.FirstName);

                    //send email confirmation to user
                    await sendgrid.SendAsync(model.Email, "Aktivasi", template);
                }

                if(!String.IsNullOrWhiteSpace(result.GenerateSMSToken)){
                    var smsSender = _sender.Create(SenderFactory.SMS);
                    string template = TemplateGenerator.GenerateSMSTemplate(new TemplateOption
                    {
                        TemplateType = TemplateType.Activation,
                        TemplateFolderPath = String.Format("{0}/{1}/{2}", _hostingEnvironment.ContentRootPath, "Resources", "Template")
                    });
                    template = template.Replace("{TOKEN}", result.GenerateSMSToken);
                    await smsSender.SendAsync(model.PhoneNumber, "Aktivasi", template);
                }

                if(response.Success){
                    return Ok(response.Item);
                }
                return BadRequest(response.Message);
                
            }

            return BadRequest(result.Result.Errors.FirstOrDefault());
        }

        /// <summary>
        /// Get Login user detail
        /// </summary>
        /// <returns></returns>
        [HttpGet("Detail")]
        public async Task<IActionResult> GetUserLogin()
        {
            var userId = _authServices.GetUserId() ?? 0;
            if(userId > 0 )
            {
                var response = await _userDetailService.GetSingleWithPatientAsync(userId);
                if (response.Success)
                    return Ok(response.Item);
                if (response.Message == MessageConstant.NotFound)
                    return NotFound(response.Message);
                return BadRequest(response.Message);


            }
            return Forbid(MessageConstant.UserNotAllowed);

        }

        /// <summary>
        /// Create User Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Detail")]
        public IActionResult CreateDetail([FromBody] UserDetailMediCoreModel model)
        {
            AssignModelState();
            var response = _userDetailService.Create(model);
            if ( response.Success )
            {
                // by default user is patient
                var patientResponse = _patientService.Create(new PatientModel
                {
                    PatientName = String.Format("{0} {1}", model.FirstName, model.LastName),
                    DateOfBirth = model.Patient.DateOfBirth,
                    PatientStatus = PatientStatus.Active,
                    Gender = model.Patient.Gender,
                    RelationshipStatus = RelationshipStatus.YourSelf,
                    AssociatedUserId = response.Item.UserId
                });
                if ( patientResponse.Success )
                {
                    // Update User Detail
                    var detail = response.Item;
                    detail.PatientId = patientResponse.Item.Id;
                    var userResponse = _userDetailService.Edit(response.Item);
                    if ( userResponse.Success )
                        return Ok(userResponse.Item);
                }
                
            }
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Update User Detail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("Detail/{id}")]
        public async Task<IActionResult> UpdateDetail(int id, [FromBody] UpdateProfileParam param)
        {
            var model = new UserDetailMediCoreModel
            {
                Id = id,
                UserId = _authServices.GetUserId() ?? 0,
                PatientId = param.PatientId.HasValue ? param.PatientId : null,
                FirstName = param.FirstName,
                LastName = param.LastName,
                Address = param.Address,
                RegencyId = param.RegencyId

            };

            model.Patient = new PatientModel
            {
                Id = param.PatientId ?? 0 ,
                PatientName = String.Format("{0} {1}", param.FirstName, param.LastName),
                DateOfBirth = param.DateOfBirth,
                PatientStatus = PatientStatus.Active,
                Gender = param.Gender,
                RelationshipStatus = RelationshipStatus.YourSelf,
                AssociatedUserId = model.UserId
            };
            var response = await _userDetailService.EditWithPatientAsync(model);
            if ( response.Success )
                return Ok(response.Item);
            
            return BadRequest(response.Message);
        }

        void AssignModelState(){
            _userDetailService.Initialize(new ModelStateWrapper(ModelState));
            _patientService.Initialize(new ModelStateWrapper(ModelState));
        }


        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _userDetailService.Dispose();
            _patientService.Dispose();

        }
    }
}
