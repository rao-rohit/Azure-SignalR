using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Shared
{
    public class AppSettings
    {
        public string ServiceBusConnectionString { get; set; }
        public string ServiceBusQueueNameForBuyerRequests { get; set; }
        public string ServiceBusQueueNameForSupplierResponse { get; set; }
        public string SQLConnectionString { get; set; }
    }
}
