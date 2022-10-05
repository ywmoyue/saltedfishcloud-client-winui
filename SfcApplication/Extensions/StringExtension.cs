using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Extensions
{
    public static class StringExtension
    {
        public static string ReplaceParameter(this string source,string name,string value)
        {
            return source.Replace("{{"+name+"}}", value);
        }
    }
}
