using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SfcApplication.ViewModels;
using System;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Components
{
    public sealed partial class DownloadingView : UserControl
    {
        public event EventHandler<DownloadItemViewModel> OpenFolderToFile;
        public event EventHandler<DownloadItemViewModel> DownloadPlay;
        public event EventHandler<DownloadItemViewModel> DownloadPause;
        public event EventHandler<DownloadItemViewModel> DeleteDownloadItem;

        public DownloadingView()
        {
            this.InitializeComponent();
        }

        private async void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            var view = sender as FrameworkElement;
            var item = view.DataContext as DownloadItemViewModel;
            DownloadPlay?.Invoke(this,item);
        }

        private void PauseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var view = sender as FrameworkElement;
            var item = view.DataContext as DownloadItemViewModel;
            DownloadPause?.Invoke(this, item);
        }
        private void OpenFolderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var view = sender as FrameworkElement;
            var item = view.DataContext as DownloadItemViewModel;
            OpenFolderToFile?.Invoke(this, item);
        }

        private void DeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var view = sender as FrameworkElement;
            var item = view.DataContext as DownloadItemViewModel;
            DeleteDownloadItem?.Invoke(this, item);
        }
    }
}
