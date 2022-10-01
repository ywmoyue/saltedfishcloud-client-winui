using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SfcApplication.Models.Entities;

namespace SfcApplication.Models.Mappers
{
    [AutoMap(typeof(DiskFileInfo), ReverseMap = true)]
    internal class DiskFileInfoMapper:DiskFileInfo
    {
        public List<string> Paths;
    }
}
