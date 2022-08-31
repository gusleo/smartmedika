using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediCore.Service.Helper.Template
{
    public static class TemplateGenerator
    {
        public static string GenerateTemplate(TemplateOption opt)
        {
            string root = opt.TemplateFolderPath;
            switch ( opt.TemplateType )
            {
                case TemplateType.Activation:
                    root = root + "/email_activation/index.html";
                    break;
                default:
                    break;
            }
            string txt = File.ReadAllText(root);
            return txt;
        }
        public static string GenerateSMSTemplate(TemplateOption opt)
        {
            string root = opt.TemplateFolderPath;
            switch (opt.TemplateType)
            {
                case TemplateType.Activation:
                    root = root + "/sms_activation.txt";
                    break;
                default:
                    break;
            }
            string txt = File.ReadAllText(root);
            return txt;
        }
    }
}
