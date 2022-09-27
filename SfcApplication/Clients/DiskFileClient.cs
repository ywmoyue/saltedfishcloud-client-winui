using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Entities;

namespace SfcApplication.Clients
{
    internal class DiskFileClient
    {
        private string m_baseUrl;
        private string m_token;

        public DiskFileClient(ClientConfig clientConfig)
        {
            m_baseUrl = clientConfig.BaseUrl + "diskFile/";
        }

        public async Task<List<DiskFileInfo>> GetFileList()
        {
            var result = await (m_baseUrl + "0/fileList/byPath/")
                .GetAsync()
                .ReceiveJsonList();
            return result.Cast<DiskFileInfo>().ToList();
        }
    }
}
