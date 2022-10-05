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
        public ObservableCollection<DownloadItem> DownloadItemList { get; set; }

        public ObservableCollection<DownloadItem> DownloadingItemList
        {
            get => new ObservableCollection<DownloadItem>(DownloadItemList.Where(x => x.Status != Models.Enums.DownloadStatus.Downloaded));
        }

        public DownloadPageViewModel()
        {
            DownloadItemList = new ObservableCollection<DownloadItem>();
        }
    }
}
