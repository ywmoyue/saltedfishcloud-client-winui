using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using AutoMapper;
using SfcApplication.Clients;
using SfcApplication.Models.Entities;
using SfcApplication.Services;
using Microsoft.UI.Xaml.Shapes;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Mappers;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileListPage : RoutePage
    {
        private DiskFileClient m_diskFileClient;
        private readonly RouteService m_routeService;
        private readonly IMapper m_mapper;
        private readonly ClientConfig m_config;
        private readonly DownloadHostedService m_downloadHostedService;

        public FileListPage(DiskFileClient diskFileClient, RouteService mRouteService, IMapper mMapper, ClientConfig config, DownloadHostedService downloadHostedService)
        {
            m_diskFileClient = diskFileClient;
            m_routeService = mRouteService;
            m_mapper = mMapper;
            m_config = config;
            this.InitializeComponent();
            m_downloadHostedService = downloadHostedService;
        }

        public override async void OnNavigated(object query)
        {
            if (query == null)
            {
                if(ViewModel.Paths!=null) ViewModel.Paths.Clear();
                else ViewModel.Paths = new ObservableCollection<string>();
                ViewModel.Paths.Add("根");
            }
            else
            {
                var paths = query as List<string>;
                {
                    ViewModel.Paths = new ObservableCollection<string>(paths);
                }
            }
            await InitData();
        }

        public async Task InitData()
        {
            var path = ViewModel.Paths.GetFilePathExceptRoot();
            var diskFileInfos = await m_diskFileClient.GetFileList(path);
            var mappers = m_mapper.Map<List<DiskFileInfoMapper>>(diskFileInfos);
            mappers.ForEach(x=>
            {
                x.Paths = ViewModel.Paths.ToList();
                x.BaseUrl = m_config.BaseUrl;
                x.UserId = 0;
            });
            ViewModel.DiskFileInfos = new ObservableCollection<DiskFileInfoMapper>(mappers);
        }

        private async void FileItemPanel_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var originalSource = sender as FrameworkElement;
            
            var fileInfo = originalSource.DataContext as DiskFileInfo;
            if (fileInfo.Dir)
            {
                ViewModel.Paths.Add(fileInfo.Name);
                await this.InitData();

            }
        }

        private async void PathBreadcrumb_OnItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            for (var i = ViewModel.Paths.Count - 1; i >= args.Index+1; i--)
            {
                ViewModel.Paths.RemoveAt(i);
            }
            await InitData();
        }

        private void FileInfoGridView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FileListRightClickMenu.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileInfo = ViewModel.SelectedDiskFileInfos.FirstOrDefault();
            await m_downloadHostedService.Download(fileInfo);
        }
    }
}
