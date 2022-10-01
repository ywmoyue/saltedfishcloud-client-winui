using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SfcApplication.Models.Entities;
using SfcApplication.Models.Mappers;

namespace SfcApplication.ViewModels
{
    internal class FileListPageViewModel : BaseViewModel
    {
        private ObservableCollection<DiskFileInfoMapper> m_diskFileInfos;
        private ObservableCollection<string> m_paths;

        public ObservableCollection<DiskFileInfoMapper> DiskFileInfos
        {
            get => m_diskFileInfos;
            set => Set(ref m_diskFileInfos, value);
        }

        public ObservableCollection<string> Paths
        {
            get => m_paths;
            set => Set(ref m_paths, value);
        }
    }
}
