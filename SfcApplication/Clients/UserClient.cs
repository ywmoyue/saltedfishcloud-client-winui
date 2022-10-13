using System;
using System.Threading.Tasks;
using Flurl.Http;
using SfcApplication.Extensions;
using SfcApplication.Models.Beans;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Entities;

namespace SfcApplication.Clients
{
    public class UserClient
    {
        private ClientConfig m_clientConfig;

        public UserClient(ClientConfig clientConfig)
        {
            m_clientConfig = clientConfig;
        }

        public async Task<(string,bool)> GetToken(string user, string password)
        {
            try
            {
                var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.GetUserToken.ReplaceParameter("user", user)
                    .ReplaceParameter("passwd", password);
                var result = await url
                    .PostUrlEncodedAsync(new
                    {
                        user = user,
                        passwd = password
                    })
                    .ReceiveJson<BaseBean<string>>();
                var success = result.Code == 200;
                return (result.Data, success);
            }
            catch (FlurlHttpException ex)
            {
                var result=await ex.Call.Response.GetJsonAsync<BaseBean<string>>();
                return (result.Msg, false);
            }
            catch (Exception ex)
            {
                // ignored
            }

            return default;
        }

        public async Task<User> GetUserInfo(string token)
        {
            var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.GetUserInfo;
            var result = await url
                .WithHeader("Token", token)
                .GetAsync()
                .ReceiveJson<BaseBean<User>>();
            return result.Data;
        }

        public async Task<QuotaUsed> GetQuatoUsed(string token)
        {
            var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.GetQuotaUsed;

            var result = await url
                .WithHeader("Token", token)
                .GetAsync()
                .ReceiveJson<BaseBean<QuotaUsed>>();
            return result.Data;
        }
    }
}
