using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Converters
{
    public class ByteSizeConverter
    {
        public static string ByteToSizeString(long size)
        {
            if (size < 1024)
            {
                return $"{size} B";
            }
            if (size < 1024 * 1024)
            {
                var kbyteSize = Math.Round(size / 1024f,2);
                return $"{kbyteSize} KB";
            }
            if (size < 1024 * 1024 * 1024)
            {
                var mbyteSize = Math.Round(size / (1024f * 1024f), 2);
                return $"{mbyteSize} MB";
            }
            var gbyteSize = Math.Round(size / (1024f * 1024f * 1024f), 2);
            return $"{gbyteSize} GB";
        }
    }
}
