using Microsoft.UI.Dispatching;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace SfcApplication.ViewModels
{
    public class DownloadPageViewModel : BaseViewModel
    {
        public DispatcherQueue DispatcherQueue { get; set; }
        public ObservableCollection<DownloadItemViewModel> DownloadItemList { get; set; }
        
        [DependsOn(nameof(DownloadItemList))]
        public ObservableCollection<DownloadItemViewModel> DownloadingItemList => new ObservableCollection<DownloadItemViewModel>(DownloadItemList.Where(x => x.Status != Models.Enums.DownloadStatus.Downloaded));


        [DependsOn(nameof(DownloadItemList))]
        public ObservableCollection<DownloadItemViewModel> DownloadedItemList => new ObservableCollection<DownloadItemViewModel>(DownloadItemList.Where(x => x.Status == Models.Enums.DownloadStatus.Downloaded));

        public DownloadPageViewModel()
        {
            DownloadItemList = new ObservableCollection<DownloadItemViewModel>();
            DownloadItemList.CollectionChanged += DownloadItemList_CollectionChanged;
        }

        private void DownloadItemList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (DispatcherQueue == null) return;
            DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
            {
                Set(nameof(DownloadingItemList));
                Set(nameof(DownloadedItemList));
            });
        }

        public void UpdateDownloadItemList()
        {
            Set(nameof(DownloadingItemList));
            Set(nameof(DownloadedItemList));
        }
    }
}
