namespace MediCore.Service.Helper.Template
{
    public class TemplateOption
    {
        public string TemplateFolderPath{ get; set; }
        public TemplateType TemplateType { get; set; }
    }

    public enum TemplateType
    {
        Welcome = 1,
        Activation = 2
    }
}