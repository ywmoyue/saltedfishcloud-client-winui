using SfcApplication.Models.Enums;
using SfcApplication.Models.Mappers;
using System;

namespace SfcApplication.Models.Common
{
    public class DownloadItem
    {
        private long m_downloadedSize;
        public int Id { get; set; }
        public DiskFileInfoMapper DiskFileInfo { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public DateTimeOffset DownloadedTime { get; set; }
        public long DownloadedLastSecondSize { get; set; }
        public long DownloadedSize
        {
            get =>m_downloadedSize; 
            set {
                if (DownloadedSizeSaveTime == DateTimeOffset.MinValue || DownloadedSizeSaveTime <= DateTimeOffset.Now - TimeSpan.FromSeconds(1))
                {
                    DownloadedLastSecondSize = DownloadedSize;
                    m_downloadedSize = value;
                    DownloadedSizeSaveTime = DateTimeOffset.Now;
                }
            }
        }
        public long DownloadSpeed
        {
            get => DownloadedSize - DownloadedLastSecondSize;
        }
        public DateTimeOffset DownloadedSizeSaveTime { get; set; }=DateTimeOffset.MinValue;
        public TimeSpan EstimatedRemainingTime
        {
            get
            {
                if (DownloadSpeed == 0) return TimeSpan.MaxValue;
                var timeMs = (DiskFileInfo.Size-DownloadedSize) / DownloadSpeed;
                return TimeSpan.FromSeconds(timeMs);
            }
        }

        public int ProgressPercentage
        {
            get {
                return (int)((DownloadedSize*1f) / (DiskFileInfo.Size*1f)*100);
            }
        }
        public DownloadStatus Status { get; set; }
    }
}
