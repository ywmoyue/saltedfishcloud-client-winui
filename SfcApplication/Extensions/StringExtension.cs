using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SfcApplication.Extensions
{
    public static class StringExtension
    {
        public static string ReplaceParameter(this string source,string name,string value)
        {
            return source.Replace("{{"+name+"}}", value);
        }

        public static string UrlToPath(this string url)
        {
            return url.Replace("/","\\");
        }

        public static BitmapImage GetBitmapImage(this string uri)
        {
            return new BitmapImage(new Uri(uri));
        }
    }
}
