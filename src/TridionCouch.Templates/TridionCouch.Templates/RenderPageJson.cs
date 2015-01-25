using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.Templating;
using Tridion.ContentManager.Templating.Assembly;
using TridionCouch.Templates.Extensions;
using TridionCouch.Templates.Model;

namespace TridionCouch.Templates
{
    [TcmTemplateTitle("Render Page as JSON")]
    public class RenderPageJson : ITemplate
    {
        public void Transform(Engine engine, Package package)
        {
            var pageItem = package.GetByType(ContentType.Page);
            var page = engine.GetObject(pageItem.GetAsSource().GetValue("ID")) as Page;
            if (page == null) throw new Exception("No page found in package.");

            var couchPage = new CouchPage
            {
                Id = page.Id.ToString(),
                Url = page.PublishLocationUrl,
                PageTemplate = page.PageTemplate.Title
            };

            foreach (var cp in page.ComponentPresentations)
            {
                var cpString = engine.RenderComponentPresentation(cp.Component.Id, cp.ComponentTemplate.Id);
                var jsonString = XElement.Parse(cpString).Value;

                TemplatingLogger.GetLogger(typeof (RenderPageJson)).Info("CP: " + cpString);
                couchPage.ComponentPresentations.Add(JsonConvert.DeserializeObject<CouchComponentPresentation>(jsonString));
            }
            package.PushItem("CouchPage", package.CreateStringItem(ContentType.Page, JsonConvert.SerializeObject(couchPage)));
        }
    }

    [TcmTemplateTitle("Render Component Presentation as Json")]
    public class RenderPresentationJson : ITemplate
    {
        public void Transform(Engine engine, Package package)
        {
            var template = engine.PublishingContext.ResolvedItem.Template as ComponentTemplate;
            var component =
                engine.GetObject(package.GetByType(ContentType.Component).GetAsSource().GetValue("ID")) as Component;

            var presentation = new CouchComponentPresentation
            {
                ComponentId = component.Id.ToString(),
                TemplateId = template.Id.ToString(),
                Template = template.Title.Clean(),
                Priority = -template.Priority
            };

            foreach (var thing in component.Content.ChildNodes.Cast<XmlNode>())
            {
                ((IDictionary<string, object>)presentation.Fields)[thing.Name] = thing.InnerText;
            }

            package.PushItem("Output",
                package.CreateStringItem(ContentType.Text, JsonConvert.SerializeObject(presentation)));
        }
    }
}
