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
    internal class UserClient
    {
        private string m_baseUrl;
        private string m_token;

        public UserClient(ClientConfig clientConfig)
        {
            m_baseUrl = clientConfig.BaseUrl + "user/";
        }

        public async Task<string> GetToken(string user, string password)
        {
            try
            {
                var result = await (m_baseUrl + $"token?user={user}&passwd={password}")
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
            var result = await (m_baseUrl)
                .WithHeader("Token", m_token)
                .GetAsync()
                .ReceiveJson<BaseBean<User>>();
            return result.Data;
        } 
    }
}
