using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using SfcApplication.Extensions;
using SfcApplication.Models.Beans;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Entities;

namespace SfcApplication.Clients
{
    internal class DiskFileClient
    {
        private string m_token;
        private ClientConfig m_clientConfig;

        public DiskFileClient(ClientConfig clientConfig)
        {
            m_clientConfig = clientConfig;
        }

        public async Task<List<DiskFileInfo>> GetFileList(string path = "",int userId=0)
        {
            try
            {
                var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.GetFileList.
                    ReplaceParameter("userId", userId+"").
                    ReplaceParameter("path", path);
                var result= await url
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
