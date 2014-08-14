using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;

namespace Glimpse.NLog.Resource
{
    public class HtmlResource : FileResource
    {
        private const string InternalName = "glimpse_nlog_html";

        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            return new EmbeddedResourceInfo(this.GetType().Assembly, "Glimpse.NLog.Resource.glimpse.nlog.html", @"text/html");
        }

        public override IEnumerable<ResourceParameterMetadata> Parameters {
            get { return Enumerable.Empty<ResourceParameterMetadata>(); }
        }

        public HtmlResource()
        {
            //ResourceName = "Glimpse.NLog.Resource.glimpse.nlog.html";
            //ContentType = @"text/html";
            Name = InternalName;
        }

        public string GetResourceName() {
            return InternalName;
        }
    }
}