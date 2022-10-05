using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SfcApplication.Models.Entities;

namespace SfcApplication.Models.Mappers
{
    [AutoMap(typeof(DiskFileInfo), ReverseMap = true)]
    public class DiskFileInfoMapper:DiskFileInfo
    {
        public List<string> Paths { get; set; }
        public string BaseUrl { get; set; }
        public int UserId { get; set; }
    }
}
