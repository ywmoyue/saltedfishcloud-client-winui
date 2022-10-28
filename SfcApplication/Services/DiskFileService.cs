using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using SfcApplication.Clients;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Entities;
using SfcApplication.Models.Mappers;
using SfcApplication.Models.Requests;

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

        public async Task<List<DiskFileInfo>> GetFolderSubFileList(List<string> paths, string fileName, int userId = 0)
        {
            var path = paths.GetFileUrlExceptRoot() + fileName + '/';
            var list = await m_diskFileClient.GetFileList(path, userId, m_userHostedService.Token);
            var subList = new List<DiskFileInfo>();
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

        public async Task MoveFiles(List<DiskFileInfoMapper> files, DiskFileInfoMapper target, int targetUserId = 0, int sourceUserId = 0)
        {
            var request = new MoveFileRequest();
            var targetPath = $"/{target.Paths.GetFileUrlExceptRoot()}{target.Name}";
            request.TargetUid = targetUserId;
            request.SourceUid = sourceUserId;
            request.Files = new List<MoveFileRequestItem>();
            foreach (var diskFileInfo in files)
            {
                var filePath = $"/{diskFileInfo.Paths.GetFileUrlExceptRoot()}{diskFileInfo.Name}";
                var item = new MoveFileRequestItem()
                {
                    Source = filePath,
                    Target = $"{targetPath}/{diskFileInfo.Name}"
                };
                request.Files.Add(item);
            }

            await m_diskFileClient.MoveFiles(request, targetUserId, m_userHostedService.Token);
        }

        public async Task CreateFolder(string name, List<string> Paths, int userId = 0)
        {
            var path = Paths.GetFileUrlExceptRoot();
            await m_diskFileClient.CreateFolder(name, path, userId, m_userHostedService.Token);
        }

        public async Task RenameFile(string oldName, string newName, List<string> Paths, int userId = 0)
        {
            var path = Paths.GetFileUrlExceptRoot();
            var request = new RenameFileRequest()
            {
                OldName = oldName,
                NewName = newName
            };
            await m_diskFileClient.RenameFile(request, path, userId, m_userHostedService.Token);
        }

        public async Task DeleteFile(List<string> fileNames, List<string> Paths, int userId = 0)
        {
            var path = Paths.GetFileUrlExceptRoot();
            var request = new DeleteFileRequest()
            {
                FileNames = fileNames
            };
            await m_diskFileClient.DeleteFile(request, path, userId, m_userHostedService.Token);
        }
    }
}
