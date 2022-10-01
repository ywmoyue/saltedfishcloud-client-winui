using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using SfcApplication.Models.Beans;
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

        public async Task<List<DiskFileInfo>> GetFileList(string path = "")
        {
            try
            {

                var result = await (m_baseUrl + "0/fileList/byPath/" + path)
                    .GetAsync()
                    .ReceiveJson<BaseBean<List<List<DiskFileInfo>>>>();
                result.Data[0].AddRange(result.Data[1]);
                return result.Data[0];
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
