using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SfcApplication.Models.Enums;
using AutoMapper;
using SfcApplication.Models.Common;
using SfcApplication.Models.Entities;

namespace SfcApplication.ViewModels
{
    [AutoMap(typeof(User), ReverseMap = true)]
    public class UserItemViewModel:BaseViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public UserType Type { get; set; }

        public int Quota { get; set; }

        public string Email { get; set; }
    }
}
