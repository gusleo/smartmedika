using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Infrastructure
{

    public class EmailTemplate
    {
        public const string WELCOME_EMAIL = "welcome.html";
        public const string APPOINTMENT_REMINDER = "appointment.html";
        public const string TREATMENT_REMINDER = "treatment.html";
        public const string NEWS = "news.html";
        private readonly IHostingEnvironment _env;
        public EmailTemplate(IHostingEnvironment hostingEnvirontment)
        {
            _env = hostingEnvirontment;
        }
        public string GetByType(string emailType)
        {
            string root = String.Format("{0}{1}{2}", _env.ContentRootPath, "\\Resources\\MailTemplate\\", emailType);            
            string txt = File.ReadAllText(root);
            return txt;
        }
    }
}
