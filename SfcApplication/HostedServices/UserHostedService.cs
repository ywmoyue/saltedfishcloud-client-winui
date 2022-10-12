using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SfcApplication.Clients;
using SfcApplication.Models.Entities;
using SfcApplication.Services;

namespace SfcApplication.HostedServices
{
    public class UserHostedService:IHostedService
    {
        private readonly LocalFileIOService m_localFileIoService;
        private readonly UserClient m_userClient;
        public event EventHandler UserLogined;

        public string Token { get; set; }
        public User User { get; set; }

        public UserHostedService(LocalFileIOService localFileIoService, UserClient userClient)
        {
            m_localFileIoService = localFileIoService;
            m_userClient = userClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Token = await GetLocalToken();
            if (string.IsNullOrEmpty(Token)) return;
            User = await GetUserInfo();
            if (User != null)
            {
                UserLogined?.Invoke(this, EventArgs.Empty);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<(bool, string)> Login(string username, string passwd)
        {
            var (data, success) = await m_userClient.GetToken(username, passwd);
            if (!success)
            {
                return (false, data);
            }

            Token = data;
            await m_localFileIoService.SetUserToken(data);
            await GetUserInfo();
            UserLogined?.Invoke(this, EventArgs.Empty);
            return (true, null);
        }

        public async Task<User> GetUserInfo()
        {
            var user = await m_userClient.GetUserInfo(Token);
            return user;
        }

        public async Task<string> GetLocalToken()
        {
            var token = await m_localFileIoService.GetUserToken();
            return token;
        }

        public async Task<QuotaUsed> GetQuotaUsed()
        {
            var quotaUsed = await m_userClient.GetQuatoUsed(Token);
            return quotaUsed;
        }
    }
}
