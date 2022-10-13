using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;

namespace SfcApplication.Converters
{
    public class VisibilityConverter
    {
        public static Visibility NoValueToHidden(object listObject)
        {
            var list=listObject as IEnumerable<object>;
            if(list==null||!list.Any()) return Visibility.Collapsed;
            return Visibility.Visible;
        }
    }
}
