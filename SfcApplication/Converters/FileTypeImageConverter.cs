using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using SfcApplication.Models.Entities;
using System.Collections.ObjectModel;

namespace SfcApplication.Converters
{
    internal class FileTypeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var fileInfo = value as DiskFileInfo;
            var paths = parameter as ObservableCollection<string>;
            if (fileInfo.Dir)
            {
                return "../Assets/icon/dir.png";
            }

            switch (fileInfo.Suffix.ToLower())
            {
                case "apk":
                    return "../Assets/icon/android.png";
                case "mp3":
                case "wav":
                    return "../Assets/icon/audio.png";
                case "cs":
                case "js":
                case "java":
                    return "../Assets/icon/code.png";
                case "doc":
                case "docx":
                    return "../Assets/icon/doc.png";
                case "xls":
                case "xlsx":
                    return "../Assets/icon/excel.png";
                case "exe":
                    return "../Assets/icon/exe.png";
                case "txt":
                case "md":
                    return "../Assets/icon/txt.png";
                case "iso":
                    return "../Assets/icon/iso.png";
                case "jpg":
                case "png":
                    var path = "";
                    for (var i = 1; i < paths.Count; i++)
                    {
                        path += $"{paths[i]}/";
                    }
                    var img = new BitmapImage(new Uri($"https://disk.xiaotao2333.top:344/api/diskFile/0/content/{path}{fileInfo.Name}"));
                    return img;
                case "ppt":
                case "pptx":
                    return "../Assets/icon/ppt.png";
                case "mp4":
                    return "../Assets/icon/video.png";
                case "rar":
                case "zip":
                case "gz":
                case "7z":
                    return "../Assets/icon/zipped.png";
            }
            return "../Assets/icon/file.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
