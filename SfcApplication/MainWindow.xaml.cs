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
        private UserHostedService m_userHostedService;
        private DiskFileClient m_diskFileClient;
        private IServiceProvider m_serviceProvider;
        private readonly RouteService m_routeService;
        private readonly DownloadHostedService m_downloadHostedService;
        private readonly ToastService m_toastService;

        public MainWindow(DiskFileClient diskFileClient, HelloClient helloClient ,RouteService routeService, IServiceProvider serviceProvider, DownloadHostedService downloadHostedService, ToastService toastService, UserHostedService userHostedService)
        {
            Title = "咸鱼云网盘";
            m_diskFileClient = diskFileClient;
            m_serviceProvider = serviceProvider;
            m_downloadHostedService = downloadHostedService;
            m_toastService = toastService;
            m_userHostedService = userHostedService;
            m_routeService = routeService;
            this.InitializeComponent();
            m_routeService.Init(MainFrame);
            m_toastService.Init(RootElement);
            Closed += MainWindow_Closed;
            m_routeService.Push("/fileList/public");
            m_userHostedService.UserLogined += UserHostedService_UserLogined;
        }

        private void UserHostedService_UserLogined(object sender, EventArgs e)
        {
            var navItems=ViewModel.NavFullMenu.Where(x => x.Name == "登陆" || x.Name == "注册");
            foreach (var navMenuItem in navItems)
            {
                navMenuItem.Hidden = true;
            }

            navItems = ViewModel.NavFullMenu.Where(x => x.Name == "用户中心");
            foreach (var navMenuItem in navItems)
            {
                navMenuItem.Hidden = false;
            }
            ViewModel.UpdateNavMenu();
        }

        private async void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            await m_downloadHostedService.PauseAllWithExit();
        }

        private void MainNavView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavMenuItem;
            if (item.Path == null) return;
            if (item.Path == "/fileList/private")
            {
                var query = new FileListNavigatedQuery()
                {
                    UserId = m_userHostedService.User.Id
                };
                m_routeService.Push(item.Path, query);
                return;
            }
            m_routeService.Push(item.Path);
        }
    }
}
