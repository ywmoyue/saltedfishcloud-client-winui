using SfcApplication.Models.Common;
using SfcApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Converters
{
    internal class DownloadItemConverter
    {
        public static DownloadingViewModel GetDownloadingItem(IList<DownloadItem> source)
        {
            if(source == null)
            {
                return new DownloadingViewModel { DownloadItemList = new ObservableCollection<DownloadItem>() };
            }
            return new DownloadingViewModel()
            {
                DownloadItemList = new ObservableCollection<DownloadItem>(source.Where(x => x.Status != Models.Enums.DownloadStatus.Downloaded))
            };
        }
    }
}
