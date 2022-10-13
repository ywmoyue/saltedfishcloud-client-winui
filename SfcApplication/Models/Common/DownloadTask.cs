using Downloader;
using Newtonsoft.Json;

namespace SfcApplication.Models.Common
{
    public class DownloadTask
    {
        public DownloadItem DownloadItem { get; set; }
        [JsonIgnore]
        public IDownload Downloader { get; set; }
        public DownloadPackage DownloadPackage { get; set; }
    }
}
