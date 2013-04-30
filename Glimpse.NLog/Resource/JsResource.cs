using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.NLog.Resource
{
    public class JsResource : MyFileResource, IDynamicClientScript
    {
        private const string InternalName = "glimpse_nlog_js";

        public JsResource() {
            ResourceName = "Glimpse.NLog.Resource.glimpse.nlog.js";
            ResourceType = @"application/x-javascript";
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