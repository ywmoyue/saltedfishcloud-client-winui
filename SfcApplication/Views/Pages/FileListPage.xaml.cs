using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using AutoMapper;
using SfcApplication.Services;
using SfcApplication.Models.Configs;
using SfcApplication.Models.Mappers;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Common;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.Extensions.DependencyInjection;
using SfcApplication.ViewModels;
using SfcApplication.Views.Components;

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
        private readonly IServiceProvider m_serviceProvider;

        public FileListPage(RouteService mRouteService, IMapper mMapper, ClientConfig config, DownloadHostedService downloadHostedService, UserHostedService userHostedService, ToastService toastService, DiskFileService diskFileService, IServiceProvider serviceProvider)
        {
            m_routeService = mRouteService;
            m_mapper = mMapper;
            m_config = config;
            this.InitializeComponent();
            m_downloadHostedService = downloadHostedService;
            m_userHostedService = userHostedService;
            m_toastService = toastService;
            m_diskFileService = diskFileService;
            m_serviceProvider = serviceProvider;
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
            var mappers = m_mapper.Map<List<DiskFileInfoMapperViewModel>>(diskFileInfos);
            mappers.ForEach(x =>
            {
                x.Paths = ViewModel.Paths.ToList();
                x.BaseUrl = m_config.BaseUrl + m_config.OpenApi.GetThumbnailImage;
                x.UserId = ViewModel.UserId;
            });
            ViewModel.DiskFileInfos = new ObservableCollection<DiskFileInfoMapperViewModel>(mappers);
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

            var fileInfo = originalSource.DataContext as DiskFileInfoMapperViewModel;
            if (fileInfo.Dir)
            {
                ViewModel.Paths.Add(fileInfo.Name);
                await this.InitData();

            }
        }

        private async void PathBreadcrumb_OnItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            for (var i = ViewModel.Paths.Count - 1; i >= args.Index + 1; i--)
            {
                ViewModel.Paths.RemoveAt(i);
            }
            await InitData();
        }

        private void FileInfoGridView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedDiskFileInfos == null || !ViewModel.SelectedDiskFileInfos.Any())
            {
                var originalSource = e.OriginalSource as FrameworkElement;
                var fileInfo = originalSource.DataContext as DiskFileInfoMapperViewModel;
                if (fileInfo != null)
                {
                    FileInfoGridView.SelectedItems.Add(fileInfo);
                }
            }

            FileListRightClickMenu.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedFileViewModelList = ViewModel.SelectedDiskFileInfos;
            var selectedFileList = m_mapper.Map<List<DiskFileInfoMapper>>(selectedFileViewModelList);
            foreach (var fileInfo in selectedFileList)
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
            var fileInfo = originalSource.DataContext as DiskFileInfoMapperViewModel;
            if (fileInfo == null)
            {
                FileInfoGridView.SelectedItems.Clear();
            }
        }

        private async void RefreshBtn_OnClick(object sender, RoutedEventArgs e)
        {
            await this.InitData();
        }

        private void CopyBtn_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void CutBtn_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void PasteBtn_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void FileInfoGridView_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ViewModel.IsSelectionBorderShow = true;
            var point = e.GetCurrentPoint(sender as FrameworkElement);
            ViewModel.SelectionBorderMouseSourcePoint = point.Position;
            ViewModel.SelectionBorderMouseTargetPoint = point.Position;
        }

        private void FileInfoGridView_OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!ViewModel.IsSelectionBorderShow) return;
            var point = e.GetCurrentPoint(sender as FrameworkElement);
            ViewModel.SelectionBorderMouseTargetPoint = point.Position;
            foreach (var fileBox in FileInfoGridView.ItemsPanelRoot.Children)
            {
                var offset = fileBox.ActualOffset;
                var size = fileBox.ActualSize;
                var sizeA = size.ToPoint();
                var sizeB = new Point(ViewModel.SelectionBorderWidth, ViewModel.SelectionBorderHeight);
                var pointA = new Point(offset.X, offset.Y);
                var pointB = new Point(ViewModel.SelectionBorderPosX, ViewModel.SelectionBorderPosY);
                var isIntersect = IsIntersectionRectangles(pointA, pointB, sizeA, sizeB);

                if (isIntersect)
                {
                    var fileElement = fileBox as GridViewItem;
                    var fileInfo = fileElement.Content as DiskFileInfoMapperViewModel;
                    var index = FileInfoGridView.SelectedItems.IndexOf(fileInfo);
                    if (index < 0)
                    {
                        FileInfoGridView.SelectedItems.Add(fileInfo);
                    }
                }
                else
                {
                    var fileElement = fileBox as GridViewItem;
                    var fileInfo = fileElement.Content as DiskFileInfoMapperViewModel;
                    FileInfoGridView.SelectedItems.Remove(fileInfo);
                }
            }
        }

        private void FileInfoGridView_OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ViewModel.IsSelectionBorderShow = false;
            ViewModel.SelectionBorderMouseSourcePoint = new Point(0, 0);
            ViewModel.SelectionBorderMouseTargetPoint = new Point(0, 0);
        }

        public bool IsIntersectionRectangles(Point a, Point b, Point sizeA, Point sizeB)
        {
            return Math.Max(a.X, b.X) < Math.Min(a.X + sizeA.X, b.X + sizeB.X)
                   && Math.Max(a.Y, b.Y) < Math.Min(a.Y + sizeA.Y, b.Y + sizeB.Y);
        }

        private async void FileItemPanel_DragEnter(object sender, DragEventArgs e)
        {
            var element = sender as FrameworkElement;
            var fileInfo = element.DataContext as DiskFileInfoMapperViewModel;
            if (!fileInfo.Dir) return;
            if (ViewModel.DraggingDiskFileInfos != null &&
                ViewModel.DraggingDiskFileInfos.Exists(x => x.Md5 == fileInfo.Md5)) return;
            if (ViewModel.DraggingDiskFileInfos != null)
                e.AcceptedOperation = DataPackageOperation.Move;
            else
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
            }

            if ((ViewModel.DraggingDiskFileInfos == null || !ViewModel.DraggingDiskFileInfos.Any()) &&
                !await HasDragLocalFile(e.DataView)) return;
            try
            {
                e.DragUIOverride.IsCaptionVisible = false;
                e.DragUIOverride.IsContentVisible = true;
                e.DragUIOverride.IsGlyphVisible = false;
                e.Handled = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void FileItemPanel_DragLeave(object sender, DragEventArgs e)
        {
            e.Handled = false;
        }

        private async void FileItemPanel_OnDrop(object sender, DragEventArgs e)
        {
            var element = sender as FrameworkElement;
            var fileInfoViewModel = element.DataContext as DiskFileInfoMapperViewModel;
            var fileInfo = m_mapper.Map<DiskFileInfoMapper>(fileInfoViewModel);
            if (!fileInfo.Dir) return;
            if (ViewModel.DraggingDiskFileInfos != null &&
                ViewModel.DraggingDiskFileInfos.Exists(x => x.Md5 == fileInfo.Md5)) return;
            if (ViewModel.DraggingDiskFileInfos != null&&ViewModel.DraggingDiskFileInfos.Any())
            {
                var fileViewModelList = ViewModel.DraggingDiskFileInfos;
                var fileList = m_mapper.Map<List<DiskFileInfoMapper>>(fileViewModelList);
                await m_diskFileService.MoveFiles(fileList, fileInfo, ViewModel.UserId,
                    ViewModel.UserId);
                ViewModel.DiskFileInfos.RemoveRange(fileViewModelList);
            }
        }

        private void FileInfoGridView_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            ViewModel.DraggingDiskFileInfos = e.Items.Cast<DiskFileInfoMapperViewModel>().ToList();
        }

        private void FileInfoGridView_OnDragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            ViewModel.DraggingDiskFileInfos = null;
        }

        private void FileInfoGridView_OnDrop(object sender, DragEventArgs e)
        {
        }

        private async void FileInfoGridView_OnDragEnter(object sender, DragEventArgs e)
        {
            if (!await HasDragLocalFile(e.DataView)) return;

            try
            {
                e.AcceptedOperation = DataPackageOperation.Move;
                e.DragUIOverride.IsCaptionVisible = false;
                e.DragUIOverride.IsContentVisible = true;
                e.DragUIOverride.IsGlyphVisible = false;
                e.Handled = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void FileInfoGridView_OnDragLeave(object sender, DragEventArgs e)
        {
            e.Handled = false;
        }

        private async Task<bool> HasDragLocalFile(DataPackageView packageView)
        {
            try
            {
                var storageList = await packageView.GetStorageItemsAsync();
                return storageList.Any();
            }
            catch
            {
                return false;
            }
        }

        private async Task<List<FileInfo>> GetDragLocalFileList(DataPackageView packageView)
        {
            var storageList = await packageView.GetStorageItemsAsync();
            var fileInfos = new List<FileInfo>();
            foreach (var storageItem in storageList)
            {
                var fileInfo = new FileInfo(storageItem.Path);
                fileInfos.Add(fileInfo);
            }

            return fileInfos;
        }

        private async void CreateFolderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = m_serviceProvider.GetRequiredService<CreateFolderDialog>();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Paths = ViewModel.Paths.ToList();
            dialog.UserId = ViewModel.UserId;
            await dialog.ShowAsync();
            if (dialog.IsOk)
            {
                await this.InitData();
            }
        }

        private async void ReNameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var fileInfo= ViewModel.SelectedDiskFileInfos.FirstOrDefault();
            if (fileInfo == null) return;
            var dialog = m_serviceProvider.GetRequiredService<RenameFileDialog>();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Paths = ViewModel.Paths.ToList();
            dialog.UserId = ViewModel.UserId;
            dialog.OldName = fileInfo.Name;
            await dialog.ShowAsync();
            if (dialog.IsOk)
            {
                var file = ViewModel.DiskFileInfos.FirstOrDefault(x => x.Md5 == fileInfo.Md5);
                file.Name = dialog.NewName;
            }
        }

        private async void DeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var fileList = ViewModel.SelectedDiskFileInfos.ToList();
            if(!fileList.Any()) return;
            var fileNameList = fileList.Select(x => x.Name).ToList();
            var confirm = await m_toastService.Confirm("即将删除以下文件", string.Join(',', fileNameList));
            if (!confirm) return;
            await m_diskFileService.DeleteFile(fileNameList, ViewModel.Paths.ToList(), ViewModel.UserId);
            ViewModel.DiskFileInfos.RemoveRange(fileList);
        }
    }
}
