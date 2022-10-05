using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Extensions
{
    public static class IEnumerableExtension
    {
        public static string GetFilePathExceptRoot(this IEnumerable<string> filePaths)
        {
            var path = "";
            for (var i = 1; i < filePaths.Count(); i++)
            {
                path += $"{filePaths.ElementAt(i)}/";
            }
            return path;
        }
    }
}
