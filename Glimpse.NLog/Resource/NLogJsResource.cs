using Glimpse.Core.Extensibility;

namespace Glimpse.NLog.Resource
{
    public class NLogJsResource : MyFileResource, IDynamicClientScript
    {
        private const string InternalName = "glimpse_nlog";

        public NLogJsResource() {
            ResourceName = "Glimpse.NLog.Resource.glimpse.nlog.js";
            ResourceType = @"application/x-javascript";
            Name = InternalName;
        }

        public ScriptOrder Order {
            get { return ScriptOrder.ClientInterfaceScript; }
        }

        public string GetResourceName() {
            return InternalName;
        }
    }
}