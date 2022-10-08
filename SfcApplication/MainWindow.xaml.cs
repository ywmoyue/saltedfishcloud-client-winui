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
using Microsoft.Extensions.DependencyInjection;
using SfcApplication.Clients;
using SfcApplication.HostedServices;
using SfcApplication.Models.Common;
using SfcApplication.Services;
using SfcApplication.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private UserClient m_userClient;
        private DiskFileClient m_diskFileClient;
        private IServiceProvider m_serviceProvider;
        private readonly RouteService m_routeService;
        private readonly DownloadHostedService m_downloadHostedService;

        public MainWindow(UserClient userClient, DiskFileClient diskFileClient, HelloClient helloClient ,RouteService routeService, IServiceProvider serviceProvider, DownloadHostedService downloadHostedService)
        {
            m_userClient = userClient;
            m_diskFileClient = diskFileClient;
            m_serviceProvider = serviceProvider;
            m_downloadHostedService = downloadHostedService;
            m_routeService = routeService;
            this.InitializeComponent();
            m_routeService.Init(MainFrame);
            Closed += MainWindow_Closed;
        }

        private async void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            await m_downloadHostedService.PauseAllWithExit();
        }

        private void MainNavView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavMenuItem;
            if (item.Path == null) return;
            m_routeService.Push(item.Path);
        }
    }
}
