using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;

namespace Glimpse.NLog.Resource
{
    public class HtmlResource : MyFileResource
    {
        private const string InternalName = "glimpse_nlog_html";

        public override IEnumerable<ResourceParameterMetadata> Parameters {
            get { return Enumerable.Empty<ResourceParameterMetadata>(); }
        }

        public HtmlResource() {
            ResourceName = "Glimpse.NLog.Resource.glimpse.nlog.html";
            ResourceType = @"text/html";
            Name = InternalName;
        }

        public string GetResourceName() {
            return InternalName;
        }
    }
}