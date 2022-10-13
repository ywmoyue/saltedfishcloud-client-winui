using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml.Media;
using SfcApplication.Extensions;

namespace SfcApplication.Converters
{
    public class FileTypeImageConverter
    {
        public static ImageSource Convert(string suffix, string name,bool dir,List<string> paths,string md5,string imageUrl,int userId)
        {
            var appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var iconDir = new DirectoryInfo($"{appPath}/Assets/icon");
            if (dir)
            {
                return $"{iconDir.FullName}/dir.png".GetBitmapImage();
            }

            switch (suffix.ToLower())
            {
                case "apk":
                    return $"{iconDir.FullName}/android.png".GetBitmapImage();
                case "mp3":
                case "wav":
                    return $"{iconDir.FullName}/audio.png".GetBitmapImage();
                case "cs":
                case "js":
                case "java":
                    return $"{iconDir.FullName}/code.png".GetBitmapImage();
                case "doc":
                case "docx":
                    return $"{iconDir.FullName}/doc.png".GetBitmapImage();
                case "xls":
                case "xlsx":
                    return $"{iconDir.FullName}/excel.png".GetBitmapImage();
                case "exe":
                    return $"{iconDir.FullName}/exe.png".GetBitmapImage();
                case "txt":
                case "md":
                    return $"{iconDir.FullName}/txt.png".GetBitmapImage();
                case "iso":
                    return $"{iconDir.FullName}/iso.png".GetBitmapImage();
                case "jpg":
                case "png":
                case "gif":
                    var url = imageUrl.ReplaceParameter("userId", userId + "").ReplaceParameter("md5", md5)
                        .ReplaceParameter("suffix", suffix);
                    var img = url.GetBitmapImage();
                    return img;
                case "ppt":
                case "pptx":
                    return $"{iconDir.FullName}/ppt.png".GetBitmapImage();
                case "mp4":
                    return $"{iconDir.FullName}/video.png".GetBitmapImage();
                case "rar":
                case "zip":
                case "gz":
                case "7z":
                    return $"{iconDir.FullName}/zipped.png".GetBitmapImage();
            }
            return $"{iconDir.FullName}/file.png".GetBitmapImage();
        }

        public static ImageSource Convert(string suffix, bool dir)
        {
            var appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var iconDir = new DirectoryInfo($"{appPath}/Assets/icon");
            if (dir)
            {
                return $"{iconDir.FullName}/dir.png".GetBitmapImage();
            }

            switch (suffix.ToLower())
            {
                case "apk":
                    return $"{iconDir.FullName}/android.png".GetBitmapImage();
                case "mp3":
                case "wav":
                    return $"{iconDir.FullName}/audio.png".GetBitmapImage();
                case "cs":
                case "js":
                case "java":
                    return $"{iconDir.FullName}/code.png".GetBitmapImage();
                case "doc":
                case "docx":
                    return $"{iconDir.FullName}/doc.png".GetBitmapImage();
                case "xls":
                case "xlsx":
                    return $"{iconDir.FullName}/excel.png".GetBitmapImage();
                case "exe":
                    return $"{iconDir.FullName}/exe.png".GetBitmapImage();
                case "txt":
                case "md":
                    return $"{iconDir.FullName}/txt.png".GetBitmapImage();
                case "iso":
                    return $"{iconDir.FullName}/iso.png".GetBitmapImage();
                case "jpg":
                case "png":
                case "gif":
                    return $"{iconDir.FullName}/picture.png".GetBitmapImage();
                case "ppt":
                case "pptx":
                    return $"{iconDir.FullName}/ppt.png".GetBitmapImage();
                case "mp4":
                    return $"{iconDir.FullName}/video.png".GetBitmapImage();
                case "rar":
                case "zip":
                case "gz":
                case "7z":
                    return $"{iconDir.FullName}/zipped.png".GetBitmapImage();
            }
            return $"{iconDir.FullName}/file.png".GetBitmapImage();
        }
    }
}
