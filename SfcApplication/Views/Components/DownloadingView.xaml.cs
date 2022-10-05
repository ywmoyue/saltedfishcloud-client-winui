using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Common;
using SfcApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Components
{
    public sealed partial class DownloadingView : UserControl
    {
        private readonly DownloadHostedService m_downloadHostedService;
        public DownloadingView(DownloadHostedService downloadHostedService)
        {
            this.InitializeComponent();
            m_downloadHostedService = downloadHostedService;
            ViewModel.DownloadItemList.AddRange(m_downloadHostedService.DownloadingItems);

            m_downloadHostedService.DownloadingItemChange += DownloadHostedService_DownloadingItemChange;
        }

        private void DownloadHostedService_DownloadingItemChange(object sender, DownloadItem e)
        {
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
            {
                ViewModel.DownloadItemList.FirstOrDefault(x => x.Id == e.Id).DownloadedSize = e.DownloadedSize;
            });
        }
    }
}
