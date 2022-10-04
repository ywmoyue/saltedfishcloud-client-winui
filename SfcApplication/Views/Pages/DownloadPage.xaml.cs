﻿using Microsoft.UI.Xaml;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class DownloadPage : RoutePage
    {
        private readonly DownloadHostedService m_downloadHostedService;

        public DownloadPage(DownloadHostedService downloadHostedService)
        {
            m_downloadHostedService = downloadHostedService;
            this.InitializeComponent();
            ViewModel.DownloadItemList = new ObservableCollection<DownloadItem>(m_downloadHostedService.DownloadItems);
        }
    }
}