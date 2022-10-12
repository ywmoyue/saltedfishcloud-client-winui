using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Configs
{
    public class UserConfig
    {
        public bool IsRemoveDownloadedTaskWithFile { get; set; } = false;
        public bool IsRemoveDownloadingTaskWithFile { get; set; } = true;
    }
}
