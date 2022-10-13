using System.Collections.Generic;

namespace SfcApplication.Models.Common
{
    public class FileListNavigatedQuery
    {
        public List<string> Paths { get; set; }
        public int UserId { get; set; } = 0;
    }
}
