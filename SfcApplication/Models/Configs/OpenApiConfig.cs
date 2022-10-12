using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Configs
{
    public class OpenApiConfig
    {
        public string DownloadFile { get; set; }
        public string GetUserToken { get; set; }
        public string GetUserInfo { get; set; }
        public string GetQuotaUsed { get; set; }
        public string GetFileList { get; set; }
        public string GetThumbnailImage { get; set; }
        public string GetUserAvatarImage { get; set; }
    }
}
