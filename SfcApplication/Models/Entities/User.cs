﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SfcApplication.Models.Enums;

namespace SfcApplication.Models.Entities
{
    internal class User
    {
        public int Id { get; set; }

        [JsonProperty("user")]
        public string UserName { get; set; }

        public UserType Type { get; set; }
        
        public int Quota { get; set; }

        public string Email { get; set; }
    }
}
