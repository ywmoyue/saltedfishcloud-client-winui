using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SfcApplication.Models.Entities;

namespace SfcApplication.ViewModels
{
    public class UserSpacePageViewModel:BaseViewModel
    {
        public UserItemViewModel User { get; set; }
        public string AvatarUrl { get; set; }
        public string DefaultDownloadPath { get; set; }

        public bool IsRemoveDownloadedTaskWithFile { get; set; } = false;
        public bool IsRemoveDownloadingTaskWithFile { get; set; } = true;

        public QuotaUsed QuotaUsed { get; set; }
    }
}
