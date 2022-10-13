using System.Collections.Generic;
using System.Threading.Tasks;
using SfcApplication.Clients;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Entities;

namespace SfcApplication.Services
{
    public class DiskFileService
    {
        private readonly DiskFileClient m_diskFileClient;
        private readonly UserHostedService m_userHostedService;

        public DiskFileService(DiskFileClient diskFileClient, UserHostedService userHostedService)
        {
            m_diskFileClient = diskFileClient;
            m_userHostedService = userHostedService;
        }

        public async Task<List<DiskFileInfo>> GetFileList(string path = "", int userId = 0)
        {
            return await m_diskFileClient.GetFileList(path, userId, m_userHostedService.Token);
        }

        public async Task<List<DiskFileInfo>> GetFolderSubFileList(List<string> paths,string fileName, int userId = 0)
        {
            var path = paths.GetFileUrlExceptRoot() + fileName+'/';
            var list= await m_diskFileClient.GetFileList(path, userId, m_userHostedService.Token);
            var subList=new List<DiskFileInfo>();
            paths.Add(fileName);
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Dir)
                {
                    subList.AddRange(await GetFolderSubFileList(paths, list[i].Name, userId));
                    list.RemoveAt(i);
                    i--;
                }
                else
                {
                    list[i].Paths = paths;
                }
            }
            list.AddRange(subList);
            return list;
        }
    }
}
