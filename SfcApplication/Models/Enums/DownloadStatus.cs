using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Enums
{
    public enum DownloadStatus
    {
        NotStarted,//未开始
        Paused,//暂停中
        Downloading,
        Downloaded,
        Failed
    }
}
