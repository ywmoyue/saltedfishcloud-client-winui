using SfcApplication.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.ViewModels
{
    internal class DownloadingViewModel:BaseViewModel
    {
        private ObservableCollection<DownloadItem> m_downloadItemList;
        public ObservableCollection<DownloadItem> DownloadItemList
        {
            get => m_downloadItemList;
            set => Set(ref m_downloadItemList, value);
        }
    }
}
