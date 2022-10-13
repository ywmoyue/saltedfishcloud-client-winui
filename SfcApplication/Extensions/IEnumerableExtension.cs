using System.Collections.Generic;
using System.Linq;

namespace SfcApplication.Extensions
{
    public static class IEnumerableExtension
    {
        public static string GetFileUrlExceptRoot(this IEnumerable<string> filePaths)
        {
            var path = "";
            for (var i = 1; i < filePaths.Count(); i++)
            {
                path += $"{filePaths.ElementAt(i)}/";
            }
            return path;
        }

        public static string GetFilePathExceptRoot(this IEnumerable<string> filePaths)
        {
            var path = "";
            for (var i = 1; i < filePaths.Count(); i++)
            {
                path += $"{filePaths.ElementAt(i)}\\";
            }
            return path;
        }
    }
}
