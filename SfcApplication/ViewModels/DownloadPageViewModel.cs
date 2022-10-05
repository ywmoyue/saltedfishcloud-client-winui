using SfcApplication.Models.Common;
using SfcApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.ViewModels
{
    public class DownloadPageViewModel : BaseViewModel
    {

        private ObservableCollection<DownloadItem> m_downloadItemList;
        public ObservableCollection<DownloadItem> DownloadItemList
        {
            get => m_downloadItemList;
            set {
                Set(ref m_downloadItemList, value);
                Set("DownloadingItemList");
            }
        }

        public ObservableCollection<DownloadItem> DownloadingItemList
        {
            get => new ObservableCollection<DownloadItem>(DownloadItemList.Where(x => x.Status != Models.Enums.DownloadStatus.Downloaded));
        }

        public DownloadPageViewModel()
        {
            DownloadItemList = new ObservableCollection<DownloadItem>();
            DownloadingItemList.CollectionChanged += DownloadingItemList_CollectionChanged;
        }

        public void DownloadingItemList_CollectionChanged(object sender=null, System.Collections.Specialized.NotifyCollectionChangedEventArgs e=null)
        {
            Set("DownloadingItemList");
        }
    }
}
