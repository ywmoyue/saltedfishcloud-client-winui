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
    internal class UserClient
    {
        private string m_token;
        private ClientConfig m_clientConfig;

        public UserClient(ClientConfig clientConfig)
        {
            m_clientConfig = clientConfig;
        }

        public async Task<string> GetToken(string user, string password)
        {
            try
            {
                var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.GetUserToken.
                    ReplaceParameter("user", user).
                    ReplaceParameter("passwd", password);
                var result = await url
                    .PostUrlEncodedAsync(new
                    {
                        user=user,
                        passwd=password
                    })
                    .ReceiveJson<BaseBean<string>>();
                return result.Data;
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        public async Task<User> GetUserInfo()
        {
            var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.GetUserInfo;
            var result = await url
                .WithHeader("Token", m_token)
                .GetAsync()
                .ReceiveJson<BaseBean<User>>();
            return result.Data;
        } 
    }
}
