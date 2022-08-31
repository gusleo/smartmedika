using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dna.core.libs.Sender;
using MediCore.Api.InputParam;
using Microsoft.AspNetCore.Hosting;
using MediCore.Service.Helper.Template;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// Sender Test
    /// </summary>
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ISenderFactory _sender;
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="hostingEnvironment"></param>
        public TestController(ISenderFactory sender, IHostingEnvironment hostingEnvironment)
        {
            _sender = sender;
            _hostingEnvironment = hostingEnvironment;
        }
        
       /// <summary>
       /// Sending message to device by token
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FirebaseModel model)
        {
            
            var fcm = _sender.Create(SenderFactory.FCM);
            var options = new FCMOption
            {
                Priority = "high",
                JsonData = model.JsonData,
                TargetScreen = model.TargetScreen
            };
            var response = await fcm.SendAsync(model.DeviceToken, model.Title, model.Message, options);
            if ( response.Successful )
                return Ok(response.Message);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Send Email Test
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        [HttpPost("Email")]
        public async Task<IActionResult> SendEmail(string destination)
        {
            var sendgrid = _sender.Create(SenderFactory.EMAIL);
            string message = TemplateGenerator.GenerateTemplate(new TemplateOption {
                TemplateType = TemplateType.Activation,
                TemplateFolderPath = String.Format("{0}\\{1}\\{2}", _hostingEnvironment.ContentRootPath, "Resources", "Template")
             });
            var response = await sendgrid.SendAsync(destination, "Activation", message);
            if ( response.Successful )
                return Ok(response.Message);
            else
                return BadRequest(response.Message);
        }

        
    }
}
