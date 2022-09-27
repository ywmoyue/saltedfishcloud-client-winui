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
using SfcApplication.Models.Common;
using SfcApplication.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class MainWindow : Window
    {
        private UserClient m_userClient;
        private DiskFileClient m_diskFileClient;
        private IServiceProvider m_serviceProvider;

        public MainWindow(UserClient userClient, DiskFileClient diskFileClient, HelloClient helloClient, IServiceProvider serviceProvider)
        {
            m_userClient = userClient;
            m_diskFileClient = diskFileClient;
            m_serviceProvider = serviceProvider;
            this.InitializeComponent();
            var fileListPage = serviceProvider.GetRequiredService<FileListPage>();
            MainFrame.Content = fileListPage;
        }

        private void MainNavView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavMenuItem;
            if (item.PageType == null) return;
            var page = m_serviceProvider.GetRequiredService(item.PageType);
            MainFrame.Content = page;
        }
    }
}
