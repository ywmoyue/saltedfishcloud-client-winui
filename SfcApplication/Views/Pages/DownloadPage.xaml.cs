using System.Collections.Generic;
using System.Linq;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Common;
using AutoMapper;
using SfcApplication.Models.Configs;
using SfcApplication.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DownloadPage : RoutePage
    {
        private readonly DownloadHostedService m_downloadHostedService;
        private readonly ClientConfig m_clientConfig;
        private readonly IMapper m_mapper;

        public DownloadPage(DownloadHostedService downloadHostedService, IMapper mapper, ClientConfig clientConfig)
        {
            m_downloadHostedService = downloadHostedService;
            m_mapper = mapper;
            m_clientConfig = clientConfig;
            this.InitializeComponent();
            var list = mapper.Map<List<DownloadItemViewModel>>(m_downloadHostedService.DownloadItems);
            ViewModel.DownloadItemList.AddRange(list);
            ViewModel.DispatcherQueue = DispatcherQueue;
            m_downloadHostedService.DownloadItemChange += DownloadHostedService_DownloadItemChange;
            m_downloadHostedService.DownloadItemAdd += DownloadHostedService_DownloadItemAdd;
            m_downloadHostedService.DownloadItemFinish += DownloadHostedService_DownloadItemFinish;
            DownloadedView.OpenFolderToFile += OpenFolderToFile;
            DownloadedView.DeleteDownloadItem += DeleteDownloadItem;
            DownloadingView.OpenFolderToFile += OpenFolderToFile;
            DownloadingView.DownloadPlay += DownloadingView_DownloadPlay;
            DownloadingView.DownloadPause += DownloadingView_DownloadPause;
            DownloadingView.DeleteDownloadItem += DeleteDownloadItem;
        }

        private void DeleteDownloadItem(object sender, DownloadItemViewModel e)
        {
            m_downloadHostedService.Remove(e.Id);
            ViewModel.DownloadItemList.Remove(e);
        }

        private void DownloadingView_DownloadPause(object sender, DownloadItemViewModel e)
        {
            m_downloadHostedService.Pause(e.Id);
        }

        private async void DownloadingView_DownloadPlay(object sender, DownloadItemViewModel e)
        {
            await m_downloadHostedService.Resume(e.Id);
        }

        private void OpenFolderToFile(object sender, DownloadItemViewModel e)
        {
            var path = m_downloadHostedService.GetFilePath(e.Id);
            System.Diagnostics.Process.Start("Explorer.exe", "/select," + $"{path}");
        }

        private void DownloadHostedService_DownloadItemFinish(object sender, DownloadItem e)
        {
            DispatcherQueue?.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, () =>
            {
                var item = ViewModel.DownloadItemList.FirstOrDefault(x => x.Id == e.Id);
                if (item == null) return;
                item.Status = e.Status;
                ViewModel.UpdateDownloadItemList();
            });
        }

        private void DownloadHostedService_DownloadItemAdd(object sender, DownloadItem e)
        {
            DispatcherQueue?.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
            {
                var downloadItem = m_mapper.Map<DownloadItemViewModel>(e);
                ViewModel.DownloadItemList.Add(downloadItem);
            });
        }

        private void DownloadHostedService_DownloadItemChange(object sender, DownloadItem e)
        {
            DispatcherQueue?.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, () =>
            {
                var item = ViewModel.DownloadItemList.FirstOrDefault(x => x.Id == e.Id);
                if (item == null) return;
                item.DownloadedSize = e.DownloadedSize;
                item.DownloadSpeed = e.DownloadSpeed;
                item.EstimatedRemainingTime = e.EstimatedRemainingTime;
                item.ProgressPercentage = e.ProgressPercentage;
                item.Status = e.Status;
            });
        }
    }
}
