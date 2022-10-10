using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Common
{
    public class FileListNavigatedQuery
    {
        public List<string> Paths { get; set; }
        public int UserId { get; set; } = 0;
    }
}
