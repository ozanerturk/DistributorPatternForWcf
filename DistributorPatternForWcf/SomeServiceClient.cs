using System;
using System.ServiceModel;

namespace DistributorPatternForWcf
{
    public class SomeServiceClient : ClientBase<ISomeService>, IDisposable
    {
        public string EndpointName { get; internal set; }
        public SomeServiceClient(string configurationName)
           : base(configurationName)
        {
        }
        public void Dispose()
        {
        }
    }
}
