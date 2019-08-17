using System;
using System.Collections.Generic;
using System.ServiceModel.Configuration;
using System.Configuration;
using System.Linq;

namespace DistributorPatternForWcf
{
    class Distributor
    {
        private static object _lockObject = new object();
        private List<string> enpoints;
        private IEnumerator<SomeServiceClient> _enumerator;
        public Distributor()
        {
            enpoints = new List<string>();

            if (!(ConfigurationManager.GetSection("system.serviceModel/client") is ClientSection clientSettings))
            {
                throw new Exception("missing client config!");
            }
            foreach (ChannelEndpointElement endpoint in clientSettings.Endpoints)
            {
                if (endpoint.Binding != "netMsmqBinding" || endpoint.Contract != "DistributorPatternForWcf.ISomeService") continue;
                enpoints.Add(endpoint.Name);
            }
            if (!enpoints.Any())
            {
                throw new Exception("No action to enumerate");
            }
            _enumerator = initiateEnumerator().GetEnumerator();

        }

        private IEnumerable<SomeServiceClient> initiateEnumerator()
        {
            while (true)
            {
                foreach (var configurationName in enpoints)
                {
                    var client = new SomeServiceClient(configurationName)
                    {
                        EndpointName = configurationName
                    };
                    yield return client;
                }
            }
        }

        public void ExecuteNext(Action<SomeServiceClient> clientAction)
        {
            lock (_lockObject)
            {
                if (_enumerator.MoveNext())//do not expected but you know...
                {
                    using (var client = _enumerator.Current)
                    {
                        clientAction(client);
                        client.Close();
                    }
                }
            }

        }
    }
}
