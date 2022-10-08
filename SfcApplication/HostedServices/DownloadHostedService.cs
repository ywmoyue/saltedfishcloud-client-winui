using Downloader;
using Microsoft.Extensions.Hosting;
using SfcApplication.Extensions;
using SfcApplication.Models.Common;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Entities;
using SfcApplication.Models.Mappers;
using SfcApplication.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace SfcApplication.HostedServices
{
    public class DownloadHostedService : IHostedService
    {
        private readonly ClientConfig m_clientConfig;
        private readonly DownloadConfiguration m_downloadConfig;
        private readonly LocalFileIOService m_localFileIOService;
        private List<DownloadTask> m_downloadTasks;

        public List<DownloadItem> DownloadItems { get => m_downloadTasks.Select(x => x.DownloadItem).ToList(); }

        public event EventHandler<List<DownloadItem>> DownloadItemsChange;
        public event EventHandler<DownloadItem> DownloadItemChange;
        public event EventHandler<DownloadItem> DownloadItemAdd;

        public DownloadHostedService(ClientConfig clientConfig, DownloadConfiguration downloadConfig, LocalFileIOService localFileIOService)
        {
            m_clientConfig = clientConfig;
            m_downloadConfig = downloadConfig;
            m_localFileIOService = localFileIOService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            m_downloadTasks = await m_localFileIOService.GetDownloadTaskList();
            if (m_downloadTasks == null)
            {
                m_downloadTasks = new List<DownloadTask>();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Download(DiskFileInfoMapper diskFileInfo,string downloadPath="")
        {
            if (string.IsNullOrEmpty(downloadPath))
                downloadPath = m_clientConfig.DefaultDownloadPath;
            var path = diskFileInfo.Paths.GetFilePathExceptRoot();
            var url = ConstructDownloadUrl(path, diskFileInfo.Name);
            downloadPath += path;
            var downloader = DownloadBuilder.New()
                .WithConfiguration(m_downloadConfig)
                .WithUrl(url)
                .WithDirectory(downloadPath)
                .WithFileName(diskFileInfo.Name)
                .Build();
            var downloadItem = new DownloadItem
            {
                Id = CreateNewDownloadId(),
                DiskFileInfo = diskFileInfo,
                CreationTime = DateTimeOffset.Now,
                DownloadedSize = 0,
                Status = Models.Enums.DownloadStatus.NotStarted
            };
            downloader.DownloadStarted += (sender, e) =>
            {
                downloadItem.Status = Models.Enums.DownloadStatus.Downloading;
                DownloadItemChange?.Invoke(this, downloadItem);
            };
            downloader.DownloadProgressChanged += (sender, e) =>
            {
                downloadItem.DownloadedSize = e.ReceivedBytesSize;
                DownloadItemChange?.Invoke(this, downloadItem);
            };
            downloader.DownloadFileCompleted += (sender, e) =>
            {
                downloadItem.Status = Models.Enums.DownloadStatus.Downloaded;
                DownloadItemChange?.Invoke(this, downloadItem);
            };
            var downloadTask = new DownloadTask()
            {
                Downloader = downloader,
                DownloadItem = downloadItem,
                DownloadPackage = downloader.Package
            };
            m_downloadTasks.Add(downloadTask);
            DownloadItemAdd?.Invoke(this, downloadItem);
            await WriteTaskToFile();
            await downloader.StartAsync();
        }

        public async Task Pause(int downloadItemId)
        {
            var task = m_downloadTasks.FirstOrDefault(x => x.DownloadItem.Id == downloadItemId);
            task.DownloadPackage = task.Downloader.Package;
            task.Downloader.Stop();
            await WriteTaskToFile();
        }

        public async Task Resume(int downloadItemId)
        {
            var task = m_downloadTasks.FirstOrDefault(x => x.DownloadItem.Id == downloadItemId);
            task.Downloader = DownloadBuilder.Build(task.DownloadPackage, m_downloadConfig);
            task.Downloader.DownloadStarted += (sender, e) =>
            {
                task.DownloadItem.Status = Models.Enums.DownloadStatus.Downloading;
                DownloadItemChange?.Invoke(this, task.DownloadItem);
            };
            task.Downloader.DownloadProgressChanged += (sender, e) =>
            {
                task.DownloadItem.DownloadedSize = e.ReceivedBytesSize;
                DownloadItemChange?.Invoke(this, task.DownloadItem);
            };
            task.Downloader.DownloadFileCompleted += (sender, e) =>
            {
                task.DownloadItem.Status = Models.Enums.DownloadStatus.Downloaded;
                DownloadItemChange?.Invoke(this, task.DownloadItem);
            };
            await task.Downloader.StartAsync();
        }

        private string ConstructDownloadUrl(string path,string fileName,int userId=0)
        {
            var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.DownloadFile
                .ReplaceParameter("userId", userId + "")
                .ReplaceParameter("path", path)
                .ReplaceParameter("fileName", fileName);
            return url;
        }

        private int CreateNewDownloadId()
        {
            var id = m_downloadTasks.LastOrDefault()?.DownloadItem.Id+1;
            if (id == null)
            {
                id = 1;
            }
            return id.Value;
        }

        private async Task WriteTaskToFile()
        {
            await m_localFileIOService.SetDownloadTaskList(m_downloadTasks);
        }
    }
}
