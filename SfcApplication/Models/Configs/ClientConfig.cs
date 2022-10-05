using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Configs
{
    public class ClientConfig
    {
        public string AppName { get; set; }
        public string BaseUrl { get; set; }
        public OpenApiConfig OpenApi { get; set; }
        public string DefaultDownloadPath { get; set; }
    }
}
