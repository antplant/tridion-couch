using System.Collections.Generic;

namespace TridionCouch.Templates.Model
{
    public class CouchPage
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string PageTemplate { get; set; }
        public List<CouchComponentPresentation> ComponentPresentations { get; set; }

        public CouchPage()
        {
            ComponentPresentations = new List<CouchComponentPresentation>();
        }
    }
}