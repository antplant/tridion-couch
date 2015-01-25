using System.Dynamic;

namespace TridionCouch.Templates.Model
{
    public class CouchComponentPresentation
    {
        public string ComponentId { get; set; }
        public string TemplateId { get; set; }
        public string Template { get; set; }
        public int Priority { get; set; }
        public ExpandoObject Fields { get; set; }

        public CouchComponentPresentation()
        {
            Fields = new ExpandoObject();
        }
    }
}