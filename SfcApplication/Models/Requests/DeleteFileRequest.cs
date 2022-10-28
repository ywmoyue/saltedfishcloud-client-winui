using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SfcApplication.Models.Requests
{
    public class DeleteFileRequest
    {
        [JsonProperty("fileName")]
        public List<string> FileNames { get; set; }
    }
}
