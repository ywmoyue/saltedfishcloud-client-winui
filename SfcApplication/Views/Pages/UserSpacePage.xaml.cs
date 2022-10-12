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
using Windows.Storage.Pickers;
using AutoMapper;
using Downloader;
using Microsoft.Extensions.Configuration;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Configs;
using SfcApplication.Services;
using SfcApplication.ViewModels;
using WinRT;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserSpacePage : RoutePage
    {
        private readonly UserHostedService m_userHostedService;
        private readonly IMapper m_mapper;
        private readonly DownloadConfiguration m_downloadConfiguration;
        private readonly UserConfig m_userConfig;
        private readonly ConfigService m_configService;
        private readonly MainWindow m_mainWindow;

        public UserSpacePage(UserHostedService userHostedService, IMapper mapper, ClientConfig clientConfig, DownloadConfiguration downloadConfiguration, UserConfig userConfig,  ConfigService configService, MainWindow mainWindow)
        {
            m_userHostedService = userHostedService;
            m_mapper = mapper;
            m_downloadConfiguration = downloadConfiguration;
            m_userConfig = userConfig;
            m_configService = configService;
            m_mainWindow = mainWindow;
            this.InitializeComponent();
            ViewModel.User = m_mapper.Map<UserItemViewModel>(m_userHostedService.User);
            ViewModel.AvatarUrl = clientConfig.BaseUrl + clientConfig.OpenApi.GetUserAvatarImage;
            ViewModel.DefaultDownloadPath = m_downloadConfiguration.TempDirectory;
            ViewModel.IsRemoveDownloadedTaskWithFile = m_userConfig.IsRemoveDownloadedTaskWithFile;
            ViewModel.IsRemoveDownloadingTaskWithFile = m_userConfig.IsRemoveDownloadingTaskWithFile;
        }

        public override async void OnNavigated(object query)
        {
            var quotaUsed = await m_userHostedService.GetQuotaUsed();
            ViewModel.QuotaUsed =quotaUsed;
        }

        private async void RemoveDownloadedTaskWithFileSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch=sender as ToggleSwitch;
            if (m_userConfig.IsRemoveDownloadedTaskWithFile == toggleSwitch.IsOn) return;
            m_userConfig.IsRemoveDownloadedTaskWithFile = toggleSwitch.IsOn;
            await m_configService.SetIsRemoveDownloadedTaskWithFile(m_userConfig.IsRemoveDownloadedTaskWithFile);
        }

        private async void RemoveDownloadingTaskWithFileSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;
            if (m_userConfig.IsRemoveDownloadingTaskWithFile == toggleSwitch.IsOn) return;
            m_userConfig.IsRemoveDownloadingTaskWithFile = toggleSwitch.IsOn;
            await m_configService.SetIsRemoveDownloadingTaskWithFile(m_userConfig.IsRemoveDownloadingTaskWithFile);
        }

        private async void DefaultDownloadPathBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var picker = new FolderPicker();
            picker.AttachWindow();

            var folder = await picker.PickSingleFolderAsync();
            if (folder == null) return;
            var path = folder.Path;
            m_downloadConfiguration.TempDirectory = path;
            await m_configService.SetDefaultDownloadPath(path);
            ViewModel.DefaultDownloadPath = path;
        }
    }
}
