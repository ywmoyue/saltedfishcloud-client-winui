using Downloader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Common
{
    internal class DownloadTask
    {
        public DownloadItem DownloadItem { get; set; }
        [JsonIgnore]
        public IDownload Downloader { get; set; }
        public DownloadPackage DownloadPackage { get; set; }
    }
}
