using AutoMapper;
using SfcApplication.Models.Common;
using SfcApplication.Models.Entities;
using SfcApplication.Models.Enums;
using SfcApplication.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.ViewModels
{
    [AutoMap(typeof(DownloadItem), ReverseMap = true)]
    public class DownloadItemViewModel:BaseViewModel
    {
        public int Id { get; set; }
        public DiskFileInfoMapper DiskFileInfo { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public DateTimeOffset DownloadedTime { get; set; }
        public long DownloadedSize { get; set; }
        public DownloadStatus Status { get; set; }
    }
}
