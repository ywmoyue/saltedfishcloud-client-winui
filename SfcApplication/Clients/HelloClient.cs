using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using SfcApplication.Models.Configs;

namespace SfcApplication.Clients
{
    internal class HelloClient
    {
        private string m_baseUrl;

        public HelloClient(ClientConfig clientConfig)
        {
            m_baseUrl = clientConfig.BaseUrl + "hello/";
        }

        public async Task<string> GetFeature()
        {
            return await (m_baseUrl + "feature")
                .GetAsync()
                .ReceiveString();
        }
    }
}
