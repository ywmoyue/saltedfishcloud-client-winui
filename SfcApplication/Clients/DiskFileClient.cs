using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using SfcApplication.Extensions;
using SfcApplication.Models.Beans;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Entities;
using SfcApplication.Models.Requests;

namespace SfcApplication.Clients
{
    public class DiskFileClient
    {
        private ClientConfig m_clientConfig;

        public DiskFileClient(ClientConfig clientConfig)
        {
            m_clientConfig = clientConfig;
        }

        public async Task<List<DiskFileInfo>> GetFileList(string path = "", int userId = 0, string token = null)
        {
            try
            {
                var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.GetFileList
                    .ReplaceParameter("userId", userId + "").ReplaceParameter("path", path);
                var result = await url
                    .WithHeader("Token", token)
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

        public async Task MoveFiles(MoveFileRequest request, int userId = 0,string token=null)
        {
            try
            {
                var url = m_clientConfig.BaseUrl +
                          m_clientConfig.OpenApi.MoveFiles.ReplaceParameter("userId", userId + "");
                var result = await url
                    .WithHeader("Token", token)
                    .PostJsonAsync(request)
                    .ReceiveJson<BaseBean>();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task CreateFolder(string name, string path = "", int userId = 0, string token = null)
        {
            try
            {
                var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.CreateFolder
                    .ReplaceParameter("userId", userId + "").ReplaceParameter("path", path);
                var result = await url
                    .WithHeader("Token", token)
                    .SendUrlEncodedAsync(HttpMethod.Put, new
                    {
                        name=name
                    })
                    .ReceiveJson<BaseBean>();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task RenameFile(RenameFileRequest request, string path = "", int userId = 0,
            string token = null)
        {
            try
            {
                var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.RenameFile
                    .ReplaceParameter("userId", userId + "").ReplaceParameter("path", path);
                var result = await url
                    .WithHeader("Token", token)
                    .SendUrlEncodedAsync(HttpMethod.Put, request)
                    .ReceiveJson<BaseBean>();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task DeleteFile(DeleteFileRequest request, string path = "", int userId = 0,
            string token = null)
        {
            try
            {
                var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.DeleteFile
                    .ReplaceParameter("userId", userId + "").ReplaceParameter("path", path);
                var result = await url
                    .WithHeader("Token", token)
                    .SendJsonAsync(HttpMethod.Delete, request)
                    .ReceiveJson<BaseBean>();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
