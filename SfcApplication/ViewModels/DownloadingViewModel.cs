using SfcApplication.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.ViewModels
{
    public class DownloadingViewModel:BaseViewModel
    {
        public ObservableCollection<DownloadItemViewModel> DownloadItemList { get; set; }

        public DownloadingViewModel()
        {
            DownloadItemList = new ObservableCollection<DownloadItemViewModel>();
        }
    }
}
