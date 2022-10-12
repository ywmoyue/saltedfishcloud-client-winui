using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;
using SfcApplication.Extensions;
using SfcApplication.Models.Enums;

namespace SfcApplication.Converters
{
    public class UserConverter
    {
        public static ImageSource GetUserAvatarImage(string userName,string avatarUrl)
        {
            var url = avatarUrl.ReplaceParameter("user", userName);
            return url.GetBitmapImage();
        }

        public static string GetUserTypeString(UserType userType)
        {
            switch (userType)
            {
                case UserType.TypeAdmin:
                    return "admin";
                case UserType.TypeCommon:
                    return "common";
            }

            return "游客";
        }
    }
}
