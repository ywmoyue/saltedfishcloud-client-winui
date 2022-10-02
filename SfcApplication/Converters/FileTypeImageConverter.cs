using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SfcApplication.Models.Entities;

namespace SfcApplication.Converters
{
    internal class FileTypeImageConverter
    {
        public static ImageSource Convert(string suffix, string name,bool dir,List<string> paths,string md5,string baseUrl,int userId)
        {
            var appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var iconDir = new DirectoryInfo($"{appPath}/Assets/icon");
            if (dir)
            {
                return GetBitmapImage($"{iconDir.FullName}/dir.png");
            }

            switch (suffix.ToLower())
            {
                case "apk":
                    return GetBitmapImage($"{iconDir.FullName}/android.png");
                case "mp3":
                case "wav":
                    return GetBitmapImage($"{iconDir.FullName}/audio.png");
                case "cs":
                case "js":
                case "java":
                    return GetBitmapImage($"{iconDir.FullName}/code.png");
                case "doc":
                case "docx":
                    return GetBitmapImage($"{iconDir.FullName}/doc.png");
                case "xls":
                case "xlsx":
                    return GetBitmapImage($"{iconDir.FullName}/excel.png");
                case "exe":
                    return GetBitmapImage($"{iconDir.FullName}/exe.png");
                case "txt":
                case "md":
                    return GetBitmapImage($"{iconDir.FullName}/txt.png");
                case "iso":
                    return GetBitmapImage($"{iconDir.FullName}/iso.png");
                case "jpg":
                case "png":
                case "gif":
                    var img = GetBitmapImage($"{baseUrl}resource/{userId}/thumbnail/{md5}?type={suffix}");
                    return img;
                case "ppt":
                case "pptx":
                    return GetBitmapImage($"{iconDir.FullName}/ppt.png");
                case "mp4":
                    return GetBitmapImage($"{iconDir.FullName}/video.png");
                case "rar":
                case "zip":
                case "gz":
                case "7z":
                    return GetBitmapImage($"{iconDir.FullName}/zipped.png");
            }
            return GetBitmapImage($"{iconDir.FullName}/file.png");
        }

        private static BitmapImage GetBitmapImage(string uri)
        {
            return new BitmapImage(new Uri(uri));
        }
    }
}
