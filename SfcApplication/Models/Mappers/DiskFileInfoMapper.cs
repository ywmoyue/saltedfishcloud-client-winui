using System;
using System.Collections.Generic;
using AutoMapper;
using SfcApplication.Models.Entities;
using SfcApplication.Models.Enums;

namespace SfcApplication.Models.Mappers
{
    [AutoMap(typeof(DiskFileInfo), ReverseMap = true)]
    public class DiskFileInfoMapper
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

        public List<string> Paths { get; set; }
        public string BaseUrl { get; set; }
        public int UserId { get; set; }
    }
}
