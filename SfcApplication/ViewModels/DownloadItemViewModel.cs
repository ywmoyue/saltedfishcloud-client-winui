using AutoMapper;
using SfcApplication.Models.Common;
using SfcApplication.Models.Enums;
using SfcApplication.Models.Mappers;
using System;

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
        public long DownloadSpeed { get; set; }
        public TimeSpan EstimatedRemainingTime { get; set; }
        public int ProgressPercentage { get; set; }
        public DownloadStatus Status { get; set; }
    }
}
