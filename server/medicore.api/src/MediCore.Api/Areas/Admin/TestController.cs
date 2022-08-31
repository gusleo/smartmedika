using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dna.core.libs.Sender;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MediCore.Api.Areas.Admin
{
    public class SmsModel
    {
        public string ReceiverNumber { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
    }

    /// <summary>
    /// Controller of test
    /// </summary>
    [Route("admin/[controller]")]
    public class TestController : Controller
    {
        private readonly ISenderFactory _factory;

        public TestController(ISenderFactory factory)
        {
            _factory = factory;
        }
        /// <summary>
        /// Test SMS
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("SMS")]
        public async Task<IActionResult> SendSMS([FromBody] SmsModel param)
        {
            ISender sender = _factory.Create("sms");
            var result = await sender.SendAsync(param.ReceiverNumber, param.Subject, param.Content);
            if ( result.Successful )
                return Ok(result.Message);
            else
                return BadRequest(result.Message);
        }
    }
}
