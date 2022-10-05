using Newtonsoft.Json;
using PropertyChanged;
using SfcApplication.Models.Entities;
using SfcApplication.Models.Enums;
using SfcApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Common
{
    public class DownloadItem : BaseViewModel
    {
        public int Id { get; set; }
        public DiskFileInfo DiskFileInfo { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public DateTimeOffset DownloadedTime { get; set; }
        public long DownloadedSize { get; set; }
        public DownloadStatus Status { get; set; }
    }
}
