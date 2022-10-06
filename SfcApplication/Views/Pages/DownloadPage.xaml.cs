using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Common;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using SfcApplication.Views.Components;
using SfcApplication.Services;
using AutoMapper;
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
        private DownloadHostedService m_downloadHostedService;
        private IMapper m_mapper;

        public DownloadPage(DownloadHostedService downloadHostedService, IMapper mapper)
        {
            m_downloadHostedService = downloadHostedService;
            m_mapper = mapper;
            this.InitializeComponent();
            var list = mapper.Map<List<DownloadItemViewModel>>(m_downloadHostedService.DownloadItems);
            ViewModel.DownloadItemList.AddRange(list);
            ViewModel.DispatcherQueue = DispatcherQueue;
            m_downloadHostedService.DownloadItemChange += DownloadHostedService_DownloadItemChange;
            m_downloadHostedService.DownloadItemAdd += DownloadHostedService_DownloadItemAdd;
        }

        private void DownloadHostedService_DownloadItemAdd(object sender, DownloadItem e)
        {
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
            {
                var downloadItem = m_mapper.Map<DownloadItemViewModel>(e);
                ViewModel.DownloadItemList.Add(downloadItem);
            });
        }

        private void DownloadHostedService_DownloadItemChange(object sender, DownloadItem e)
        {
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, () =>
            {
                var item = ViewModel.DownloadItemList.FirstOrDefault(x => x.Id == e.Id);
                if (item == null) return;
                item.DownloadedSize = e.DownloadedSize;
                var index = ViewModel.DownloadItemList.IndexOf(item);
            });
        }
    }
}
