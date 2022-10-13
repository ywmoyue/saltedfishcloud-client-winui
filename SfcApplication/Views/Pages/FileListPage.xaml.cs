using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SfcApplication.Services;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Mappers;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Common;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileListPage : RoutePage
    {
        private readonly RouteService m_routeService;
        private readonly DiskFileService m_diskFileService;
        private readonly IMapper m_mapper;
        private readonly ClientConfig m_config;
        private readonly DownloadHostedService m_downloadHostedService;
        private readonly UserHostedService m_userHostedService;
        private readonly ToastService m_toastService;

        public FileListPage(RouteService mRouteService, IMapper mMapper, ClientConfig config, DownloadHostedService downloadHostedService, UserHostedService userHostedService, ToastService toastService, DiskFileService diskFileService)
        {
            m_routeService = mRouteService;
            m_mapper = mMapper;
            m_config = config;
            this.InitializeComponent();
            m_downloadHostedService = downloadHostedService;
            m_userHostedService = userHostedService;
            m_toastService = toastService;
            m_diskFileService = diskFileService;
        }

        public override async void OnNavigated(object query)
        {
            if (query == null)
            {
                InitPaths();
                ViewModel.UserId = 0;
            }
            else
            {
                var navigatedQuery = query as FileListNavigatedQuery;
                if (navigatedQuery.Paths == null)
                {
                    InitPaths();
                }
                else
                {
                    var paths = navigatedQuery.Paths;
                    ViewModel.Paths = new ObservableCollection<string>(paths);
                }
                
                ViewModel.UserId = navigatedQuery.UserId;
            }
            await InitData();
        }

        public async Task InitData()
        {
            var path = ViewModel.Paths.GetFileUrlExceptRoot();
            var diskFileInfos = await m_diskFileService.GetFileList(path, ViewModel.UserId);
            var mappers = m_mapper.Map<List<DiskFileInfoMapper>>(diskFileInfos);
            mappers.ForEach(x=>
            {
                x.Paths = ViewModel.Paths.ToList();
                x.BaseUrl = m_config.BaseUrl+m_config.OpenApi.GetThumbnailImage;
                x.UserId = ViewModel.UserId;
            });
            ViewModel.DiskFileInfos = new ObservableCollection<DiskFileInfoMapper>(mappers);
        }

        private void InitPaths()
        {
            if (ViewModel.Paths != null) ViewModel.Paths.Clear();
            else ViewModel.Paths = new ObservableCollection<string>();
            ViewModel.Paths.Add("根");
        }

        private async void FileItemPanel_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var originalSource = sender as FrameworkElement;
            
            var fileInfo = originalSource.DataContext as DiskFileInfoMapper;
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
            if (ViewModel.SelectedDiskFileInfos==null||!ViewModel.SelectedDiskFileInfos.Any())
            {
                var originalSource = e.OriginalSource as FrameworkElement;
                var fileInfo = originalSource.DataContext as DiskFileInfoMapper;
                if (fileInfo != null)
                {
                    FileInfoGridView.SelectedItems.Add(fileInfo);
                }
            }

            FileListRightClickMenu.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var fileInfo in ViewModel.SelectedDiskFileInfos)
            {
                if (!fileInfo.Dir)
                {
                    await m_downloadHostedService.Download(fileInfo, token: m_userHostedService.Token,
                        userId: ViewModel.UserId);
                    m_toastService.Show($"文件{fileInfo.Name}已加入下载");
                    return;
                }

                var fileList =
                    await m_diskFileService.GetFolderSubFileList(fileInfo.Paths, fileInfo.Name, ViewModel.UserId);
                var mappers = m_mapper.Map<List<DiskFileInfoMapper>>(fileList);
                foreach (var diskFileInfo in mappers)
                {
                    await m_downloadHostedService.Download(diskFileInfo, token: m_userHostedService.Token,
                        userId: ViewModel.UserId);
                }
                m_toastService.Show($"目录{fileInfo.Name}已加入下载");
            }
        }

        private void FileInfoGridView_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var originalSource = e.OriginalSource as FrameworkElement;
            var fileInfo = originalSource.DataContext as DiskFileInfoMapper;
            if (fileInfo == null)
            {
                FileInfoGridView.SelectedItems.Clear();
            }
        }

        private async void RefreshBtn_OnClick(object sender, RoutedEventArgs e)
        {
            await this.InitData();
        }
    }
}
