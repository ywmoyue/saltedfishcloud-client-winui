using Downloader;
using Microsoft.Extensions.Hosting;
using SfcApplication.Extensions;
using SfcApplication.Models.Common;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Mappers;
using SfcApplication.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NClone;

namespace SfcApplication.HostedServices
{
    public class DownloadHostedService : IHostedService
    {
        private readonly ClientConfig m_clientConfig;
        private readonly DownloadConfiguration m_downloadConfig;
        private readonly LocalFileIOService m_localFileIOService;
        private List<DownloadTask> m_downloadTasks;
        private readonly UserConfig m_userConfig;

        public List<DownloadItem> DownloadItems { get => m_downloadTasks.Select(x => x.DownloadItem).ToList(); }

        public event EventHandler<List<DownloadItem>> DownloadItemsChange;
        public event EventHandler<DownloadItem> DownloadItemChange;
        public event EventHandler<DownloadItem> DownloadItemFinish;
        public event EventHandler<DownloadItem> DownloadItemAdd;

        public DownloadHostedService(ClientConfig clientConfig, DownloadConfiguration downloadConfig, LocalFileIOService localFileIOService, UserConfig userConfig)
        {
            m_clientConfig = clientConfig;
            m_downloadConfig = downloadConfig;
            m_localFileIOService = localFileIOService;
            m_userConfig = userConfig;
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

        public async Task Download(DiskFileInfoMapper diskFileInfo, string downloadPath = "", int userId = 0, string token = null)
        {
            if (string.IsNullOrEmpty(downloadPath))
                downloadPath = m_downloadConfig.TempDirectory;
            var path = diskFileInfo.Paths.GetFileUrlExceptRoot();
            var url = ConstructDownloadUrl(path, diskFileInfo.Name, userId);
            if (downloadPath[downloadPath.Length - 1] != '/')
            {
                downloadPath = downloadPath + '/';
            }
            downloadPath += path;
            var config = Clone.ObjectGraph(m_downloadConfig);
            if (!string.IsNullOrEmpty(token))
            {
                config.RequestConfiguration.Headers.Add("Token", token);
            }
            var downloader = DownloadBuilder.New()
                .WithConfiguration(config)
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
            InitDownloaderEvent(downloader, downloadItem);
            var downloadTask = new DownloadTask()
            {
                Downloader = downloader,
                DownloadItem = downloadItem,
                DownloadPackage = downloader.Package
            };
            m_downloadTasks.Add(downloadTask);
            DownloadItemAdd?.Invoke(this, downloadItem);
            Task.Run(()=>downloader.StartAsync());
        }

        public void Pause(int downloadItemId)
        {
            var task = m_downloadTasks.FirstOrDefault(x => x.DownloadItem.Id == downloadItemId);
            task.DownloadItem.Status = Models.Enums.DownloadStatus.Paused;
            DownloadItemChange?.Invoke(this, task.DownloadItem);
            task?.Downloader.Pause();
        }

        public async Task Resume(int downloadItemId)
        {
            var task = m_downloadTasks.FirstOrDefault(x => x.DownloadItem.Id == downloadItemId);
            if (task.Downloader == null)
            {
                await ResumeWithStart(downloadItemId);
            }
            else
            {
                task?.Downloader.Resume();
                task.DownloadItem.Status = Models.Enums.DownloadStatus.Downloading;
                DownloadItemChange?.Invoke(this, task.DownloadItem);
            }
        }

        public void Remove(int downloadItemId)
        {
            var task = m_downloadTasks.FirstOrDefault(x => x.DownloadItem.Id == downloadItemId);
            if (task == null) return;
            var filePath = "";
            if (task.Downloader == null)
            {
                if (task.DownloadItem.Status == Models.Enums.DownloadStatus.Downloaded && m_userConfig.IsRemoveDownloadedTaskWithFile)
                    filePath = task.DownloadPackage.FileName.UrlToPath();
                else if (task.DownloadItem.Status != Models.Enums.DownloadStatus.Downloaded && m_userConfig.IsRemoveDownloadingTaskWithFile && task.DownloadPackage.Chunks != null)
                    filePath = (task.DownloadPackage.Chunks[0].Storage as FileStorage)?.FileName.UrlToPath();
            }
            else
            {
                if (task.DownloadItem.Status == Models.Enums.DownloadStatus.Downloaded && m_userConfig.IsRemoveDownloadedTaskWithFile)
                    filePath = task.Downloader.Package.FileName.UrlToPath();
                else if (task.DownloadItem.Status != Models.Enums.DownloadStatus.Downloaded &&
                         m_userConfig.IsRemoveDownloadingTaskWithFile && task.Downloader.Package.Chunks != null)
                    filePath = (task.Downloader.Package.Chunks[0].Storage as FileStorage)?.FileName?.UrlToPath();


                task.Downloader.Stop();
                task.Downloader.Dispose();
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                File.Delete(filePath);
            }
            m_downloadTasks.Remove(task);
        }

        public async Task PauseAllWithExit()
        {
            foreach (var downloadTask in m_downloadTasks)
            {
                if (downloadTask.Downloader == null || downloadTask.DownloadItem.Status == Models.Enums.DownloadStatus.Downloaded) continue;
                downloadTask.DownloadPackage = downloadTask.Downloader.Package;
                downloadTask.DownloadItem.Status = Models.Enums.DownloadStatus.Paused;
                downloadTask.Downloader.Stop();
            }
            await WriteTaskToFile();
        }

        public async Task ResumeWithStart(int downloadItemId)
        {
            var task = m_downloadTasks.FirstOrDefault(x => x.DownloadItem.Id == downloadItemId);
            task.Downloader = DownloadBuilder.Build(task.DownloadPackage);
            InitDownloaderEvent(task.Downloader, task.DownloadItem);
            await task.Downloader.StartAsync();
        }

        public string GetFilePath(int downloadItemId)
        {
            var task = m_downloadTasks.FirstOrDefault(x => x.DownloadItem.Id == downloadItemId);
            if (task.DownloadPackage != null)
            {
                if (task.DownloadItem.Status == Models.Enums.DownloadStatus.Downloaded)
                {
                    return task.DownloadPackage.FileName.UrlToPath();
                }

                var pkgStorage = task.DownloadPackage.Chunks[0].Storage as FileStorage;
                return pkgStorage.FileName.UrlToPath();
            }

            if (task.DownloadItem.Status == Models.Enums.DownloadStatus.Downloaded)
            {
                return task.Downloader.Filename.UrlToPath();
            }
            var storage = task.Downloader.Package.Chunks[0].Storage as FileStorage;
            return storage.FileName.UrlToPath();
        }

        private void InitDownloaderEvent(IDownload downloader, DownloadItem downloadItem)
        {
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
                var package = e.UserState as DownloadPackage;
                if (package.Status == DownloadStatus.Completed)
                {
                    downloadItem.Status = Models.Enums.DownloadStatus.Downloaded;
                    DownloadItemFinish?.Invoke(this, downloadItem);
                }
                else
                {
                    if (e.Cancelled) return;
                    downloadItem.Status = Models.Enums.DownloadStatus.Failed;
                    DownloadItemChange?.Invoke(this, downloadItem);
                }
            };
        }

        private string ConstructDownloadUrl(string path, string fileName, int userId = 0)
        {
            var url = m_clientConfig.BaseUrl + m_clientConfig.OpenApi.DownloadFile
                .ReplaceParameter("userId", userId + "")
                .ReplaceParameter("path", path)
                .ReplaceParameter("fileName", fileName);
            return url;
        }

        private int CreateNewDownloadId()
        {
            var id = m_downloadTasks.LastOrDefault()?.DownloadItem.Id + 1;
            if (id == null)
            {
                id = 1;
            }
            return id.Value;
        }

        private async Task WriteTaskToFile()
        {
            try
            {
                await m_localFileIOService.SetDownloadTaskList(m_downloadTasks);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
