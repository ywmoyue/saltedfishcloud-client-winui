using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SfcApplication.Models.Enums;

namespace SfcApplication.Models.Entities
{
    public class DiskFileInfo
    {
        public int Uid { get; set; }

        public string Name { get; set; }

        public string Md5 { get; set; }

        public FileType Type { get; set; }

        public long Size { get; set; }

        public string Node { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Suffix { get; set; }

        public bool Dir { get; set; }

        [JsonIgnore]
        public List<string> Paths { get; set; }
    }
}
