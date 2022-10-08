using Microsoft.UI.Xaml;
using SfcApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Converters
{
    public class DownloadStatusConverter
    {
        public static Visibility GetDownloadingViewVisibility(DownloadStatus status)
        {
            if (status == DownloadStatus.Downloading)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }


        public static Visibility GetNotDownloadingViewVisibility(DownloadStatus status)
        {
            if (status == DownloadStatus.Downloading)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public static string GetDownloadStatusText(DownloadStatus status)
        {
            switch (status)
            {
                case DownloadStatus.Downloading:
                    return "下载中";
                case DownloadStatus.Downloaded:
                    return "已完成";
                case DownloadStatus.NotStarted:
                    return "未开始";
                case DownloadStatus.Failed:
                    return "下载失败";
                case DownloadStatus.Paused:
                    return "已暂停";
            }
            return "";
        }
    }
}
