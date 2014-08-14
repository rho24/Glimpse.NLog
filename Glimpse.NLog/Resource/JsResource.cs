using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;

namespace Glimpse.NLog.Resource
{
    public class JsResource : FileResource, IDynamicClientScript
    {
        private const string InternalName = "glimpse_nlog_js";


        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            return new EmbeddedResourceInfo(this.GetType().Assembly, "Glimpse.NLog.Resource.glimpse.nlog.html", @"text/html");
        }

        public JsResource() {
//            ResourceName = "Glimpse.NLog.Resource.glimpse.nlog.js";
            //ResourceType = @"application/x-javascript";
            Name = InternalName;
        }

        public ScriptOrder Order {
            get { return ScriptOrder.IncludeAfterClientInterfaceScript; }
        }

        public string GetResourceName() {
            return InternalName;
        }
    }
}